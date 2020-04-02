using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellsOverlay : MonoBehaviour
{

    public GameObject WellsPanel;

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
}
