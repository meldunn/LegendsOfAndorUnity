using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    // Reference to WaypointManager
    private WaypointManager WaypointManager;
    private UIManager UIManager;

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
    }

    // Note: Used for testing and debugging purposes ONLY
    // Moves the hero to the specified location with no regards for game rules
    public void Teleport(int RegionNum)
    {
        myRegion = WaypointManager.GetWaypoint(RegionNum);
        // TODO: change transform location of hero
        HeroMoveUI();

        //Debug.Log("New Region is now "+ myRegion.GetWaypointNum());
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

    public void DrinkFromWell(int regionNum)
    {
        // TODO: avoid Teleport and fix null reference to myRegion
        Teleport(regionNum);
        if(myRegion != null)
        {
            if(myRegion.containsFullWell())
            {
                // willpower ++;
                myRegion.EmptyWell();
            }
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

    public void SetWaypoint(Waypoint Region)
    {
        this.myRegion = Region;
    }

    public Waypoint GetWaypoint()
    {
        return myRegion;
    }

    private void HeroMoveUI()
    {
        UIManager.onHeroMove(this);
    }
}
