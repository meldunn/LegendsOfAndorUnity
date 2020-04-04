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

    // Used to start the battle if all participants accepted their invitations
    public void Start()
    {
        Status = BattleStatus.Started;
        Notify("STARTED");
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

    public bool WasCancelled()
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
        foreach (Observer o in Observers)
        {
            o.UpdateData(Category);
        }
    }
}
