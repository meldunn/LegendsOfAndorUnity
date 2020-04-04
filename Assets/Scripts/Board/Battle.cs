using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Creature GetCreature()
    {
        return Creature;
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
