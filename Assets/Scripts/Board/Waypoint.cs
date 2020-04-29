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
    public GameObject itemPanel;

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

    private List<Farmer> farmers = new List<Farmer>();

    // REMOVED ITEM LIST -- USING DICTIONARY INSTEAD - jonathan
    private Dictionary<ItemType, int> Items = new Dictionary<ItemType, int>();

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
        // Dropping a farmer at the castle is handled by overriding this method in WaypointCastle.cs
    }

    // Destroys all farmers standing on this region. Used when a creature enters the region.
    public void DestroyFarmers()
    {
        //farmers.Clear();
        numFarmers = 0;
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
    }

    public void removeItem(Item item)
    {
        ItemType Type = item.GetItemType();
        if(Items[Type] > 0) Items[Type] -= 1;
    }

    public void addItem(Item item)
    {
        ItemType Type = item.GetItemType();
        Items[Type] += 1;
    }

    public bool containsItem(Item item)
    {
        ItemType Type = item.GetItemType();

        if(Items[Type] > 0) return true;
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

    // Should be moved to a UI class
    public void SetIcon()
    {
        //string name = "GoldIcon (" + WaypointNum + ")";
        //goldIcon = GameObject.Find(name);
        //goldIcon.transform.SetPositionAndRotation(this.GetLocation(), Quaternion.identity);
        //goldText = goldIcon.transform.Find("Text").GetComponent<Text>();
        //goldText.text = "" + gold;
        //goldIcon.SetActive(false);

        string name = "RegionItemsPanel (" + WaypointNum + ")";
        itemPanel = GameObject.Find(name);
        itemPanel.transform.SetPositionAndRotation(this.GetLocation(), Quaternion.identity);
        farmersText = itemPanel.transform.Find("NumFarmersText").GetComponent<Text>();
        farmersText.text = " Farmers: " + numFarmers;
        goldText = itemPanel.transform.Find("NumGoldText").GetComponent<Text>();
        goldText.text = " Gold: " + gold;
        itemPanel.SetActive(false);
    }

    public void showItems()
    {
        if (numItems > 0)
        {
            itemPanel.SetActive(true);
            farmersText.text = " Farmers: " + numFarmers;
            goldText.text = " Gold: " + gold;
        }
    }

    public void hideItems()
    {
        itemPanel.SetActive(false);
    }
}
