using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    // Reference to managers
    private WaypointManager WaypointManager;
    private CreatureManager CreatureManager;
    private GameManager GameManager;
    private NarratorManager NarratorManager;

    // Creature type (Gor, Skral, Wardrak, etc.)
    private CreatureType Type;

    // Willpower
    int Willpower;
    int MaxWillpower;

    // Strength
    int Strength;

    // Flag that tracks whether the creature is currently moving
    private bool IsMoving = false;

    // Creature move speed
    private float MoveSpeed = 3f;

    // Current location of the creature, or, its destination if it is currently moving
    private Waypoint Region;

    // Callback to be called after moving
    private Action Callback;

    // Whether the creature was defeated
    private bool Defeated = false;

    // Start is called before the first frame update
    void Start()
    {
        // Not used to ensure all references are available in the right order.
        // CreatureManager calls Initialize instead.
    }

    // Update is called once per frame
    void Update()
    {
        // If the creature is supposed to be moving, move it closer to its target location
        if (IsMoving) Move();
    }
    
    public void Initialize()
    {
        // Initialize reference to managers
        WaypointManager = GameObject.Find("WaypointManager").GetComponent<WaypointManager>();
        CreatureManager = GameObject.Find("CreatureManager").GetComponent<CreatureManager>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        NarratorManager = GameObject.Find("NarratorManager").GetComponent<NarratorManager>();
    }

    public CreatureType GetCreatureType()
    {
        return Type;
    }

    public void SetType(CreatureType Type)
    {
        this.Type = Type;

        // Initialize willpower, max willpower and strength
        switch (Type)
        {
            case CreatureType.Gor:
            case CreatureType.HerbGor:
                MaxWillpower = 4;
                Strength = 2;
                break; 

            case CreatureType.Skral:
                MaxWillpower = 6;
                Strength = 6;
                break;

            case CreatureType.Wardrak:
                MaxWillpower = 7;
                Strength = 10;
                break;

            case CreatureType.TowerSkral:

                MaxWillpower = 6;

                int NumPlayers = GameManager.GetNumOfPlayers();
                DifficultyLevel Difficulty = GameManager.GetDifficulty();

                if (Difficulty == DifficultyLevel.Easy)
                {
                    if (NumPlayers == 2) Strength = 10;
                    else if (NumPlayers == 3) Strength = 20;
                    else if (NumPlayers == 4) Strength = 30;
                }
                else if (Difficulty == DifficultyLevel.Normal)
                {
                    if (NumPlayers == 2) Strength = 20;
                    else if (NumPlayers == 3) Strength = 30;
                    else if (NumPlayers == 4) Strength = 40;
                }
                else Debug.LogError("Could not initialize tower skral for " + NumPlayers + " players with difficulty level " + Difficulty + ".");
                
                break;

            default:
                Debug.LogError("Cannot initialize willpower for unknown CreatureType.");
                break;
        }

        Willpower = MaxWillpower;
    }

    // Starts the creature moving towards the target region
    public void StartAdvancing(Action Callback)
    {
        // Debug.Log("Start advancing called on " + this + " from "+ Region.GetWaypointNum());

        // Check whether the creature is already moving
        if (IsMoving)
        {
            Debug.LogError("Cannot advance creature; creature is already moving.");
            return;
        }

        // Remove the old region's reference to this creature
        if (Region.GetCreature() == this) Region.SetCreature(null);             // Important: putting this in ContinueAdvancing causes errors (makes some creatures partially vanish from recognition)

        // Store the callback to be called when the creature is done moving
        this.Callback = Callback;

        ContinueAdvancing();
    }

    // Second part of the advancing process (choosing a target destination)
    // This part is in a separate method because it needs to be repeated for creatures who find another creature on their target destination
    private void ContinueAdvancing()
    {
        // Find the right region to which to advance this creature
        // First, get the next region according to the creature advancement arrows
        Waypoint NextRegion = WaypointManager.GetNext(Region);

        // Check whether the next region is a valid destination
        if (NextRegion != null)
        {
            // Set the target location reference for the creature
            Region = NextRegion;

            // Debug.Log("Advancing to target " + Region.GetWaypointNum());

            // Start moving
            IsMoving = true;
        }
        // If the creature can't move, call the callback to let the next creature advance.
        else
        {
            // Debug.Log("Cannot advance " + this + " from " + Region.GetWaypointNum());
            if (Callback != null) Callback();
        }
    }

    private void Move()
    {
        // Move algorithm adapted from Alexander Zotov's Unity 2D Board Game Tutorial: https://www.youtube.com/watch?v=W8ielU8iURI

        // Debug.Log("Move" + this + " to " + Region.GetWaypointNum());

        if (!HasReachedLocation())
        {
            // Debug.Log("Has not reached location");

            transform.position = Vector2.MoveTowards(this.GetLocation(),     // Self
                Region.GetLocation(),                                        // Destination
                MoveSpeed * Time.deltaTime);                                 // Max distance moved
        }
        else
        {
            // Debug.Log("Has reached location");

            WaypointCastle Castle = WaypointManager.GetCastle();
            Creature OtherRegionCreature = Region.GetCreature();

            // Check whether the creature has entered the castle
            if (Region.Equals(Castle))
            {
                // Hide the creature icon
                this.gameObject.SetActive(false);

                Castle.CreatureEnterCastle(this);

                IsMoving = false;
                if (Callback != null) Callback();        // When this creature is done advancing, let the next creature advance
            }
            else
            {
                // If the creature has entered a regular region, destroy all farmers standing on it, or carried by heroes on it
                Region.DestroyFarmers();
                Region.DestroyAllFarmersCarriedByHeroes();

                // Check whether this region was already occupied (if it isn't the castle). If so, keep moving.
                if (OtherRegionCreature != null)
                {
                    // Debug.Log("This creature will move again because there is already a creature on " + Region.GetWaypointNum());
                    IsMoving = false;   // To allow the creature to move again
                    ContinueAdvancing();
                }
                else
                {
                    // Debug.Log("This creature will not move again because " + Region.GetWaypointNum() + " is free");

                    // Set the region's creature reference now that the creature has arrived there
                    Region.SetCreature(this);

                    IsMoving = false;
                    if (Callback != null) Callback();        // When this creature is done advancing, let the next creature advance
                }
            }
        }
    }

    // Get the location of this creature
    private Vector3 GetLocation()
    {
        return transform.position;
    }

    public Waypoint GetRegion()
    {
        return Region;
    }

    public void SetRegion(Waypoint Region)
    {
        this.Region = Region;
    }

    // Returns whether of not this creature has reached its target location
    private bool HasReachedLocation()
    {
        // Destination coordinates
        float XTarget = Region.GetLocation().x;
        float YTarget = Region.GetLocation().y;

        // Creature coordinates
        float X = this.GetLocation().x;
        float Y = this.GetLocation().y;

        // Give the creature a margin of error to have reached the target
        float Margin = 0.00001f;
        return ( (X >= XTarget - Margin && X <= XTarget + Margin) && (Y >= YTarget - Margin && Y <= YTarget + Margin));
    }

    // Returns the type of dice used by this creature
    public DiceType GetDiceType()
    {
        switch (Type)
        {
            case CreatureType.Gor:
            case CreatureType.HerbGor:
            case CreatureType.Skral:
            case CreatureType.TowerSkral:
                return DiceType.Regular;

            case CreatureType.Wardrak:
                return DiceType.Black;

            default:
                Debug.LogError("Cannot get dice type for unknown CreatureType.");
                return DiceType.Regular;
        }
    }

    // Returns the number of dice used by this creature
    public int GetNumOfDice()
    {
        switch (Type)
        {
            case CreatureType.Gor:
            case CreatureType.HerbGor:
            case CreatureType.Skral:
            case CreatureType.TowerSkral:
                return 2;

            case CreatureType.Wardrak:
                if (Willpower <= 6) return 1;
                else return 2;

            default:
                Debug.LogError("Cannot get number of dice for unknown CreatureType.");
                return -1;
        }
    }

    public int GetWillpower()
    {
        return Willpower;
    }

    public void ResetWillpower()
    {
        Willpower = MaxWillpower;
    }

    public int GetStrength()
    {
        return Strength;
    }

    // Decreases the creature's willpower by the indicated positive amount, to a minimum of 0.
    public void DecreaseWillpower(int Amount)
    {
        if (Amount > 0) Willpower = Math.Max(Willpower - Amount, 0);
    }

    // Marks this creature as defeated and sends it to region 80
    public void Defeat()
    {
        Defeated = true;

        // If the defeated creature is a medicinal herb-carrying gor, instantiate an herb on the creature's region
        if (Type == CreatureType.HerbGor)
        {
            // TODO
            // Item MedicinalHerb = new Item();
            // MedicinalHerb.type = Type.MedicinalHerb;
            // Region.addItem(MedicinalHerb);
        }

        // Unlink the creature from its region
        if (Region.GetCreature() == this) Region.SetCreature(null);
        Region = null;

        // Decrease the number of creatures in GameManager
        CreatureManager.DecreaseNumCreatures();

        // Send the creature to region 80
        Waypoint Region80 = WaypointManager.GetWaypoint(80);
        transform.SetPositionAndRotation(Region80.GetLocation(),     // Destination
            Quaternion.identity);                                    // No rotation
        Region = Region80;

        // If the defeated creature was the tower skral, advance the narrator to 'N'
        if (Type == CreatureType.TowerSkral)
        {
            NarratorManager.advanceToN();
        }
        // Otherwise, advance the narrator by one space
        else
        {
            NarratorManager.advanceNarratorRPC(51);
        }
    }

    public bool IsDefeated()
    {
        return Defeated;
    }
}
