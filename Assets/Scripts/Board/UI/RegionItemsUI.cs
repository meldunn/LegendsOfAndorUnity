using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionItemsUI : MonoBehaviour, Observer
{

    HeroManager HeroManager;
    GameManager GameManager;
    Waypoint Waypoint;

    [SerializeField] GameObject Bow;
    [SerializeField] GameObject Brew;
    [SerializeField] GameObject BlueRune;
    [SerializeField] GameObject Falcon;
    [SerializeField] GameObject GreenRune;
    [SerializeField] GameObject Helm;
    [SerializeField] GameObject Shield;
    [SerializeField] GameObject Telescope;
    [SerializeField] GameObject Wineskin;
    [SerializeField] GameObject YellowRune;
    [SerializeField] GameObject Medicinalherb;

    int numItems;
    int gold;
    int numFarmers;
    Text goldText;
    Text farmersText;

    public GameObject goldIcon;
    public GameObject itemPanel;

    public void Initialize(Waypoint w)
    {
        HeroManager = GameObject.Find("HeroManager").GetComponent<HeroManager>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Waypoint = w;

        Bow.SetActive(false);
        Brew.SetActive(false);
        BlueRune.SetActive(false);
        Falcon.SetActive(false);
        GreenRune.SetActive(false);
        Helm.SetActive(false);
        Shield.SetActive(false);
        Telescope.SetActive(false);
        Wineskin.SetActive(false);
        YellowRune.SetActive(false);
        Medicinalherb.SetActive(false);
    }


    public void showItems(Dictionary<ItemType, int> Items)
    {
        if (Items[ItemType.Bow] > 0) Bow.SetActive(true);
        if (Items[ItemType.Witchbrew] > 0) Brew.SetActive(true);
        if (Items[ItemType.BlueRuneStone] > 0) BlueRune.SetActive(true);
        if (Items[ItemType.Falcon] > 0) Falcon.SetActive(true);
        if (Items[ItemType.GreenRuneStone] > 0) GreenRune.SetActive(true);
        if (Items[ItemType.Helm] > 0) Helm.SetActive(true);
        if (Items[ItemType.Shield] > 0) Shield.SetActive(true);
        if (Items[ItemType.Telescope] > 0) Telescope.SetActive(true);
        if (Items[ItemType.Wineskin] > 0) Wineskin.SetActive(true);
        if (Items[ItemType.YellowRuneStone] > 0) YellowRune.SetActive(true);
        if (Items[ItemType.MedicinalHerb] > 0) Medicinalherb.SetActive(true);

    }

    public void pickupBow()
    {
        if (Waypoint.containsHero(GameManager.GetSelfHero()))
        {
            GameManager.GetSelfHero().heroInventory.addItem(ItemType.Bow);
            Bow.SetActive(false);
        }
    }

    public void pickupWineskin()
    {
        if (Waypoint.containsHero(GameManager.GetSelfHero()))
        {
            //doesnt work...
            GameManager.GetSelfHero().pickupItem(ItemType.Wineskin);
            Wineskin.SetActive(false);

        }
    }

    public void pickupWitchbrew()
    {
        if (Waypoint.containsHero(GameManager.GetSelfHero()))
        {
            GameManager.GetSelfHero().heroInventory.addItem(ItemType.Witchbrew);
            Brew.SetActive(false);
        }
    }

    public void pickupBlueRuneStone()
    {
        if (Waypoint.containsHero(GameManager.GetSelfHero()))
        {
            GameManager.GetSelfHero().heroInventory.addItem(ItemType.BlueRuneStone);
            BlueRune.SetActive(false);
        }
    }

    public void pickupFalcon()
    {
        if (Waypoint.containsHero(GameManager.GetSelfHero()))
        {
            GameManager.GetSelfHero().heroInventory.addItem(ItemType.Falcon);
            Falcon.SetActive(false);
        }
    }

    public void pickupGreenRuneStone()
    {
        if (Waypoint.containsHero(GameManager.GetSelfHero()))
        {
            GameManager.GetSelfHero().heroInventory.addItem(ItemType.GreenRuneStone);
            GreenRune.SetActive(false);
        }
    }

    public void pickupHelm()
    {
        if (Waypoint.containsHero(GameManager.GetSelfHero()))
        {
            GameManager.GetSelfHero().heroInventory.addItem(ItemType.Helm);
            Helm.SetActive(false);
        }
    }

    public void pickupShield()
    {
        if (Waypoint.containsHero(GameManager.GetSelfHero()))
        {
            GameManager.GetSelfHero().heroInventory.addItem(ItemType.Shield);
            Shield.SetActive(false);
        }
    }

    public void pickupTelescope()
    {
        if (Waypoint.containsHero(GameManager.GetSelfHero()))
        {
            GameManager.GetSelfHero().heroInventory.addItem(ItemType.Telescope);
            Telescope.SetActive(false);
        }
    }

    public void pickupYellowRuneStone()
    {
        if (Waypoint.containsHero(GameManager.GetSelfHero()))
        {
            GameManager.GetSelfHero().heroInventory.addItem(ItemType.YellowRuneStone);
            YellowRune.SetActive(false);
        }
    }

    public void pickupMedicinalHerb()
    {
        if (Waypoint.containsHero(GameManager.GetSelfHero()))
        {
            GameManager.GetSelfHero().heroInventory.addItem(ItemType.MedicinalHerb);
            Medicinalherb.SetActive(false);
        }
    }

    // Should be moved to a UI class
    public void SetPanel(int wpNumber)
    {
        string name = "RegionItemsPanel (" + wpNumber + ")";
        Waypoint = GameObject.Find("Waypoint (" + wpNumber + ")").GetComponent<Waypoint>();
        itemPanel = GameObject.Find(name);
        //RegionItemsUI = GameObject.Find(name).GetComponent<RegionItemsUI>();
        itemPanel.transform.SetPositionAndRotation(Waypoint.GetLocation(), Quaternion.identity);
        farmersText = itemPanel.transform.Find("NumFarmersText").GetComponent<Text>();
        farmersText.text = " Farmers: " + numFarmers;
        goldText = itemPanel.transform.Find("NumGoldText").GetComponent<Text>();
        goldText.text = " Gold: " + gold;
        itemPanel.SetActive(false);
        Waypoint.Attach(this);
        //RegionItemsUI.Initialize(this);
    }


    public void UpdateData(string Category)
    {
        if (string.Equals(Category, "REGION_ITEMS"))
        {
            updateRegionItems();
        }
    }

    public void updateRegionItems()
    {
        farmersText.text = " Farmers: " + Waypoint.getNumFarmers();
        goldText.text = " Gold: " + Waypoint.getNumGold();
        Debug.LogError("NOTIFY");
        showItems(Waypoint.Items);
    }
}
