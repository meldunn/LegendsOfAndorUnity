using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Dynamically loaded UI elements
    HeroMenu HeroMenu;
    HeroControlMenu HeroControlMenu;
    StartBattleMenu StartBattleMenu;
    WellUIManager WellUIManager;
    InputManager InputManager;
    RuneStoneMenu RuneStoneMenu;
    TradeMenuUI TradeMenuUI;
    BattleInvitationMenu BattleInvitationMenu;
    StatsUIManager StatsUIManager;
    MerchantUIManager MerchantUIManager;
    BattleMenu BattleMenu;
    WPButtonMoveUI WPButtonMoveUI;
    FogManager FogManager;
    CastleMenu CastleMenu;
    HeroCardUI WarriorHeroCard;
    HeroCardUI DwarfHeroCard;
    HeroCardUI ArcherHeroCard;
    HeroCardUI WizardHeroCard;
    TimeTrackUI TimeTrackUI;
    ChatUI ChatUI;
    DivideBattleResources DivideBattleResources;
    WaypointManager WaypointManager;
    GameManager GameManager;

    // Directly linked UI elements
    [SerializeField]
    GameObject HeroControlMenuObject = null;
    [SerializeField]
    GameObject StartBattleMenuObject = null;
    [SerializeField]
    GameObject BattleInvitationMenuObject = null;
    [SerializeField]
    GameObject BattleMenuObject = null;
    [SerializeField]
    GameObject CastleMenuObject = null;
    [SerializeField]
    GameObject TimeTrackUIObject = null;
    [SerializeField]
    GameObject ChatUIObject = null;

    [SerializeField]
    GameObject WarriorCardObject = null;
    [SerializeField]
    GameObject DwarfCardObject = null;
    [SerializeField]
    GameObject ArcherCardObject = null;
    [SerializeField]
    GameObject WizardCardObject = null;
    [SerializeField]
    GameObject helpMenu;
    [SerializeField]
    GameObject fakeMerchant;
    [SerializeField]
    GameObject fakeEndMove;


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

    public void Initialize(GameManager GM)
    {
        // Initialize references to UI element
        HeroMenu = GameObject.Find("HeroMenu").GetComponent<HeroMenu>();
        HeroControlMenu = HeroControlMenuObject.GetComponent<HeroControlMenu>();
        StartBattleMenu = StartBattleMenuObject.GetComponent<StartBattleMenu>();
        WellUIManager = GameObject.Find("WellUIManager").GetComponent<WellUIManager>();
        RuneStoneMenu = GameObject.Find("RuneStoneMenu").GetComponent<RuneStoneMenu>();
        TradeMenuUI = GameObject.Find("TradeMenu").GetComponent<TradeMenuUI>();
        BattleInvitationMenu = BattleInvitationMenuObject.GetComponent<BattleInvitationMenu>();
        StatsUIManager = GameObject.Find("StatsUIManager").GetComponent<StatsUIManager>();
        MerchantUIManager = GameObject.Find("MerchantUIManager").GetComponent<MerchantUIManager>();
        BattleMenu = BattleMenuObject.GetComponent<BattleMenu>();
        WPButtonMoveUI = GameObject.Find("WPButtonMoveUI").GetComponent<WPButtonMoveUI>();
        WaypointManager = GameObject.Find("WaypointManager").GetComponent<WaypointManager>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // DivideBattleResources = GameObject.Find("DivideBattleResourcesPanel").GetComponent<DivideBattleResources>();
        FogManager = GameObject.Find("FogManager").GetComponent<FogManager>();
        CastleMenu = CastleMenuObject.GetComponent<CastleMenu>();
        TimeTrackUI = TimeTrackUIObject.GetComponent<TimeTrackUI>();
        ChatUI = ChatUIObject.GetComponent<ChatUI>();
        //helpMenu = GameObject.Find("HelpOverlay");
        helpMenu.SetActive(false);

        // Initializing the HeroCards
        WarriorHeroCard = WarriorCardObject.GetComponent<HeroCardUI>();
        DwarfHeroCard = DwarfCardObject.GetComponent<HeroCardUI>();
        ArcherHeroCard = ArcherCardObject.GetComponent<HeroCardUI>();
        WizardHeroCard = WizardCardObject.GetComponent<HeroCardUI>();
        
        // Must come AFTER all Game objects are found.
        InputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        // Initialize all UI elements
        HeroMenu.Initialize();
        HeroControlMenu.Initialize();
        StartBattleMenu.Initialize();
        WellUIManager.Initialize();
        RuneStoneMenu.Initialize();
        BattleInvitationMenu.Initialize();
        TradeMenuUI.Initialize();
        // DivideBattleResources.Initialize();

        // Initializing the HeroCards
        WarriorHeroCard.Initialize();
        DwarfHeroCard.Initialize();
        ArcherHeroCard.Initialize();
        WizardHeroCard.Initialize();
       
        // StatsUIManager.Initialize();
        MerchantUIManager.Initialize();
        BattleMenu.Initialize();
        WPButtonMoveUI.Initialize(GM);
        FogManager.Initialize();
        CastleMenu.Initialize();
        TimeTrackUI.Initialize();
        ChatUI.Initialize();

        // Must come AFTER all initializations
        InputManager.Initialize();
    }
    
    public void onHeroMove()
    {
        // Makes all the nevessary UI changes AFTER a Hero has moved to the new Waypoint
        Waypoint HeroRegion = GameManager.GetSelfHero().GetCurrentRegion();

        Debug.Log("HeroRegion: "+HeroRegion.containsFullWell());

        // Update UI position-based buttons.
        if (HeroRegion.containsFullWell())
        {
            Debug.Log("Request to show well");
            WellUIManager.DisplayWellButton(HeroRegion.GetWaypointNum());
        }

        MerchantUIManager.UpdateMerchantButton(HeroRegion.GetWaypointNum());
    }

    public void ToggleCheatMenu(GameObject CheatMenu)
    {
        if (CheatMenu.gameObject.activeSelf == true) CheatMenu.SetActive(false);
        else if (CheatMenu.gameObject.activeSelf == false) CheatMenu.SetActive(true);
        
    }
    public void ToggleHelpMenu()
    {
        if (helpMenu.gameObject.activeSelf == true)
        {
            helpMenu.SetActive(false);
            fakeMerchant.SetActive(false);
            fakeEndMove.SetActive(false);
        }
        else if (helpMenu.gameObject.activeSelf == false)
        {
            helpMenu.SetActive(true);
            fakeMerchant.SetActive(true);
            fakeEndMove.SetActive(true);
        }
    }

    public StartBattleMenu GetStartBattleMenu()
    {
        return this.StartBattleMenu;
    }

    // Called once at the beginning of the game
    public void PlaceRuneStoneCard()
    {
        NarratorManager NarratorManager = GameObject.Find("NarratorManager").GetComponent<NarratorManager>(); 
        TMPro.TextMeshProUGUI RuneRoll = GameObject.Find("RuneCardRoll").GetComponent<TMPro.TextMeshProUGUI>();
        int Roll;
        NarratorLetter NarratorSpace = NarratorLetter.H;
        System.Random rand = new System.Random();
        Roll = rand.Next(6);
        Roll += 1; 

        if(Roll == 6) NarratorSpace = NarratorLetter.H;
        else if(Roll == 5 || Roll == 4) NarratorSpace = NarratorLetter.F;
        else if(Roll == 3) NarratorSpace = NarratorLetter.E;
        else if(Roll == 2) NarratorSpace = NarratorLetter.D;
        else if(Roll == 1) NarratorSpace = NarratorLetter.B;

        RuneRoll.text = "You rolled a "+Roll+
            "! The Rune Stone card will be activated when the Narrator reaches square " + NarratorSpace;

        NarratorManager.AddRuneStoneCard(NarratorSpace);

        Invoke("HideRuneStonePopup", 5);

    }

    public void HideRuneStonePopup()
    {
        GameObject RuneStonePopup = GameObject.Find("PlaceRuneStonePopup"); 

        Vector3 far = new Vector3(200, 0, 0);
        RuneStonePopup.transform.Translate(far - RuneStonePopup.transform.position);
    }

    public void EndGame(bool GameWon)
    {
        
        TMPro.TextMeshProUGUI Header = GameObject.Find("EndGameHeader").GetComponent<TMPro.TextMeshProUGUI>();
        TMPro.TextMeshProUGUI Message = GameObject.Find("EndGameMessage").GetComponent<TMPro.TextMeshProUGUI>();
        if(GameWon)
        {
            Header.text = "Game has been won!";
            Message.text = "Thanks to you brave heroes, peace in the kingdom is now restored. Stay tuned for Legend 3!";
        }
        else
        {
            Header.text = "Game has been lost!";
            Message.text = "The Monsters prevailed. You allowed too many monsters to enter the castle.";
        }

        Vector3 Origin = new Vector3(0,0,0);
        GameObject Panel = GameObject.Find("EndGamePopup");

        Panel.transform.Translate(Origin - Panel.transform.position);
    }
}
