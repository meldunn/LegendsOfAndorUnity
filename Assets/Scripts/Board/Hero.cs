using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class Hero : MonoBehaviourPun, Subject
{
    // References to managers
    private GameManager GameManager;
    private WaypointManager WaypointManager;
    private static FogManager FogManager;
    private UIManager UIManager;

    // List of Observers (Observer design pattern)
    List<Observer> Observers = new List<Observer>();

    public int[] path;

    // Hero move speed
    private float MoveSpeed = 3f;

    // Type of hero
    HeroType Type;
    bool TypeWasSet = false;

    int Rank = 0;

    int maxStrength;
    int strength;
    int maxWillpower;
    int willpower;
    int timeOfDay;
    bool InRoosterBox = false;
    bool EndedDay = false;
    int myGold;
    int numFarmers;
    bool moveCompleted;
    public HeroInventory heroInventory;
    Waypoint myRegion;

    // Current battle invitation
    BattleInvitation BattleInvitation;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize reference to managers
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        WaypointManager = GameObject.Find("WaypointManager").GetComponent<WaypointManager>();
        UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        FogManager = GameObject.Find("FogManager").GetComponent<FogManager>();
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
        if (Type == HeroType.PrinceThorald)
        {
            strength = 4;
        } 
        else
        {
            strength = 1;
        }
        willpower = 7;
        maxWillpower = 20;
        myGold = 20;
        heroInventory = new HeroInventory();

        numFarmers = 1;

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
        // Set the correct references
        myRegion.RemoveHero(this);
        myRegion = WaypointManager.GetWaypoint(RegionNum);
        myRegion.AddHero(this);


        UIManager.onHeroMove(this);

        // Move the hero sprite
        this.transform.SetPositionAndRotation(myRegion.GetLocation(),     // Destination
            Quaternion.identity);                                         // No rotation
    }

    public void ExecuteMove()
    {
        if (path.Length > 0)
        {
            for (int i =0; i < path.Length; i++)
            {
                if (path[i] != -1)
                {
                    Debug.Log("in hero execmove, path[i] is " + path[i]);
                    myRegion.RemoveHero(this);
                    myRegion = WaypointManager.GetWaypoint(path[i]);
                    myRegion.AddHero(this);
                    //Move the hero sprite
                    this.transform.SetPositionAndRotation(myRegion.GetLocation(),     // Destination
                        Quaternion.identity);                                         // No rotation

                    //this.transform.position = Vector2.MoveTowards(this.GetLocation(),     // Self
                    //myRegion.GetLocation(),                                        // Destination
                    //MoveSpeed * Time.deltaTime);                                 // Max distance moved


                    //if on last path tile check if fog
                    if (path[i + 1] == -1)
                    {
                        Debug.Log("i == path.Length - 1 " + i);
                        FogManager.triggerFogAtWP(path[i]);

                    }

                    //return path to empty
                    path[i] = -1;
                }
            }
        }

    }

    public void Move()
    {
        HeroType SelfHeroType = GameManager.GetSelfHero().GetHeroType();

        Debug.Log("hero turn character is on wp " + this.GetWaypoint().GetWaypointNum());

        // TODO Increase max length because a hero can move further if using a wineskin
        path = new int[10]; //max len 10, reset every turn

        //initialize to have each element be -1
        for(int i =0; i < 10; i++)
        {
            path[i] = -1;
        }
        
        // If this is the moving hero's machine, show the adjacent waypoints
        if (this.Type == SelfHeroType) this.GetWaypoint().ShowAdjWP();

        UIManager.onHeroMove(this);
    }


    // Get the location of this hero
    private Vector3 GetLocation()
    {
        return transform.position;
    }

    public void ReceiveGold(int Amount)
    {
        myGold += Amount;
        Notify("HERO_STATS");
    }

    public void pickupGold()
    {
        if (myRegion.pickupOneGold() == 1)
        {
            myGold++;
            Notify("HERO_STATS");
        }
        else
        {
            //Debug.LogError(this.Type + " can't pickup Gold");
            // Can't pick up gold
        }
    }

    public void dropGold()
    {
        if (myGold >= 1)
        {
            myGold--;
            myRegion.dropOneGold();
            Notify("HERO_STATS");
        }
        else
        {
            //Debug.LogError(this.Type + " can't drop Gold");
            //No Gold to drop
        }
    }

    public void pickupFarmer()
    {
        if (myRegion.pickupOneFarmer() == 1)
        {
            numFarmers++;
            Notify("HERO_STATS");
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
            Notify("HERO_STATS");
        }
        else
        {
            //No Farmer to drop
        }
    }

    public void pickupItem(ItemType ItemType)
    {
        if (myRegion.containsItem(ItemType))
        {
            heroInventory.addItem(ItemType);
            myRegion.removeItem(ItemType);

            Notify("HERO_ITEMS");
        }
        else
        {
            Debug.Log("The region does not have this item.");
        }
    }

    public void dropItem(ItemType ItemType)
    {
        if (heroInventory.containsItem(ItemType))
        {
            heroInventory.removeItem(ItemType);
            myRegion.addItem(ItemType);

            Notify("HERO_ITEMS");
        }
        else
        {
            Debug.Log("The hero does not have this item.");
        }
    }

    public void useItem(Item item)
    {
        /*
        switch (item.type)
        {
            //case, bow then ...
            //case, .... then
        }
        */
    }

    // Called from MerchantUIManager when items are purchased and hero has enough gold.
    public void BuyFromMerchant(ItemType ItemType)
    {
        if(ItemType == ItemType.StrengthPoints)
        {
            if(strength < maxStrength) strength ++;
            Debug.Log("Updating Strength Points");
        }
        else heroInventory.addItem(ItemType);

        Notify("HERO_ITEMS");
    }

    public Dictionary<ItemType, int> GetInventory()
    {
        return heroInventory.getInventory();
    }

    public void GiveItemFromTrade(ItemType Item)
    {
        heroInventory.removeItem(Item);

        Notify("HERO_ITEMS");
    }
    public void ReceiveItemFromTrade(ItemType Item)
    {
        heroInventory.addItem(Item);

        Notify("HERO_ITEMS");
    }

    public void DrinkFromWell(int regionNum)
    {
        if (myRegion != null)
        {
            if (myRegion.containsFullWell())
            {
                // RPC: EmptyWell(); Setting the willpower
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

    public Waypoint GetCurrentRegion()
    {
        return myRegion;
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
    public bool HasBow()
    {
        return heroInventory.containsItem(ItemType.Bow);
    }

    // Returns whether the hero has a helm object
    public bool HasHelm()
    {
        return heroInventory.containsItem(ItemType.Helm);
    }

    // Tries to advance the time marker and returns whether or not the operation succeeded
    public bool AdvanceTimeMarker(int Amount)
    {
        int NewTime = timeOfDay + Amount;
        int NewWillpower = willpower;

        // Validate the new time value
        if (NewTime > 10)
        {
            Debug.LogWarning("Cannot advance time marker; new time would exceed 10 hours.");
            return false;
        }
        else if (NewTime >= 8)
        {
            int NumOfOvertimeHours = NewTime - Math.Max(timeOfDay, 7);
            NewWillpower = willpower - 2 * NumOfOvertimeHours;

            // Validate the new willpower value
            if (NewWillpower <= 0)      // Allowing overtime to bring a hero's willpower to 0 is disallowed
            {
                Debug.LogWarning("Cannot advance time marker; overtime cannot be used if it brings a hero to 0 willpower.");
                return false;
            }
        }

        // Reset the rooster box value
        if (NewTime > 0) InRoosterBox = false;

        // Finalize the time marker advancement
        timeOfDay = NewTime;
        willpower = NewWillpower;

        // Notify observers; notifications can be received to prompt UI updates
        Notify("HERO_TIME");
        Notify("HERO_WILLPOWER");

        return true;
    }

    // Checks whether the hero can advance their time marker by the specified amount
    public bool CanAdvanceTimeMarker(int Amount)
    {
        int NewTime = timeOfDay + Amount;
        int NewWillpower = willpower;

        // Validate the new time value
        if (NewTime > 10) return false;
        
        else if (NewTime >= 8)
        {
            int NumOfOvertimeHours = NewTime - Math.Max(timeOfDay, 7);
            NewWillpower = willpower - 2 * NumOfOvertimeHours;

            // Validate the new willpower value
            if (NewWillpower <= 0) return false;
        }
        return true;
    }

    public int GetTimeOfDay()
    {
        return timeOfDay;
    }

    public void EndHeroDay(bool InRoosterBox)
    {
        this.InRoosterBox = InRoosterBox;
        EndedDay = true;
        timeOfDay = 0;

        // Notify observers
        Notify("HERO_TIME");
    }

    public void StartHeroDay()
    {
        EndedDay = false;
    }

    // Returns true if the hero was the first to end their day
    public bool IsInRoosterBox()
    {
        return InRoosterBox;
    }

    // Returns true if the hero has ended their day
    public bool HasEndedDay()
    {
        return EndedDay;
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
