using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointCastle : Waypoint, Subject
{
    // References to managers
    private GameManager GameManager;

    // List of Observers (Observer design pattern)
    private List<Observer> Observers = new List<Observer>();

    // The creatures that have entered the castle
    private List<Creature> Creatures = new List<Creature>();

    // The number of basic golden shields available (not including farmer shields)
    private int NumBasicShields;

    // The number of farmer shields available
    private int NumFarmerShields;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
           
    }

    public void Initialize(int NumPlayers)
    {
        // Initialize reference to WaypointManager
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Initialize the starting number of golden shields
        int NumGoldenShields = 0;

        if (NumPlayers == 2) NumGoldenShields = 3;
        else if (NumPlayers == 3) NumGoldenShields = 2;
        else if (NumPlayers == 4) NumGoldenShields = 1;
        else
        {
            Debug.LogError("Error: Invalid number of players; cannot initialize the castle's golden shields.");
        }

        this.NumBasicShields = NumGoldenShields;
    }

    public override void dropOneFarmer()
    {
        base.dropOneFarmer();
        NumFarmerShields++;

        Notify("CASTLE_FARMER");
    }

    public void CreatureEnterCastle(Creature Creature)
    {
        Creatures.Add(Creature);

        if (ShieldOverflow()) GameManager.LoseGame(LoseReason.Castle);

        Notify("CASTLE_CREATURE");
    }

    public int GetNumBasicShields()
    {
        return NumBasicShields;
    }

    public int GetNumFarmerShields()
    {
        return NumFarmerShields;
    }
    
    // Returns the total number of shields (regular + farmer)
    public int GetNumShields()
    {
        return GetNumBasicShields() + GetNumFarmerShields();
    }

    public List<Creature> GetCreatures()
    {
        return Creatures;
    }

    // Returns whether the castle has been overtaken by creatures
    public bool ShieldOverflow()
    {
        return Creatures.Count > GetNumShields();
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
