﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleStatus { Pending, Started, Won, Lost, Cancelled }

public class Battle : Subject
{
    // Managers
    private GameManager GameManager;
    private CreatureManager CreatureManager;
    private UIManager UIManager;

    // List of Observers (Observer design pattern)
    private List<Observer> Observers = new List<Observer>();

    // Hero that started the battle
    private Hero BattleStarter;

    // Creature being fought
    private Creature Creature;

    // Heroes participating in the battle, including the battle starter (but not including Thorald)
    private List<Hero> Participants = new List<Hero>(1);

    // Heroes that the battle starter wants to invite to the battle
    private List<Hero> HeroesToInvite = new List<Hero>();

    // Invitations sent to the other heroes
    private List<BattleInvitation> Invitations = new List<BattleInvitation>();

    // Whether the invitations have been sent. This value will become true after sending was triggered (even if the hero is fighting alone and there were none to send).
    private bool InvitationsSent = false;

    // List of rounds in the battle
    private List<BattleRound> Rounds = new List<BattleRound>();

    // Status of this battle
    private BattleStatus Status = BattleStatus.Pending;

    // Hero whose turn it is within the battle
    private Hero TurnHolder;

    // List of heroes who have agreed to start a new round
    private List<Hero> ConsentToContinue = new List<Hero>();

    // Constructor
    public Battle(Hero BattleStarter, Creature Creature)        // Initialize a battle without any other participants
    {
        this.BattleStarter = BattleStarter;
        this.Creature = Creature;

        // Managers
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        CreatureManager = GameObject.Find("CreatureManager").GetComponent<CreatureManager>();
        UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();

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

    // Used to cancel a battle being started if not all participants accepted their invitation
    public void CancelStart()
    {
        Status = BattleStatus.Cancelled;
        Notify("START_CANCELLED");
    }

    // Tests if the battle is ready to start, and starts it if it is
    public void TestToStart()
    {
        int NumInvites = GetNumInvites();
        int NumAccepted = GetNumInvitesAccepted();
        int NumDeclined = GetNumInvitesDeclined();

        if (NumDeclined > 0) CancelStart();
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

        Notify("STARTED");
        Notify("BATTLE_TURN");
    }

    // Ends the battle
    public void End()
    {
        // Go to the next hero turn in the main game
        GameManager.GoToNextHeroTurn();
    }

    // Tests whether the battle has been won, and if so, triggers winning
    private void TestWon()
    {
        // A battle is won when the creature has 0 willpower
        if (Creature.GetWillpower() == 0) Win();
    }

    // Wins the battle
    private void Win()
    {
        // Set the status as won
        Status = BattleStatus.Won;

        // Mark the creature as defeated and move it to region 80
        Creature.Defeat();

        this.End();

        // Display the division of winnings screen
        int Winnings = Creature.GetWinnings();
        if (Winnings > 0) UIManager.GetDivideBattleResourcesPanel().DivideResources(Participants[0].GetHeroType(), Winnings, new List<Hero>(Participants));

        Notify("BATTLE_WON");
    }

    // Tests whether the battle has been lost, and if so, triggers losing
    private void TestLost()
    {
        bool Lost = true;

        // A battle is lost when all participants have 0 willpower
        foreach (Hero Participant in Participants)
        {
            if (Participant.getWillpower() > 0) Lost = false;
        }

        if (Lost) Lose();
    }

    // Loses the battle
    private void Lose()
    {
        // Set the status as lost
        Status = BattleStatus.Lost;

        // Deduct one strength point and award 3 willpower to all remaining heroes
        foreach (Hero Participant in Participants)
        {
            Participant.DecreaseStrength(1);
            Participant.IncreaseWillpower(3);
        }

        // Reset the creature's willpower
        Creature.ResetWillpower();

        this.End();

        Notify("BATTLE_LOST");
    }

    // Cancels the battle in progress
    private void CancelBattleInProgress()
    {
        // Set the status as cancelled
        Status = BattleStatus.Cancelled;

        // Reset the creature's willpower
        Creature.ResetWillpower();

        this.End();

        Notify("BATTLE_CANCELLED");
    }

    // Tests whether a hero should be removed from the battle and if so, kicks them out
    private void TestKickOutHero(Hero Hero)
    {
        // Kick them out if they have no more willpower or time
        if (Hero.getWillpower() == 0 || !Hero.CanAdvanceTimeMarker(1)) KickOutHero(Hero);
    }

    // Removes a hero from the battle
    private void KickOutHero(Hero Hero)
    {
        Participants.Remove(Hero);

        // Deduct one strength point and award 3 willpower to the kicked out hero
        Hero.DecreaseStrength(1);
        Hero.IncreaseWillpower(3);

        Notify("BATTLE_PARTICIPANTS");

        // Test whether the kicked out hero was the last; if so, lose the battle
        if (Participants.Count == 0) Lose();
    }

    // Removes the target hero from the battle with no penalty (based on a hero choosing to leave)
    public void LeaveHero(Hero Hero)
    {
        Participants.Remove(Hero);

        Notify("BATTLE_PARTICIPANTS");

        if (Participants.Count == 0) CancelBattleInProgress();
        else TestGoToNextRound();
    }

    // Launches the roll for the specified hero. Returns true if the roll is new; false if it is continued (for archer / bow user).
    public bool Roll(Hero Hero)
    {
        bool IsNewRoll = GetCurrentRound().RollHeroDice(Hero);

        Notify("ROLL");

        return IsNewRoll;
    }

    // Sets the roll for the specified hero (used to set a copy of a roll made on another machine)
    public void SetHeroRoll(Hero Hero, Roll Roll)
    {
        GetCurrentRound().SetHeroRoll(Hero, Roll);

        Notify("ROLL");
    }

    // Sets the roll for the creature (used to set a copy of a roll made on another machine)
    public void SetCreatureRoll(Roll Roll)
    {
        GetCurrentRound().SetCreatureRoll(Roll);

        Notify("ROLL");
    }

    // Updates the values of the roll for the specified hero (used to update a copy of a roll made on another machine)
    public void UpdateRollValues(Hero Hero, int[] RollValues)
    {
        // Get a reference to the hero roll
        Roll CurrentRoll = GetRoll(Hero);

        // Update the roll values
        CurrentRoll.SetValues(RollValues);

        Notify("ROLL");
    }

    // Rolls the dice for the creature
    public void CreatureRoll()
    {
        GetCurrentRound().RollCreatureDice();

        Notify("ROLL");
    }

    // Flips the die at the specified index for the specified hero
    public void FlipDie(Hero Hero, int DieIndex)
    {
        GetCurrentRound().FlipDie(Hero, DieIndex);

        Notify("ROLL");
    }

    // Triggers taking damage after a creature roll
    public void TakeDamage()
    {
        // Determine who will take damage
        int HeroLostWP = GetCurrentRound().GetHeroLostWillpower();
        int CreatureLostWP = GetCurrentRound().GetCreatureLostWillpower();

        // Take damage
        foreach (Hero Participant in Participants)
        {
            if (HeroLostWP > 0) Participant.DecreaseWillpower(HeroLostWP);
        }
        if (CreatureLostWP > 0) Creature.DecreaseWillpower(CreatureLostWP);

        // Update UI
        Notify("WILLPOWER");

        TestWon();
        TestLost();
    }
    
    // Finalizes the specified hero's roll
    public void FinalizeRoll(Hero Hero)
    {
        GetCurrentRound().FinalizeRoll(Hero);
    }

    // Returns whether the round is done (for heroes)
    public bool RoundIsDoneForHeroes()
    {
        return GetCurrentRound().IsDone();
    }

    // Returns whether the creature has rolled
    public bool CreatureHasRolled()
    {
        return GetCurrentRound().GetCreatureRollValues().Length != 0;
    }

    // Marks whether the given hero agrees to go to the next round, and proceeds if all heroes have agreed
    public void ExpressConsentToContinue(Hero Hero)
    {
        // Remove any existing consent
        ConsentToContinue.Remove(Hero);

        // Add the new consent
        ConsentToContinue.Add(Hero);

        bool GoingToNextRound = TestGoToNextRound();
        if (!GoingToNextRound) Notify("ROLL");
    }

    // Tests whether the go to the next round, and proceeds if this is the case
    private bool TestGoToNextRound()
    {
        bool Went = ConsentToContinue.Count == Participants.Count;
        if (Went) GoToNextRound();
        return Went;
    }

    // Moves this battle to the next round
    public void GoToNextRound()
    {
        // Clear the agreements to go to the next round
        ConsentToContinue.Clear();

        // Kick out any participants who have reached 0 willpower or who can't advance their time marker anymore
        var ParticipantsCopy = new List<Hero>(Participants);        // Use a copy to prevent errors removing elements while iterating
        foreach (Hero Participant in ParticipantsCopy)
        {
            TestKickOutHero(Participant);
        }

        // Create a new round with the remaining participants
        Rounds.Add(new BattleRound(Creature, Participants));

        // TODO advance time marker

        GoToNextTurn();
    }

    // Passes the roll turn to the next hero in the order
    public void GoToNextTurn()
    {
        // Initialize the battle starter as the first turn holder if this is the first round
        if (TurnHolder == null) TurnHolder = BattleStarter;

        // Otherwise, advance the battle turn
        else
        {
            if (Participants.Count == 0)
            {
                TurnHolder = null;
                return;
            }

            Hero NewTurnHolder = TurnHolder;

            do
            {
                NewTurnHolder = GameManager.GetTurnHeroAfter(NewTurnHolder);
            }
            while (Participants.IndexOf(NewTurnHolder) == -1);

            TurnHolder = NewTurnHolder;

            Notify("BATTLE_TURN");
        }
    }

    public Roll GetRoll(Hero Hero)
    {
        return GetCurrentRound().GetRoll(Hero);
    }

    public List<Hero> GetParticipants()
    {
        return Participants;
    }

    // Returns whether a hero is participating in the battle
    public bool IsParticipating(Hero Hero)
    {
        return Participants.IndexOf(Hero) != -1;
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
        return Status == BattleStatus.Won || Status == BattleStatus.Lost || Status == BattleStatus.Cancelled;
    }

    public bool IsCancelled()
    {
        return Status == BattleStatus.Cancelled;
    }

    public bool IsWon()
    {
        return Status == BattleStatus.Won;
    }

    public bool IsLost()
    {
        return Status == BattleStatus.Lost;
    }

    // Returns the battle round currently in progress
    private BattleRound GetCurrentRound()
    {
        if (Rounds.Count == 0) return null;
        else return Rounds[Rounds.Count - 1];        // The current round is the last one in the list
    }

    public int GetRoundNum()
    {
        return Rounds.Count;
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

    public bool HasConsentedToContinue(Hero Hero)
    {
        return ConsentToContinue.IndexOf(Hero) != -1;
    }

    // Returns true if at least one but not all heroes have consented to advance to the next round (used to display a spinner)
    public bool WaitingOnConsentToContinue()
    {
        return ConsentToContinue.Count >= 1 && ConsentToContinue.Count < Participants.Count;
    }

    // Whether Thorald is participating in this battle
    public bool ThoraldIsParticipating()
    {
        if (GetCurrentRound() == null) Debug.LogError("The current battle has no rounds");
        return GetCurrentRound().ThoraldIsParticipating();
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
