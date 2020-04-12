using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CastleMenu : MonoBehaviour, Observer
{
    // References to managers
    private GameManager GameManager;
    private WaypointManager WaypointManager;

    // The castle
    WaypointCastle Castle;

    // Tracks whether the popout menu is shown
    bool MenuIsShown = false;

    // References to children components
    // Popout menu
    [SerializeField]
    GameObject CastlePanel = null;

    // Golden shields
    [SerializeField]
    GameObject[] GoldenShields = null;

    // Farmer shields
    [SerializeField]
    GameObject[] FarmerShields = null;

    // Golden shield creature slots
    [SerializeField]
    GameObject[] GoldenShieldCreatureSlots = null;

    // Farmer shield creature slots
    [SerializeField]
    GameObject[] FarmerShieldCreatureSlots = null;

    // Creature sprites
    [SerializeField]
    Sprite GorSprite = null;
    [SerializeField]
    Sprite SkralSprite = null;
    [SerializeField]
    Sprite WardrakSprite = null;

    // Start is called before the first frame update
    void Start()
    {
        // Not used to ensure UI elements are initialized in the right order.
        // UIManager calls Initialize instead.   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        // Initialize references to managers
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        WaypointManager = GameObject.Find("WaypointManager").GetComponent<WaypointManager>();

        // Initialize the castle
        Castle = WaypointManager.GetCastle();

        // Register as an observer of GameManager
        GameManager.Attach(this);

        // Register as an observer of the Castle
        Castle.Attach(this);
    }

    // To function correctly, this menu must be displayed using this method
    public void Show()
    {
        // If the castle hasn't been set, nothing happens
        if (Castle == null)
        {
            Debug.LogError("The castle menu can't be shown because no castle was initialized.");
            return;
        }

        MenuIsShown = true;

        CastlePanel.SetActive(true);

        InitializeUI();
    }

    // To function correctly, this menu must be hidden using this method
    public void Hide()
    {
        MenuIsShown = false;

        CastlePanel.SetActive(false);
    }

    public void ToggleVisibility()
    {
        if (MenuIsShown) Hide();
        else Show();
    }

    // Used in Observer design pattern
    public void UpdateData(string Category)
    {
        if (string.Equals(Category, "CASTLE_CREATURE"))
        {
            UpdateShields();
        }
        else if (string.Equals(Category, "CASTLE_FARMER"))
        {
            UpdateShields();
        }
        else if (string.Equals(Category, "CONTROL"))
        {
            // When changing the controlled hero, hide this menu.
            this.Hide();
        }
    }

    private void InitializeUI()
    {
        // Initialize UI
        UpdateShields();
    }

    private void UpdateShields()
    {
        int NumGoldenShields = Castle.GetNumBasicShields();
        int NumFarmerShields = Castle.GetNumFarmerShields();
        List<Creature> Creatures = Castle.GetCreatures();
        GameObject[] Shields, ShieldCreatureSlots;
        int NumShields;
        Sprite CreatureSprite;
        int i, j, t;
        int k = 0;

        // Iterate through this update twice; once for the golden shields and once for the farmer shields
        for (t = 0; t < 2; t++)
        {
            // Set the correct variables to use
            if (t == 0)
            {
                Shields = GoldenShields;
                NumShields = NumGoldenShields;
                ShieldCreatureSlots = GoldenShieldCreatureSlots;
            }
            else
            {
                Shields = FarmerShields;
                NumShields = NumFarmerShields;
                ShieldCreatureSlots = FarmerShieldCreatureSlots;
            }
            
            // Show the correct shields
            for (i = 0; i < Shields.Length; i++)
            {
                if (i < NumShields) Shields[i].SetActive(true);
                else Shields[i].SetActive(false);
            }

            // Show the creatures on each shield
            for (j = 0; j < ShieldCreatureSlots.Length; j++)
            {
                GameObject Slot = ShieldCreatureSlots[j];

                if (j < NumShields && k < Creatures.Count)
                {
                    Creature Creature = Creatures[k];

                    // Select the right creature icon
                    CreatureType Type = Creature.GetCreatureType();

                    if (Type == CreatureType.Gor) CreatureSprite = GorSprite;
                    else if (Type == CreatureType.Skral) CreatureSprite = SkralSprite;
                    else if (Type == CreatureType.Wardrak) CreatureSprite = WardrakSprite;
                    else CreatureSprite = null;

                    // Set the icon and show it
                    Slot.GetComponent<Image>().sprite = CreatureSprite;
                    Slot.SetActive(true);

                    k += 1;     // Next creature to place
                }
                else
                {
                    Slot.SetActive(false);
                }
            }
        }
    }
}
