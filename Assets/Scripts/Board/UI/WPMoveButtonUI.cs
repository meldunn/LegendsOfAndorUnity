using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WPMoveButtonUI : MonoBehaviour
{

    int[] Location = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9 , 10, 11,12,13,14,15,16,17,18,
                        19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35};
    public List<GameObject> WPButton = new List<GameObject>(3); //TODO: change number in bracket

    public void Initialize()
    {
        string ButtonName = "";
        for (int i =0; i< WPButton.Count; i++)
        {
            ButtonName = "WPbutton" + Location[i].ToString();
            if (GameObject.Find(ButtonName) == null)
            {
                Debug.Log("No icon named " + ButtonName);
            }
            WPButton.Add(GameObject.Find(ButtonName));
        }

        placeWPButtons();
        for (int i = 0; i < WPButton.Count; i++)
        {
            Visibility(WPButton[i], true);
        }


    }



    public string WhoAmI(GameObject idontknow)
    {
        string foundButton = "";
        for (int i = 0; i < WPButton.Count; i++)
        {
            if (idontknow == WPButton[i])
            {
                foundButton = WPButton[i].name;
                break;
            }
        }
        return foundButton;
    }
    //on the button script, trigger the function.
    public void WhatIsMyName()
    {
        string myNameIs = WhoAmI(this.gameObject);
    }


    public void placeWPButtons()
    {
        string WaypointName = "";
        for (int i = 0; i < WPButton.Count; i++)
        {
            WaypointName = "Waypoint (" + Location[i] + ")";

            Waypoint Waypoint = GameObject.Find(WaypointName).GetComponent<Waypoint>();

            WPButton[i].transform.Translate(Waypoint.GetLocation()
                    - WPButton[i].transform.position);

            // WPButton[i].transform.position = Waypoint.transform.position;


            WPButton[i].SetActive(true);
        }
    }
    private void Visibility(GameObject GameObject, bool Show)
    {
        if (GameObject != null)
        {
            if (Show) GameObject.SetActive(true);
            else GameObject.SetActive(false);
        }
        else Debug.Log("Error in WPMoveButtonUI. Referenced null");
    }



}
