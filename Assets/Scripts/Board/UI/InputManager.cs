using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private GameManager GameManager;

    // Get References to all the GameObjects
    private GameObject WellsOverlay;
    private GameObject RuneStoneMenu;
    private GameObject MerchantsOverlay;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        
    }
    public void Initialize()
    {
        // Get references to Game Objects
        WellsOverlay = GameObject.Find("WellsOverlay");
        RuneStoneMenu = GameObject.Find("RuneStoneMenu");
        MerchantsOverlay = GameObject.Find("MerchantsOverlay");
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

    private void HandleInput()
    {
        // Handles keyboard input

        // Toggle Wells Overlay
        if(Input.GetKeyDown("w"))
        {
            toggleGameObjectVisibility(WellsOverlay);
        }

        // Toggle Wells Overlay
        if(Input.GetKeyDown("r"))
        {
            toggleGameObjectVisibility(RuneStoneMenu);
        }

        // Toggle Merchants Overlay
        if(Input.GetKeyDown("m"))
        {
            toggleGameObjectVisibility(MerchantsOverlay);
        }
    }
}
