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

    }

    private void PlaceWell(GameObject WellImage, int WaypointNum)
    {
        string WaypointName = "Waypoint ("+WaypointNum+")";

        Waypoint Waypoint = GameObject.Find(WaypointName).GetComponent<Waypoint>();

        WellImage.transform.Translate(Waypoint.GetLocation() - WellImage.transform.position);
    }

    private void toggleGameObjectVisibility(GameObject GameObject, bool ShowRequest)
    {
        // toggle visibility of any GameObject
        // Debug.Log(GameObject);

        if(GameObject != null)
        {
            if(ShowRequest) GameObject.SetActive(false);

            else GameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Error. GameObject referenced null");
        }
    }

    // Called when a Hero lands on a waypoint with a well on it
    public void DisplayWellButton(int WaypointNum)
    {
        Vector3 Offset = new Vector3(0,1,0);
        switch (WaypointNum)
        {
            case(5):
                Well5Button.transform.Translate(Well5Image.transform.position - Well5Button.transform.position - Offset);
                toggleGameObjectVisibility(Well5Image, true);
                break;

            case(45):
                Well45Button.transform.Translate(Well45Image.transform.position - Well45Button.transform.position + Offset);
                toggleGameObjectVisibility(Well5Image, true);
                break;

            case(35):
                Well35Button.transform.Translate(Well35Image.transform.position - Well35Button.transform.position + Offset);
                toggleGameObjectVisibility(Well5Image, true);
                break;

            case(55):
                Well55Button.transform.Translate(Well55Image.transform.position - Well35Button.transform.position - Offset);
                toggleGameObjectVisibility(Well5Image, true);
                break;
                
            default:
                Debug.Log("Error. No well on waypoint "+WaypointNum);
                break;
        }
    }
    
    // Allows well button to be clicked with a new day
    public void HideWellButton(int WaypointNum)
    {
        switch (WaypointNum)
        {
            case(5):
                Well5Button.SetActive(false);
                break;

            case(45):
                Well45Button.SetActive(false);
                break;

            case(35):
                Well35Button.SetActive(false);
                break;

            case(55):
                Well55Button.SetActive(false);
                break;
                
            default:
                Debug.Log("Error. No well on waypoint "+WaypointNum);
                break;
        }
    }

    public void WellsReplenished()
    {
        // Resiet
    }
    
}
