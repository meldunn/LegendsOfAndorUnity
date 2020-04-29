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
        FogManager = GameObject.Find("FogManager").GetComponent<FogManager>();
        CastleMenu = CastleMenuObject.GetComponent<CastleMenu>();
        TimeTrackUI = TimeTrackUIObject.GetComponent<TimeTrackUI>();
        ChatUI = ChatUIObject.GetComponent<ChatUI>();

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
    
    public void onHeroMove(Hero Hero)
    {
        // Makes all the nevessary UI changes AFTER a Hero has moved to the new Waypoint
        Waypoint HeroRegion = Hero.GetWaypoint();

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

    public StartBattleMenu GetStartBattleMenu()
    {
        return this.StartBattleMenu;
    }
}
