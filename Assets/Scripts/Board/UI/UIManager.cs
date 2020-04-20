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
    BattleInvitationMenu BattleInvitationMenu;
    StatsUIManager StatsUIManager;
    MerchantUIManager MerchantUIManager;
    BattleMenu BattleMenu;
    WPButtonMoveUI WPButtonMoveUI;
    FogManager FogManager;
    CastleMenu CastleMenu;

    // Directly linked UI elements
    [SerializeField]
    GameObject StartBattleMenuObject = null;
    [SerializeField]
    GameObject BattleInvitationMenuObject = null;
    [SerializeField]
    GameObject BattleMenuObject = null;
    [SerializeField]
    GameObject CastleMenuObject = null;

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

    public void Initialize()
    {
        // Initialize references to UI element
        HeroMenu = GameObject.Find("HeroMenu").GetComponent<HeroMenu>();
        HeroControlMenu = GameObject.Find("HeroControlMenu").GetComponent<HeroControlMenu>();
        StartBattleMenu = StartBattleMenuObject.GetComponent<StartBattleMenu>();
        WellUIManager = GameObject.Find("WellUIManager").GetComponent<WellUIManager>();
        RuneStoneMenu = GameObject.Find("RuneStoneMenu").GetComponent<RuneStoneMenu>();
        BattleInvitationMenu = BattleInvitationMenuObject.GetComponent<BattleInvitationMenu>();
        // StatsUIManager = GameObject.Find("StatsUIManager").GetComponent<StatsUIManager>();
        MerchantUIManager = GameObject.Find("MerchantUIManager").GetComponent<MerchantUIManager>();
        BattleMenu = BattleMenuObject.GetComponent<BattleMenu>();
        WPButtonMoveUI = GameObject.Find("WPButtonMoveUI").GetComponent<WPButtonMoveUI>();
        FogManager = GameObject.Find("FogManager").GetComponent<FogManager>();
        CastleMenu = CastleMenuObject.GetComponent<CastleMenu>();

        // Must come AFTER all Game objects are found.
        InputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        // Initialize all UI elements
        HeroMenu.Initialize();
        HeroControlMenu.Initialize();
        StartBattleMenu.Initialize();
        WellUIManager.Initialize();
        RuneStoneMenu.Initialize();
        BattleInvitationMenu.Initialize();
        // StatsUIManager.Initialize();
        MerchantUIManager.Initialize();
        BattleMenu.Initialize();
        WPButtonMoveUI.Initialize();
        FogManager.Initialize();
        CastleMenu.Initialize();

        // Must come AFTER all initializations
        InputManager.Initialize();
    }
    
    public void onHeroMove(Hero Hero)
    {
        // Makes all the nevessary UI changes AFTER a Hero has moved to the new Waypoint

        Waypoint HeroRegion = Hero.GetWaypoint();


        // Update UI position-based buttons.
        WellUIManager.DisplayWellButton(HeroRegion.GetWaypointNum());
        MerchantUIManager.UpdateMerchantButton(HeroRegion.GetWaypointNum());

    }

    public StartBattleMenu GetStartBattleMenu()
    {
        return this.StartBattleMenu;
    }
}
