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
<<<<<<< Updated upstream
    
=======
    GoldIcon goldIcon;
    GameObject clone;
    Text goldText;

>>>>>>> Stashed changes
    private bool ContainsWell = false;
    Well well;

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< Updated upstream
        
=======
        WPButtonMoveUI = GameObject.Find("WPButtonMoveUI").GetComponent<WPButtonMoveUI>();
        //goldIcon = GameObject.Find("GoldIcon").GetComponent<GoldIcon>();
        //clone = Instantiate(GameObject.Find("GoldIcon"), this.GetLocation(), Quaternion.identity);
        //Debug.Log("GoldIcon Location: " + goldIcon.transform.position);
        //Debug.Log("Waypoint Location: " + this.GetLocation());
        //goldText = goldIcon.GetComponent<UnityEngine.UI.Text>();
        //clone.transform.SetPositionAndRotation(this.GetLocation(), Quaternion.identity);
>>>>>>> Stashed changes
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< Updated upstream
        
=======
        if (gold >= 0)
        {
            //goldText.text = "" + gold;
            
        }
    }

    public void SetWPAdjList(int[] list)
    {
        WPadjList = new int[list.Length];

        //copy the elements
        for (int i = 0; i < list.Length; i++)
        {
            WPadjList[i] = list[i];
        }

>>>>>>> Stashed changes
    }

    public void SetWaypointNum(int Number)
    {
        if (WaypointNum != -1)
        {
            Debug.LogError("Error: this Waypoint's number has already been set.");
            return;
        }
        
        WaypointNum = Number;

        if(Number == 5 || Number == 35 || Number == 45 || Number == 55)
        {
            well = new Well();
            ContainsWell = true;
        }

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

    public bool containsFullWell()
    {
        if(ContainsWell && well.IsFull())
        {
            return true;
        }

        return false;
    }
    public void EmptyWell()
    {
        well.EmptyWell();
        Debug.Log("Well has been empited and is now " + well.IsFull());
    }
    public void ReplenishWell()
    {
        Debug.Log("Region " + this.GetWaypointNum() + " gets well replenished.");
        well.ReplenishWell();
    }

    public bool Equals(Waypoint Other)
    {
        return (Other != null && this.GetWaypointNum() == Other.GetWaypointNum());
    }

<<<<<<< Updated upstream
=======
    public void InitializeRuneStone(int ID)
    {
        RuneStones.Add(new RuneStone(ID));
    }

    public void ShowAdjWP()
    {
        Debug.Log("in ShowAdjWP in Waypoint class");

        //int[] adjList = WaypointManager.GetWPAdjList(this.WaypointNum);

        WPButtonMoveUI.toMakeVisible(WPadjList);


    }

    public void SetGoldIcon(GoldIcon g)
    {
        goldIcon = g;
    }


>>>>>>> Stashed changes
}
