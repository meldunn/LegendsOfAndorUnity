using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CreatureType { Gor, Skral, Wardrak };  // No support for trolls in Legend 2

public class CreatureManager : MonoBehaviour
{
    // Reference to WaypointManager
    private WaypointManager WaypointManager;

    // Creature prefabs
    [SerializeField]
    private GameObject GorPrefab = null;

    [SerializeField]
    private GameObject SkralPrefab = null;

    [SerializeField]
    private GameObject WardrakPrefab = null;

    // Variables used to advance the creatures
    private int NumOfCreatures = 0;
    private bool IsAdvancing = false;
    private List<Creature> AdvancingList;
    private int CurrentAdvancingIndex;
    private CreatureType[] Waves = { CreatureType.Gor, CreatureType.Skral, CreatureType.Wardrak, CreatureType.Wardrak };

    // Current battle in progress
    Battle CurrentBattle;

    // Start is called before the first frame update
    void Start()
    {
        // Not used to ensure Managers are started in the right order.
        // GameManager calls Initialize instead.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        // Initialize reference to WaypointManager
        WaypointManager = GameObject.Find("WaypointManager").GetComponent<WaypointManager>();
    }

    // Spawns a new creature on the target region
    public void Spawn(CreatureType Type, int RegionNum)
    {
        // Validate input
        if (!WaypointManager.IsValidWaypoint(RegionNum))
        {
            Debug.LogError("Cannot spawn creature on region " + RegionNum + "; invalid region.");
            return;
        }

        // Get the target region
        Waypoint TargetRegion = WaypointManager.GetWaypoint(RegionNum);

        // Check that there isn't already a creature on the target region
        if (TargetRegion.GetCreature() != null)
        {
            Debug.LogError("Cannot spawn creature on region " + RegionNum + "; there is already a creature there.");
            return;
        }

        // Select the correct creature prefab based on the input type
        GameObject CreaturePrefab;
        switch (Type)
        {
            case CreatureType.Gor:
                CreaturePrefab = GorPrefab;
                break;
            case CreatureType.Skral:
                CreaturePrefab = SkralPrefab;
                break;
            case CreatureType.Wardrak:
                CreaturePrefab = WardrakPrefab;
                break;
            default:
                Debug.LogError("Cannot spawn creature on region " + RegionNum + "; invalid creature type.");
                return;
        }

        // Spawn the creature on the target region
        GameObject CreatureOnBoard = Instantiate(CreaturePrefab, TargetRegion.GetLocation(), Quaternion.identity);   // (Creature, location, no rotation)

        // Set the region's creature reference to the new creature
        Creature Creature = CreatureOnBoard.GetComponent<Creature>();
        TargetRegion.SetCreature(Creature);

        // Set the creature's location as the region
        Creature.SetRegion(TargetRegion);

        // Set the creature's type
        Creature.SetType(Type);

        // Increment the total number of creatures in the game
        NumOfCreatures++;
    }

    // Starts the process of advancing the creatures one at a time according to the game rules.
    // After starting this process, advancing proceeds using callbacks, because moving is handled asynchronously (frame-by-frame).
    public void StartAdvancing()
    {
        // Make sure advancing is not already in progress
        if (IsAdvancing)
        {
            Debug.LogError("Cannot advance creatures; advancing is already in progress.");
            return;
        }

        // Debug.Log("Starting advancing creatures.");

        IsAdvancing = true;

        // Set up the advancing order
        // Start by resetting the advancing list
        AdvancingList = new List<Creature>(NumOfCreatures);  // Initial size is NumOfCreatures, though more slots may be needed (since wardraks move twice)
        CurrentAdvancingIndex = -1;                          // Start the index at -1 so that the first call to AdvanceNext() advances the first creature (index 0)

        // Iterate through the waves
        foreach (CreatureType Wave in Waves)
        {
            // Iterate through all regions except the castle (0)
            for (int i = 1; i <= 84; i++)
            {
                if (WaypointManager.IsValidWaypoint(i)) // Skip regions that don't exist
                {
                    Waypoint Region = WaypointManager.GetWaypoint(i);
                    Creature Creature = Region.GetCreature();

                    // Check whether there is a creature on this region
                    if (Creature != null)
                    {
                        // Get the creature's type
                        CreatureType Type = Creature.GetCreatureType();

                        // Proceed with advancing this creature only if it belongs to the current wave
                        if (Type == Wave)
                        {
                            // Register this creature as the next to advance
                            AdvancingList.Add(Creature);
                        }
                    }
                }
            }
        }

        // Once the advancing list has been built, launch advancing
        AdvanceNext();
    }

    public void AdvanceNext()
    {
        // Increment the index of the creature to advance
        CurrentAdvancingIndex++;

        // Debug.Log("Advancing creature " + CurrentAdvancingIndex);

        // Check whether the end has been reached
        if (CurrentAdvancingIndex >= AdvancingList.Count)
        {
            EndAdvancing();
        }
        // Otherwise, advance the creature
        else
        {
            AdvancingList[CurrentAdvancingIndex].StartAdvancing(AdvanceNext); // Pass the current function as a callback (to advance the next creature when this one is done)
        }
    }

    public void EndAdvancing()
    {
        IsAdvancing = false;
        // Debug.Log("Advancing done.");
    }

    public void SetCurrentBattle(Battle Battle)
    {
        this.CurrentBattle = Battle;
    }

    public Battle GetCurrentBattle()
    {
        return CurrentBattle;
    }
}
