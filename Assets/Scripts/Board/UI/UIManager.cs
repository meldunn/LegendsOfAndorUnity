using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Dynamically loaded UI elements
    HeroMenu HeroMenu;
    HeroControlMenu HeroControlMenu;
    StartBattleMenu StartBattleMenu;

    // Overlays
    GameObject WellsOverlay;

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
        handleKeyStroke();
    }

    public void Initialize()
    {
        // Initialize references to UI elements
        HeroMenu = GameObject.Find("HeroMenu").GetComponent<HeroMenu>();
        HeroControlMenu = GameObject.Find("HeroControlMenu").GetComponent<HeroControlMenu>();
        StartBattleMenu = StartBattleMenuObject.GetComponent<StartBattleMenu>();

        // Overlay references (UI elements)
        WellsOverlay = GameObject.Find("WellsOverlay");

        // Initialize all UI elements
        HeroMenu.Initialize();
        HeroControlMenu.Initialize();
        StartBattleMenu.Initialize();
    }

    public StartBattleMenu GetStartBattleMenu()
    {
        return this.StartBattleMenu;
    }

    private void handleKeyStroke()
    {
        // Toggle Wells Overlay
        if(Input.GetKeyDown("w"))
        {
            toggleGameObjectVisibility(WellsOverlay);
        }
    }
    private void toggleGameObjectVisibility(GameObject GameObject)
    {
            if(GameObject != null)
            {
                bool isActive = GameObject.activeSelf;
                GameObject.SetActive(!isActive);
            }
            else
            {
                Debug.Log("Error. GameObject referenced null");
            }
    }
}
