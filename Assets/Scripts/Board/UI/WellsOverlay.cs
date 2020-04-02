using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellsOverlay : MonoBehaviour, Observer
{

    // Reference to GameManager
    private GameManager GameManager;

    GameObject WellsPanel;

    public void OpenPanel()
    {
        if(WellsPanel != null)
        {
            bool isActive = WellsPanel.activeSelf;
            Debug.Log("Panel visibility:");
            Debug.Log(isActive);
            WellsPanel.SetActive(!isActive);
        }
    }

    public void Initialize()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // Register as an observer of GameManager
        GameManager.Attach(this);

        // Initialize references to children components
        WellsPanel = GameObject.Find("WellsPanel");

        Debug.Log("Wells Overlay ...");
        if(WellsPanel != null)
        {
            Debug.Log("Working!!!");
        }

    }

    public void log(){
        Debug.Log("testiing");
    }

    public void UpdateData(string Category)
    {
        Debug.Log("updated");
    }
}
