using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    // Reference to WaypointManager
    private WaypointManager WaypointManager;

    // Creature type (Gor, Skral, Wardrak, etc.)
    private CreatureType Type;

    // Flag that tracks whether the creature is currently moving
    private bool IsMoving = false;

    // Creature move speed
    private float MoveSpeed = 3f;

    // Current location of the creature, or, its destination if it is currently moving
    private Waypoint Region;

    // Callback to be called after moving
    private Action Callback;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize reference to WaypointManager
        WaypointManager = GameObject.Find("WaypointManager").GetComponent<WaypointManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // If the creature is supposed to be moving, move it closer to its target location
        if (IsMoving) Move();
    }
    
    public CreatureType GetCreatureType()
    {
        return Type;
    }

    public void SetType(CreatureType Type)
    {
        this.Type = Type;
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
        Region.SetCreature(null);             // Important: putting this in ContinueAdvancing causes errors (makes some creatures partially vanish from recognition)

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

            // Check whether this region was already occupied (if it isn't the castle). If so, keep moving.
            Creature OtherRegionCreature = Region.GetCreature();

            if (Region.GetWaypointNum() != 0 && OtherRegionCreature != null) // Don't check this condition for the castle
            {
                // Debug.Log("This creature will move again because there is already a creature on " + Region.GetWaypointNum());
                IsMoving = false;   // To allow the creature to move again
                ContinueAdvancing();
            }
            else
            {
                // Debug.Log("This creature will not move again because " + Region.GetWaypointNum() + " is free");
                
                
                // TODO handle creatures entering the castle when done moving
                // TODO handle creatures killing farmers when done moving


                // Set the region's creature reference now that the creature has arrived there
                Region.SetCreature(this);

                IsMoving = false;
                if (Callback != null) Callback();        // When this creature is done advancing, let the next creature advance.
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
}
