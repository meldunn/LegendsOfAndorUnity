using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleStatus { Pending, Started, Finished, Cancelled }

public class Battle : Subject
{
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
    
    // Status of this battle
    BattleStatus Status = BattleStatus.Pending;

    // Constructor
    public Battle(Hero BattleStarter, Creature Creature)        // Initialize a battle without any other participants
    {
        this.BattleStarter = BattleStarter;
        this.Creature = Creature;

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

        Notify("STARTED");
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
