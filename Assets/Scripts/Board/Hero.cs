using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour, Subject
{
    // Reference to WaypointManager
    private WaypointManager WaypointManager;
    private UIManager UIManager;

    // List of Observers (Observer design pattern)
    List<Observer> Observers = new List<Observer>();

    // Type of hero
    HeroType Type;
    bool TypeWasSet = false;

    int maxStrength;
    int strength;
    int maxWillpower;
    int willpower;
    int timeOfDay;
    int myGold;
    int numFarmers;
    bool moveCompleted;
    HeroInventory heroInventory; //need to initialize somewhere
    Waypoint myRegion;

    // Current battle invitation
    BattleInvitation BattleInvitation;

    // Battle which this hero is currently involved in
    Battle CurrentBattle;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize reference to WaypointManager
        WaypointManager = GameObject.Find("WaypointManager").GetComponent<WaypointManager>();
        UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public HeroType GetHeroType()
    {
        return Type;
    }

    public void SetHeroType(HeroType Type)
    {
        if (TypeWasSet)
        {
            Debug.LogError("Error: this Hero's type has already been set.");
            return;
        }

        this.Type = Type;
        TypeWasSet = true;

        // Initialize strength and willpower
        strength = 1;
        willpower = 7;
    }

    // Note: Used for testing and debugging purposes ONLY
    // Moves the hero to the specified location with no regards for game rules
    public void Teleport(int RegionNum)
    {
        myRegion = WaypointManager.GetWaypoint(RegionNum);
        // TODO: change transform location of hero
        Move();
    }

    public void Move()
    {
        //if (GetSelfHero() == GetCurrentTurnHero())
        //{
        //    Debug.Log("hero turn character is on wp " + GetCurrentTurnHero().GetWaypoint().GetWaypointNum());
        //    Debug.Log("hero turn character is on wp " + this.GetWaypoint().GetWaypointNum());
        //}
        Debug.Log("hero turn character is on wp " + this.GetWaypoint().GetWaypointNum());

        UIManager.onHeroMove(this);

    }

    public void pickupGold()
    {
        if (myRegion.pickupOneGold() == 1) 
        {
            myGold++;
        } else
        {
            // Can't pick up gold
        }
    }

    public void dropGold()
    {
        if (myGold >= 1)
        {
            myGold--;
            myRegion.dropOneGold();
        } else
        {
            //No Gold to drop
        }
    }

    public void pickupItem(Item item)
    {
        if(myRegion.containsItem(item))
        {
            heroInventory.addItem(item);
            myRegion.removeItem(item);
        }
        else
        {
            Debug.Log("The region does not have this item.");
        }
    }

    public void dropItem(Item item)
    {
        if (heroInventory.containsItem(item))
        {
            heroInventory.removeItem(item);
            myRegion.addItem(item);
        }
        else
        {
            Debug.Log("The hero does not have this item.");
        }
    }

    public void DrinkFromWell(int regionNum)
    {
        // TODO: avoid Teleport and fix null reference to myRegion
        Teleport(regionNum);

        if(myRegion != null)
        {
            if(myRegion.containsFullWell())
            {
                myRegion.EmptyWell();

                // Warrior special ability
                if(Type == HeroType.Warrior && willpower <= maxWillpower - 5) willpower += 5;
                
                if(willpower <= maxWillpower - 3) willpower += 3;

                else willpower = maxWillpower;
            }
            Debug.Log(Type + " now has will power "+willpower);
        }
        else
        {
            Debug.Log("Error. myRegion reference null.");
        }
    }

    // Returns whether the hero is close enought to fight the given creature
    public bool IsEligibleForBattle(Creature Creature)
    {
        bool Eligible;

        // Validate the Hero and Creature regions
        if (myRegion == null)
        {
            Debug.LogError("Error: Hero " + this + " is not on a region.");
            return false;
        }
        else if (Creature == null) return false;
        else if (Creature.GetRegion() == null)
        {
            Debug.LogError("Error: Creature " + Creature + " is not on a region.");
            return false;
        }

        Waypoint CreatureRegion = Creature.GetRegion();

        Eligible = (myRegion.Equals(CreatureRegion))                                             // Hero is on the same region as the creature
            || (WaypointManager.AreAdjacent(myRegion, CreatureRegion) && CanShootArrows());      // OR Hero is on an adjacent region to the creature, and is an archer or has a bow

        // TODO also check whether the hero has not moved during this turn

        return Eligible;
    }

    // Returns whether the hero is able to fight from an adjacent space
    private bool CanShootArrows()
    {
        return (Type == HeroType.Archer || HasBow());
    }

    // Returns whether the hero has a bow object
    private bool HasBow()
    {
        return false;       // after initialized: return myInventory.containItem(Bow);
    }

    // Tries to advance the time marker and returns whether or not the operation succeeded
    public bool AdvanceTimeMarker(int Amount)
    {
        int NewTime = timeOfDay + Amount;
        int NewWillpower = willpower;

        // Validate the new time value
        if (NewTime > 10)
        {
            Debug.Log("Cannot advance time marker; new time would exceed 10 hours.");
            return false;
        }
        else if (NewTime >= 8)
        {
            int NumOfOvertimeHours = NewTime - 7;
            NewWillpower = willpower - 2 * NumOfOvertimeHours;

            // Validate the new willpower value
            if (NewWillpower <= 0)      // Allowing overtime to bring a hero's willpower to 0 is disallowed
            {
                Debug.Log("Cannot advance time marker; overtime cannot be used if it brings a hero to 0 willpower.");
                return false;
            }
        }

        // Finalize the time marker advancement
        timeOfDay = NewTime;
        willpower = NewWillpower;

        // Notify observers; notifications can be received to prompt UI updates
        Notify("TIME");
        Notify("HERO_WILLPOWER");

        return true;
    }

    public void SetCurrentBattle(Battle OwnedBattle)
    {
        this.CurrentBattle = OwnedBattle;
    }

    public Battle GetCurrentBattle()
    {
        return CurrentBattle;
    }

    public void SendBattleInvitation(BattleInvitation Invitation)
    {
        this.BattleInvitation = Invitation;

        Notify("INVITE_STATUS");
    }

    public BattleInvitation GetBattleInvitation()
    {
        return BattleInvitation;
    }

    public void SetWaypoint(Waypoint Region)
    {
        this.myRegion = Region;
    }

    public Waypoint GetWaypoint()
    {
        return myRegion;
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

    public int getStrength()
    {
        return strength;
    }

    public int getNumFarmers()
    {
        return numFarmers;
    }

    public int getWillpower()
    {
        return willpower;
    }

    public int getGold()
    {
        return myGold;
    }
}
