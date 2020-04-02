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

    // Constructor
    public Battle(Hero BattleStarter, Creature Creature)        // Initialize a battle without any other participants
    {
        this.BattleStarter = BattleStarter;
        this.Creature = Creature;
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
