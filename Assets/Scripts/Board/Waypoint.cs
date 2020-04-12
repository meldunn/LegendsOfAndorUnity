using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Waypoint : MonoBehaviourPun
{
    // Reference to WaypointManager
    private WaypointManager WaypointManager;
    private WPButtonMoveUI WPButtonMoveUI;

    public GameObject prefab;
    public GameObject goldIcon;

    // Board tile number
    private int WaypointNum = -1;
    private int[] WPadjList;

    // Creature on this waypoint
    private Creature Creature;

    //fog on this waypoint
    private Fog Fog;

    // Heroes on this waypoint
    private List<Hero> Heroes = new List<Hero>(4);

    private List<RuneStone> RuneStones = new List<RuneStone>(3);

    private List<Farmer> farmers = new List<Farmer>();

    private List<Item> items = new List<Item>();

    int gold;
    // Commented out due to an error that caused the game not to run. MonoBehaviors can't be instantiated using the new keyword.
    // GoldIcon goldIcon = new GoldIcon();
    Text goldText;

    private bool ContainsWell = false;
    Well well;

    // Start is called before the first frame update
    void Start()
    { 
        WPButtonMoveUI = GameObject.Find("WPButtonMoveUI").GetComponent<WPButtonMoveUI>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetFog(Fog Fog)
    {
        this.Fog = Fog;
    }

    public void SetWPAdjList(int[] list)
    {
        WPadjList = new int[list.Length];

        //copy the elements
        for (int i = 0; i < list.Length; i++)
        {
            WPadjList[i] = list[i];
        }

    }

    public void SetWaypointNum(int Number)
    {
        if (WaypointNum != -1)
        {
            Debug.LogError("Error: this Waypoint's number has already been set.");
            return;
        }
        
        WaypointNum = Number;
         
        // Initialize wells
        if(Number == 5 || Number == 35 || Number == 45 || Number == 55)
        {
            ContainsWell = true;
        }
        //string IconName = "GoldIcon (" + Number + ")";
        //goldIcon = GameObject.Find(IconName);
        //goldIcon.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
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

    public virtual void dropOneFarmer()
    {
        farmers.Add(new Farmer());
        
        // Dropping a farmer at the castle is handled by overriding this method in WaypointCastle.cs
    }

    public int pickupOneGold()
    {
        if(gold > 0)
        {
            gold--;
            if (gold == 0)
            {
                goldIcon.SetActive(false);
            }
            return 1;
        } else {
            return -1;
        }
    }

    public void dropOneGold()
    {
        gold++;
        goldIcon.SetActive(true);
        goldText.text = "" + gold;
    }

    public void removeItem(Item item)
    {
        items.Remove(item);
    }

    public void addItem(Item item)
    {
        items.Add(item);
    }

    public bool containsItem(Item item)
    {
        return items.Contains(item);
    }

    public bool containsFullWell()
    {
        if(ContainsWell && well != null)
        {
            return true;
        }

        return false;
    }

    // All clients have an empty well at this waypoint.
    public void EmptyWell()
    {
        well.EmptyWell();
        Debug.Log("Well has been empited and is now " + well.IsFull());
    }

    // All clients replenish the well at this waypoint.
    public void ReplenishWell()
    {
        Debug.Log("Region " + this.GetWaypointNum() + " gets well replenished.");
        well.ReplenishWell();
    }

    public bool Equals(Waypoint Other)
    {
        return (Other != null && this.GetWaypointNum() == Other.GetWaypointNum());
    }

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

    // Should be moved to a UI class
    public void SetIcon()
    {
        string name = "GoldIcon (" + WaypointNum + ")";
        goldIcon = GameObject.Find(name);
        goldIcon.transform.SetPositionAndRotation(this.GetLocation(), Quaternion.identity);
        goldText = goldIcon.transform.Find("Text").GetComponent<Text>();
        goldText.text = "" + gold;
        goldIcon.SetActive(false);
    }

}
