using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    // Board tile number
    private int WaypointNum = -1;

    // Creature on this waypoint
    private Creature Creature;

    // Heroes on this waypoint
    private List<Hero> Heroes = new List<Hero>(4);

    private List<Farmer> farmers = new List<Farmer>();

    int gold;

    // If Waypoint has well, and if it is filled
    private bool wellInitialized = false;
    private bool wellFilled = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWaypointNum(int Number)
    {
        if (WaypointNum != -1)
        {
            Debug.LogError("Error: this Waypoint's number has already been set.");
            return;
        }
        
        WaypointNum = Number;
    }

    public int GetWaypointNum()
    {
        return WaypointNum;
    }

    // Get the location of this waypoint
    public Vector3 GetLocation()
    {
        return transform.position;
    }

    public Creature GetCreature()
    {
        return Creature;
    }

    // Sets the creature as occupying this waypoint, but does NOT move the creature icon to the waypoint
    public void SetCreature(Creature Creature)
    {
        this.Creature = Creature;
    }

    // Sets the hero as occupying this waypoint, but does NOT move the hero icon to the waypoint
    public void AddHero(Hero Hero)
    {
        Heroes.Add(Hero);
    }

    // Removes the hero from occupying this waypoint, but does NOT move the hero icon away from the waypoint
    public void RemoveHero(Hero Hero)
    {
        Heroes.Remove(Hero);
    }

    public Farmer pickupOneFarmer()
    {
        if (farmers.Count > 0) {
            return new Farmer();
        } else {
            return null;
        }
    }

    public void dropOneFarmer()
    {
        farmers.Add(new Farmer());
        if (WaypointNum == 78)
        {
            // Add shield
        }
    }

    public int pickupOneGold()
    {
        if(gold > 0)
        {
            gold--;
            return 1;
        } else {
            return -1;
        }
    }

    public void dropOneGold()
    {
        gold++;
    }

    public bool Equals(Waypoint Other)
    {
        return (Other != null && this.GetWaypointNum() == Other.GetWaypointNum());
    }

    public void initializeWell()
    {
        wellInitialized = true;
        this.refillWell();
        Debug.Log("Well initialized");
    }

    public bool containsWell()
    {
        return wellInitialized;
    }

    public void refillWell()
    {
        if(wellInitialized)
        {
            wellFilled = true;
            Debug.Log("Well filled");
        }
        else
        {
            Debug.Log("Current Waypoint does not contain a well");
        }
    }

    public void emptyWell()
    {
        if(wellInitialized)
        {
            wellFilled = false;
            Debug.Log("Well has been emptied");
        }
        else
        {
            Debug.Log("Error. This waypoint does not contain a well.");
        }
    }
}
