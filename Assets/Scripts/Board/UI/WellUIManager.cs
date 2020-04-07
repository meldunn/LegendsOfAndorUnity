using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WellUIManager : MonoBehaviour
{

    private GameManager GameManager;

    private GameObject Well5Image;
    private GameObject Well35Image;
    private GameObject Well45Image;
    private GameObject Well55Image;

    private GameObject Well5Button;
    private GameObject Well35Button;
    private GameObject Well45Button;
    private GameObject Well55Button;

    private Button Button5;
    private int[] WellPosition = {5, 35, 45, 55};

    public void Initialize()
    {
        Well5Button = GameObject.Find("well5Button");
        Well35Button = GameObject.Find("well35Button");
        Well45Button = GameObject.Find("well45Button");
        Well55Button = GameObject.Find("well55Button");

        Well5Image = GameObject.Find("well5");
        Well35Image = GameObject.Find("well35");
        Well45Image = GameObject.Find("well45");
        Well55Image = GameObject.Find("well55");

        PlaceWell(Well5Image, 5);
        PlaceWell(Well35Image, 35);
        PlaceWell(Well45Image, 45);
        PlaceWell(Well55Image, 55);
        toggleGameObjectVisibility(Well5Button);
        toggleGameObjectVisibility(Well35Button);
        toggleGameObjectVisibility(Well45Button);
        toggleGameObjectVisibility(Well55Button);

    }

    private void PlaceWell(GameObject WellImage, int WaypointNum)
    {
        string WaypointName = "Waypoint ("+WaypointNum+")";

        Waypoint Waypoint = GameObject.Find(WaypointName).GetComponent<Waypoint>();

        WellImage.transform.Translate(Waypoint.GetLocation() - WellImage.transform.position);
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
    
    // Called when a Hero drinks from a well
    private void toggleButtonFocus(GameObject ButtonObject, bool Focused)
    {
        if(ButtonObject != null)
        {
            Button Button = ButtonObject.GetComponent<Button>();
            if(Focused)
            {
                Button.interactable = false;
            }
            else
            {
                Button.interactable = true;
            }
        }
        else
        {
            Debug.Log("Error. GameObject reference null");
        }
    }

    // Called when a Hero lands on a waypoint with a well on it
    //public void PositionWells(int WaypointNum)
    //{
        //Waypoint Waypoint = GameObject.Find("Waypoint (5)").GetComponent<Waypoint>();
        //Vector3 Image_Location = Waypoint.GetLocation();
        //Waypoint.transform.Translate;

        //switch (WaypointNum)
        //{
        //    case(5):
        //        
        //        //toggleGameObjectVisibility(Well5Button);
        //        break;

        //    case(45):
        //        //toggleGameObjectVisibility(Well45Button);
        //        break;

        //    case(35):
        //        //toggleGameObjectVisibility(Well35Button);
        //        break;

        //    case(55):
        //        //toggleGameObjectVisibility(Well55Button);
        //        break;
        //        
        //    default:
        //        Debug.Log("Error. No well on waypoint "+WaypointNum);
        //        break;
        //}
    //}

    // Called when a Hero lands on a waypoint with a well on it
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
    
    // Allows well button to be clicked with a new day
    public void FocusWellButton(int WaypointNum)
    {
        switch (WaypointNum)
        {
            case(5):
                toggleButtonFocus(Well5Button, false);
                break;

            case(45):
                toggleButtonFocus(Well45Button, false);
                break;

            case(35):
                toggleButtonFocus(Well35Button, false);
                break;

            case(55):
                toggleButtonFocus(Well55Button, false);
                break;
                
            default:
                Debug.Log("Error. No well on waypoint "+WaypointNum);
                break;
        }
    }
}
