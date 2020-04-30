using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Waypoint : MonoBehaviourPun, Subject
{
    // References to managers
    private GameManager GameManager;
    private WaypointManager WaypointManager;
    private WPButtonMoveUI WPButtonMoveUI;

    public GameObject prefab;
    public GameObject goldIcon;
    public GameObject itemPanel;
    public RegionItemsUI RegionItemsUI;

    // Board tile number
    private int WaypointNum = -1;
    private int[] WPadjList;

    // Creature on this waypoint
    private Creature Creature;

    private PhotonView PV;

    //fog on this waypoint
    private Fog Fog;

    // Heroes on this waypoint
    private List<Hero> Heroes = new List<Hero>(4);

    // List of Observers (Observer design pattern)
    List<Observer> Observers = new List<Observer>();

    // REMOVED ITEM LIST -- USING DICTIONARY INSTEAD - jonathan
    public Dictionary<ItemType, int> Items = new Dictionary<ItemType, int>();

    int numItems;
    int gold;
    int numFarmers;
    Text goldText;
    Text farmersText;

    private bool ContainsFullWell = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetFog(Fog Fog)
    {
        this.Fog = Fog;
    }

    public void InitializeItems()
    {
        Items[ItemType.Helm] = 0;
        Items[ItemType.Wineskin] = 0;
        Items[ItemType.Bow] = 0;
        Items[ItemType.Telescope] = 0;
        Items[ItemType.Falcon] = 0;
        Items[ItemType.MedicinalHerb] = 0;
        Items[ItemType.Witchbrew] = 0;
        Items[ItemType.StrengthPoints] = 0;
        Items[ItemType.Shield] = 0;
        Items[ItemType.BlueRuneStone] = 0;
        Items[ItemType.YellowRuneStone] = 0;
        Items[ItemType.GreenRuneStone] = 0;

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

        PV = GetComponent<PhotonView>();
         
        // Initialize wells
        if(Number == 5 || Number == 35 || Number == 45 || Number == 55)
        {
            ContainsFullWell = true;
        }
        //string IconName = "GoldIcon (" + Number + ")";
        //goldIcon = GameObject.Find(IconName);
        //goldIcon.transform.SetPositionAndRotation(transform.position, Quaternion.identity);

        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        WPButtonMoveUI = GameObject.Find("WPButtonMoveUI").GetComponent<WPButtonMoveUI>();
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

    public bool containsHero(Hero t)
    {
        foreach (Hero h in Heroes)
        {
            if (h == t) return true;
        }
        return false;
    }

    // Removes the hero from occupying this waypoint, but does NOT move the hero icon away from the waypoint
    public void RemoveHero(Hero Hero)
    {
        Heroes.Remove(Hero);
    }

    public int pickupOneFarmer()
    {
        if (numFarmers > 0) {
            numFarmers--;
            numItems--;
            Notify("REGION_ITEMS");
            return 1;
        } else {
            return -1;
        }
    }

    public virtual void dropOneFarmer()
    {
        //farmers.Add(new Farmer());
        numFarmers++;
        numItems++;
        Notify("REGION_ITEMS");
        // Dropping a farmer at the castle is handled by overriding this method in WaypointCastle.cs
    }

    // Destroys all farmers standing on this region. Used when a creature enters the region.
    public void DestroyFarmers()
    {
        //farmers.Clear();
        numItems -= numFarmers;
        numFarmers = 0;
        Notify("REGION_ITEMS");
    }

    // Destroys all farmers carried by heroes on this region. Used when a creature enters the region.
    public void DestroyAllFarmersCarriedByHeroes()
    {
        foreach(Hero Hero in Heroes)
        {
            Hero.DestroyCarriedFarmers();
        }
    }

    public int pickupOneGold()
    {
        if(gold > 0)
        {
            gold--;
            numItems--;
            Notify("REGION_ITEMS");
            if (gold == 0)
            {
                //goldIcon.SetActive(false);
            }
            return 1;
        } else {
            return -1;
        }
    }

    public void dropOneGold()
    {
        gold++;
        numItems++;
        //goldIcon.SetActive(true);
        goldText.text = "" + gold;
        Notify("REGION_ITEMS");
    }

    public void removeItem(ItemType ItemType)
    {
        if(Items[ItemType] > 0) Items[ItemType] -= 1;
        numItems -= 1;
        Notify("REGION_ITEMS");
    }

    public void addItem(ItemType ItemType)
    {
        Items[ItemType] += 1;
        numItems += 1;

        Notify("REGION_ITEMS");

        if (ItemType == ItemType.MedicinalHerb && WaypointNum == 0) GameManager.PlaceHerbOnCastle();
    }

    public bool containsItem(ItemType ItemType)
    {
        if(Items[ItemType] > 0) return true;
        else return false;
    }

    public bool containsFullWell()
    {
        return ContainsFullWell;
    }

    // All clients have an empty well at this waypoint.
    public void EmptyWell()
    {
        // TODO: PV.IsMine not working for all clients
        // Debug.Log("Emptied");
        if (PhotonNetwork.IsConnected) PV.RPC("UpdateWellRPC", RpcTarget.All, false);
        else UpdateWellRPC(false);
    }

    // All clients replenish the well at this waypoint.
    public void ReplenishWell()
    {
        // Debug.Log("Region " + this.GetWaypointNum() + " gets well replenished.");
        if (PhotonNetwork.IsConnected) PV.RPC("UpdateWellRPC", RpcTarget.All, true);
        else UpdateWellRPC(true);
    }

    [PunRPC]
    void UpdateWellRPC(bool WellStatus)
    {
        ContainsFullWell = WellStatus;
        Debug.Log("Well status: " + ContainsFullWell);
    }


    public bool Equals(Waypoint Other)
    {
        return (Other != null && this.GetWaypointNum() == Other.GetWaypointNum());
    }

    public void InitializeRuneStone(int ID)
    {
        if(ID == 1 || ID == 2) Items[ItemType.YellowRuneStone] += 1; 
        else if(ID == 3 || ID == 4) Items[ItemType.GreenRuneStone] += 1;
        else if(ID == 5) Items[ItemType.BlueRuneStone] += 1;
        else Debug.Log("Error instantiating Rune Stone. ID " + ID +" invalid.");

        Debug.Log("Yellow Rune Stones: "+Items[ItemType.YellowRuneStone]);
    }

    public void ShowAdjWP()
    {
        Debug.Log("in ShowAdjWP in Waypoint class");

        //int[] adjList = WaypointManager.GetWPAdjList(this.WaypointNum);

        WPButtonMoveUI.toMakeVisible(WPadjList);
    }

    public int[] GetWPAdjList()
    {
        return WPadjList;
    }

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

    public void showItems()
    {
        if (numItems > 0)
        {
            itemPanel.SetActive(true);
            RegionItemsUI.showItems(Items);
        }
    }

    public void hideItems()
    {
        itemPanel.SetActive(false);
    }

    public void setPanel()
    {
        itemPanel = GameObject.Find("RegionItemsPanel (" + WaypointNum + ")");
        RegionItemsUI = GameObject.Find("RegionItemsPanel (" + WaypointNum + ")").GetComponent<RegionItemsUI>();
        RegionItemsUI.Initialize(this);
        RegionItemsUI.SetPanel(WaypointNum);
    }

    public int getNumFarmers()
    {
        return numFarmers;
    }
    public int getNumGold()
    {
        return gold;
    }
}
