using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleStatus { Pending, Started, Finished, Cancelled }

public class Battle : Subject
{
    // Managers
    private GameManager GameManager;
    private CreatureManager CreatureManager;

    // List of Observers (Observer design pattern)
    List<Observer> Observers = new List<Observer>();

    // Hero that started the battle
    Hero BattleStarter;

    // Creature being fought
    Creature Creature;

    // Heroes participating in the battle, including the battle starter
    List<Hero> Participants = new List<Hero>(1);

    // Heroes that the battle starter wants to invite to the battle
    List<Hero> HeroesToInvite = new List<Hero>();

    // Invitations sent to the other heroes
    List<BattleInvitation> Invitations = new List<BattleInvitation>();

    // Whether the invitations have been sent. This value will become true after sending was triggered (even if the hero is fighting alone and there were none to send).
    bool InvitationsSent = false;
    
    // List of rounds in the battle
    List<BattleRound> Rounds = new List<BattleRound>();

    // Status of this battle
    BattleStatus Status = BattleStatus.Pending;

    // Hero whose turn it is within the battle
    Hero TurnHolder;

    // Constructor
    public Battle(Hero BattleStarter, Creature Creature)        // Initialize a battle without any other participants
    {
        this.BattleStarter = BattleStarter;
        this.Creature = Creature;

        // Managers
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        CreatureManager = GameObject.Find("CreatureManager").GetComponent<CreatureManager>();

        // Add the battle starter as a participant
        Participants.Add(BattleStarter);
    }

    // Adds a hero to the list of participants to invite
    public void AddHeroToInvite(Hero Hero)
    {
        HeroesToInvite.Add(Hero);
    }

    // Removes a hero from the list of participants to invite
    public void RemoveHeroToInvite(Hero Hero)
    {
        HeroesToInvite.Remove(Hero);
    }

    public List<BattleInvitation> GetInvitations()
    {
        return Invitations;
    }

    public bool HasHeroesToInvite()
    {
        return (HeroesToInvite.Count > 0);
    }

    // Sends the invitations
    public void SendInvitations()
    {
        foreach (Hero InvitedHero in HeroesToInvite)
        {
            BattleInvitation Invitation = new BattleInvitation(this, InvitedHero);
            InvitedHero.SendBattleInvitation(Invitation);
            Invitations.Add(Invitation);
        }
        InvitationsSent = true;

        TestToStart();
    }

    public bool InvitationsWereSent()
    {
        return InvitationsSent;
    }

    // Used to cancel a battle if not all participants accepted their invitation
    public void Cancel()
    {
        Status = BattleStatus.Cancelled;
        Notify("CANCELLED");
    }

    // Tests if the battle is ready to start, and starts it if it is
    public void TestToStart()
    {
        int NumInvites = GetNumInvites();
        int NumAccepted = GetNumInvitesAccepted();
        int NumDeclined = GetNumInvitesDeclined();

        if (NumDeclined > 0) Cancel();
        else if (NumAccepted == NumInvites && InvitationsWereSent()) Start();
    }

    // Returns the number of invitations for this battle
    public int GetNumInvites()
    {
        return Invitations.Count;
    }

    // Returns the number of invitations that are pending for this battle
    public int GetNumInvitesPending()
    {
        int Total = 0;

        foreach (BattleInvitation Invite in Invitations)
        {
            if (Invite.IsPending()) Total += 1;
        }
        return Total;
    }

    // Returns the number of invitations were accepted for this battle
    public int GetNumInvitesAccepted()
    {
        int Total = 0;

        foreach (BattleInvitation Invite in Invitations)
        {
            if (Invite.WasAccepted()) Total += 1;
        }
        return Total;
    }

    // Returns the number of invitations that were declined for this battle
    public int GetNumInvitesDeclined()
    {
        int Total = 0;

        foreach (BattleInvitation Invite in Invitations)
        {
            if (Invite.WasDeclined()) Total += 1;
        }
        return Total;
    }

    // Used to start the battle if all participants accepted their invitations
    public void Start()
    {
        // Set the status as started
        Status = BattleStatus.Started;

        // Register all invited heroes who accepted as participants (it should be all of them)
        foreach (BattleInvitation Invite in Invitations)
        {
            Hero InvitedHero = Invite.GetHero();

            if (Invite.WasAccepted()) Participants.Add(InvitedHero);
        }

        // Create the first battle round
        GoToNextRound();

        // Initialize the battle starter as the first turn holder
        TurnHolder = BattleStarter;

        // Register this as the current battle in progress in CreatureManager
        CreatureManager.SetCurrentBattle(this);
        
        Notify("STARTED");
        Notify("BATTLE_TURN");
    }

    // Ends the battle
    public void End()
    {
        // Set the status as finished
        Status = BattleStatus.Finished;

        // Un-register this as the current battle in progress in CreatureManager
        CreatureManager.SetCurrentBattle(null);

        Notify("ENDED");
    }

    // Launches the roll for specified hero
    public void Roll(Hero Hero)
    {
        GetCurrentRound().RollHeroDice(Hero);

        Notify("ROLL");
    }

    // Advances the turn in the battle (either within a round, or by moving to the next round)
    public void Next()
    {
        // Finalize the hero roll (useful for the archer)
        GetCurrentRound().FinalizeRoll(TurnHolder);

        // If the round is done, let the creature roll and go to the next round
        if (GetCurrentRound().IsDone())
        {
            // CreatureRoll();      // TODO
            GoToNextRound();
        }
        
        GoToNextTurn();
    }

    // Moves this battle to the next round
    private void GoToNextRound()
    {
        Rounds.Add(new BattleRound(Creature, Participants));
    }

    // Passes the roll turn to the next hero in the order
    private void GoToNextTurn()
    {
        Hero NewTurnHolder = TurnHolder;

        do
        {
            NewTurnHolder = GameManager.GetTurnHeroAfter(NewTurnHolder);
        }
        while (Participants.IndexOf(NewTurnHolder) == -1);

        TurnHolder = NewTurnHolder;

        Notify("BATTLE_TURN");
    }

    public Roll GetRoll(Hero Hero)
    {
        return GetCurrentRound().GetRoll(Hero);
    }

    public List<Hero> GetParticipants()
    {
        return Participants;
    }

    public Creature GetCreature()
    {
        return Creature;
    }

    public Hero GetBattleStarter()
    {
        return BattleStarter;
    }

    public Hero GetTurnHolder()
    {
        return TurnHolder;
    }

    // Returns whether the battle was declined by someone
    public bool DeclinedBySomeone()
    {
        foreach (BattleInvitation Invite in Invitations)
        {
            if (Invite.WasDeclined()) return true;
        }
        return false;
    }

    public bool IsPending()
    {
        return Status == BattleStatus.Pending;
    }

    public bool IsStarted()
    {
        return Status == BattleStatus.Started;
    }

    public bool IsFinished()
    {
        return Status == BattleStatus.Finished;
    }

    public bool IsCancelled()
    {
        return Status == BattleStatus.Cancelled;
    }

    // Returns the battle round currently in progress
    private BattleRound GetCurrentRound()
    {
        return Rounds[Rounds.Count - 1];        // The current round is the last one in the list
    }

    public bool RoundIsDone()
    {
        return GetCurrentRound().IsDone();
    }

    public bool HasStartedRoll(Hero Hero)
    {
        return GetCurrentRound().HasStartedRoll(Hero);
    }

    public bool HasFinishedRoll(Hero Hero)
    {
        return GetCurrentRound().HasFinishedRoll(Hero);
    }

    public bool WizardCanFlipDie()
    {
        return !GetCurrentRound().WizardHasFlippedDie();
    }

    public int GetLatestRollValue(Hero Hero)
    {
        return GetCurrentRound().GetRollValue(Hero);
    }

    public int GetLatestCreatureRollValue()
    {
        return GetCurrentRound().GetCreatureRollValue();
    }

    public Roll GetLatestHeroRoll()
    {
        if (TurnHolder == null) return null;
        return GetCurrentRound().GetRoll(TurnHolder);
    }

    public Roll GetLatestCreatureRoll()
    {
        return GetCurrentRound().GetCreatureRoll();
    }

    public int[] GetLatestHeroRollValues()
    {
        if (TurnHolder == null) return new int[0];
        return GetCurrentRound().GetHeroRollValues(TurnHolder);
    }

    public int[] GetLatestCreatureRollValues()
    {
        return GetCurrentRound().GetCreatureRollValues();
    }

    public DiceType GetLatestHeroRollDiceType()
    {
        if (TurnHolder == null) return DiceType.Regular;
        return GetCurrentRound().GetRollDiceType(TurnHolder);
    }

    public DiceType GetLatestCreatureRollDiceType()
    {
        return GetCurrentRound().GetCreatureRollDiceType();
    }

    public int GetLatestHeroBattleValue()
    {
        return GetCurrentRound().GetHeroBattleValue();
    }

    public int GetLatestCreatureBattleValue()
    {
        return GetCurrentRound().GetCreatureBattleValue();
    }

    public int GetHeroLostWillpower()
    {
        return GetCurrentRound().GetHeroLostWillpower();
    }

    public int GetCreatureLostWillpower()
    {
        return GetCurrentRound().GetCreatureLostWillpower();
    }

    // Used in Observer design pattern
    public void Attach(Observer o)
    {
        Observers.Add(o);
    }

    // Used in Observer design pattern
    public void Detach(Observer o)
    {
        Observers.Remove(o);
    }

    // Used in Observer design pattern
    public void Notify(string Category)
    {
        // Iterate through a copy of the observer list in case observers detach themselves during notify
        var ObserversCopy = new List<Observer>(Observers);

        foreach (Observer o in ObserversCopy)
        {
            o.UpdateData(Category);
        }
    }
}
