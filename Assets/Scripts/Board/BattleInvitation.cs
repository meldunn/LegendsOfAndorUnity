using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InvitationStatus { Pending, Accepted, Declined }

public class BattleInvitation
{
    // The battle
    Battle Battle;

    // The hero being invited
    Hero MyHero;

    // Whether the hero has accepted to join the battle
    InvitationStatus Status = InvitationStatus.Pending;

    // Constructor
    public BattleInvitation(Battle Battle, Hero MyHero)
    {
        this.Battle = Battle;
        this.MyHero = MyHero;
    }

    public Hero GetHero()
    {
        return MyHero;
    }

    public Creature GetCreature()
    {
        return Battle.GetCreature();
    }

    public Hero GetBattleStarter()
    {
        return Battle.GetBattleStarter();
    }

    public void Accept()
    {
        Status = InvitationStatus.Accepted;
        NotifyHero();
    }

    public void Decline()
    {
        Status = InvitationStatus.Declined;
        NotifyHero();
    }

    public bool IsPending()
    {
        return Status == InvitationStatus.Pending;
    }

    public bool WasAccepted()
    {
        return Status == InvitationStatus.Accepted;
    }

    public bool WasDeclined()
    {
        return Status == InvitationStatus.Declined;
    }

    public void NotifyHero()
    {
        MyHero.Notify("INVITE_STATUS");
    }
}
