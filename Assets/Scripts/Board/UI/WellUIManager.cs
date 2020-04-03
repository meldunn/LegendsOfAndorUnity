using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellUIManager : MonoBehaviour
{

    private GameManager GameManager;
    private GameObject Well5Button;
    private GameObject Well35Button;
    private GameObject Well45Button;
    private GameObject Well55Button;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Initialize()
    {
        Well5Button = GameObject.Find("well5Button");
        Well35Button = GameObject.Find("well35Button");
        Well45Button = GameObject.Find("well45Button");
        Well55Button = GameObject.Find("well55Button");

        toggleGameObjectVisibility(Well5Button);
        toggleGameObjectVisibility(Well35Button);
        toggleGameObjectVisibility(Well45Button);
        toggleGameObjectVisibility(Well55Button);
    }

    private void toggleGameObjectVisibility(GameObject GameObject)
    {
        // toggle visibility of any GameObject

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
    
    private void toggleButtonFocus(GameObject ButtonObject)
    {
        // TODO
        // Might not be requierd  - if button only pops up if well is available
       // if(ButtonObject != null)
       // {
       //     bool isFocused = ButtonObject.interactable;
       //     ButtonObject.interactable = !isFocused;
       // }
       // else
       // {
       //     Debug.Log("Error. GameObject reference null");
       // }
    }

    public void DisplayWellButton(int WaypointNum)
    {
        switch (WaypointNum)
        {
            case(5):
                toggleGameObjectVisibility(Well5Button);
                break;

            case(45):
                toggleGameObjectVisibility(Well45Button);
                break;

            case(35):
                toggleGameObjectVisibility(Well35Button);
                break;

            case(55):
                toggleGameObjectVisibility(Well55Button);
                break;
                
            default:
                Debug.Log("Error. No well on waypoint "+WaypointNum);
                break;
        }
    }
    
    public void FocusWellButton(int WaypointNum)
    {
        switch (WaypointNum)
        {
            case(5):
                toggleButtonFocus(Well5Button);
                break;

            case(45):
                toggleButtonFocus(Well45Button);
                break;

            case(35):
                toggleButtonFocus(Well35Button);
                break;

            case(55):
                toggleButtonFocus(Well55Button);
                break;
                
            default:
                Debug.Log("Error. No well on waypoint "+WaypointNum);
                break;
        }
    }
}
