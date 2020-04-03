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
    
    // References to Game Overlays
    GameObject WellsOverlayObject;

    // Directly linked UI elements
    [SerializeField]
    GameObject StartBattleMenuObject = null;

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
        // Initialize references to UI elements
        HeroMenu = GameObject.Find("HeroMenu").GetComponent<HeroMenu>();
        HeroControlMenu = GameObject.Find("HeroControlMenu").GetComponent<HeroControlMenu>();
        StartBattleMenu = StartBattleMenuObject.GetComponent<StartBattleMenu>();
        WellUIManager = GameObject.Find("WellUIManager").GetComponent<WellUIManager>();
        InputManager = GameObject.Find("InputManager").GetComponent<InputManager>();

        // Initialize all UI elements
        HeroMenu.Initialize();
        HeroControlMenu.Initialize();
        StartBattleMenu.Initialize();
        WellUIManager.Initialize();
        InputManager.Initialize();
    }
    
    public void onHeroMove(Hero Hero)
    {
        // Makes all the nevessary UI changes AFTER a Hero has moved to the new Waypoint

        Waypoint HeroRegion = Hero.GetWaypoint();
        //Debug.Log("Landed on region "+HeroRegion.GetWaypointNum());
        if(HeroRegion.containsFullWell())
        {
            WellUIManager.DisplayWellButton(HeroRegion.GetWaypointNum());
        }

    }

    public StartBattleMenu GetStartBattleMenu()
    {
        return this.StartBattleMenu;
    }
}
