using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour, Subject
{
    // Reference to WaypointManager, UIManager
    private WaypointManager WaypointManager;
    private UIManager UIManager;

    // List of Observers (Observer design pattern)
    List<Observer> Observers = new List<Observer>();

    // Type of hero
    HeroType Type;
    bool TypeWasSet = false;

    int Rank = 0;

    int maxStrength;
    int strength;
    int maxWillpower;
    int willpower;
    int timeOfDay;
    int myGold;
    int numFarmers;
    bool moveCompleted;
    HeroInventory heroInventory;
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
        maxWillpower = 20;
        myGold = 4;
        heroInventory = new HeroInventory();

        // Initialize rank
        if (Type == HeroType.Warrior) Rank = 14;
        else if (Type == HeroType.Archer) Rank = 25;
        else if (Type == HeroType.Dwarf) Rank = 7;
        else if (Type == HeroType.Wizard) Rank = 34;
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

        //show the adjacent waypoints
        // this.GetWaypoint().ShowAdjWP();

        UIManager.onHeroMove(this);

    }

    public void pickupGold()
    {
        if (myRegion.pickupOneGold() == 1)
        {
            myGold++;
        }
        else
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
        }
        else
        {
            //No Gold to drop
        }
    }

    public void pickupFarmer()
    {
        if (myRegion.pickupOneFarmer() != null)
        {
            numFarmers++;
        }
        else
        {
            // Can't pick up Farmer
        }
    }

    public void dropFarmer()
    {
        if (numFarmers >= 1)
        {
            numFarmers--;
            myRegion.dropOneFarmer();
        }
        else
        {
            //No Farmer to drop
        }
    }

    public void pickupItem(Item item)
    {
        if (myRegion.containsItem(item))
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

    public void useItem(Item item)
    {
        switch (item.type)
        {
            //case, bow then ...
            //case, .... then
        }
    }

    // Called from MerchantUIManager when items are purchased and hero has enough gold.
    public void BuyFromMerchant(Type ItemType)
    {
        // Debug.Log(ItemType);
        heroInventory.addItemByType(ItemType);
    }

    public void DrinkFromWell(int regionNum)
    {
        // TODO: avoid Teleport and fix null reference to myRegion
        Teleport(regionNum);

        if (myRegion != null)
        {
            if (myRegion.containsFullWell())
            {
                myRegion.EmptyWell();

                // Warrior special ability
                if (Type == HeroType.Warrior && willpower <= maxWillpower - 5) willpower += 5;

                if (willpower <= maxWillpower - 3) willpower += 3;

                else willpower = maxWillpower;
            }
            Debug.Log(Type + " now has will power " + willpower);
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
        bool hasBow = false;
        return heroInventory.containsItem("Bow");
        
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

    // Returns whether the hero has the black die (if they have 3 different rune stones)
    private bool HasBlackDie()
    {
        return false;            // TODO real value
    }

    // Returns the type of dice used by this hero
    public DiceType GetDiceType()
    {
        // If the hero has a black die, return it. Otherise, return regular.
        if (HasBlackDie()) return DiceType.Black;
        else return DiceType.Regular;
    }

    // Returns the number of dice used by this hero
    public int GetNumOfDice()
    {
        if (HasBlackDie()) return 1;     // A hero can only have one black die
        else
        {
            // If the hero has normal dice, find out how many
            switch (Type)
            {
                case HeroType.Warrior:
                    if (willpower <= 6) return 2;
                    else if (willpower <= 13) return 3;
                    else return 4;

                case HeroType.Archer:
                    if (willpower <= 6) return 3;
                    else if (willpower <= 13) return 4;
                    else return 5;

                case HeroType.Dwarf:
                    if (willpower <= 6) return 1;
                    else if (willpower <= 13) return 2;
                    else return 3;

                case HeroType.Wizard:
                    return 1;

                default:
                    Debug.LogError("Cannot get number of dice for unknown HeroType.");
                    return -1;
            }
        }
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

    // Increases the hero's willpower by the indicated positive amount, to a maximum of maxWillpower.
    public void IncreaseWillpower(int Amount)
    {
        if (Amount > 0)
        {
            willpower = Math.Min(willpower + Amount, maxWillpower);

            Notify("HERO_WILLPOWER");
        }
    }

    // Decreases the hero's willpower by the indicated positive amount, to a minimum of 0.
    public void DecreaseWillpower(int Amount)
    {
        if (Amount > 0)
        {
            willpower = Math.Max(willpower - Amount, 0);

            Notify("HERO_WILLPOWER");
        }
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

    // Decreases the hero's strength by the indicated positive amount, to a minimum of 1.
    public void DecreaseStrength(int Amount)
    {
        if (Amount > 0)
        {
            strength = Math.Max(strength - Amount, 1);

            Notify("HERO_STRENGTH");
        }
    }

    public int getNumFarmers()
    {
        return numFarmers;
    }

    // Destroys all farmers carried by this hero. Called when a creature steps onto the same region as a hero.
    // TODO call this when a hero moves to the same region as a creature.
    public void DestroyCarriedFarmers()
    {
        numFarmers = 0;
    }

    public int getWillpower()
    {
        return willpower;
    }

    public int getGold()
    {
        return myGold;
    }

    public int GetRank()
    {
        return Rank;
    }
}
