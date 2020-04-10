﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WPButtonMoveUI : MonoBehaviour
{

    //TODO: fix issue where if monster and WPbutoon on on same tile cant click WP button, potential fix: when making them visible check if space is occupied if yes then change x/y by a bit

    int[] Location = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9 , 10, 11,12,13,14,15,16,17,18,
                        19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50,
                        51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 81, 82, 84 };

    public List<GameObject> WPButton = new List<GameObject>(73); 

    public void Initialize()
    {
        Debug.Log("in WPButtonMoveUI initializer");
        string ButtonName = "";
        int j;
        for (int i = 0; i < 85; i++)
        {
            if (isValidIndex(i))
            {
                j = i;
                //check if special index
                if (i == 81)
                {
                    j = 71;
                }
                else if (i == 82)
                {
                    j = 72;
                }
                else if (i == 84)
                {
                    j = 73;
                }

                ButtonName = "WPbutton" + Location[j].ToString();
                if (GameObject.Find(ButtonName) != null)
                {
                    WPButton.Add(GameObject.Find(ButtonName));
                    Visibility(WPButton[j], true);
                }
            }
        }

        placeWPButtons();
        //for (int i = 0; i < 85; i++)
        //{
        //    Visibility(WPButton[i], true);
        //}


    }

    public bool isValidIndex(int i)
    {
        if (i == 71 || i == 72 || i == 73|| i == 74 || i == 75 || i == 76 || i == 77 || i == 78 || i == 79 || i == 80 || i == 83)
        {
            return false;
        }
        else
        {
            return true;
        }
    }



    public void placeWPButtons()
    {
        Debug.Log("in placeWPButtons");
        string WaypointName = "";
        Debug.Log("WPButon count in placebuttons is " + WPButton.Count);
        int j;
        for (int i = 0; i < 85; i++)
        {
            if (isValidIndex(i))
            {
                j = i;
                //check if special index
                if (i == 81)
                {
                    j = 71;
                }
                else if (i == 82)
                {
                    j = 72;
                }
                else if (i == 84)
                {
                    j = 73;
                }


                WaypointName = "Waypoint (" + Location[j] + ")";
                Waypoint waypoint = GameObject.Find(WaypointName).GetComponent<Waypoint>();

                //WPButton[i].transform.Translate(Waypoint.GetLocation()
                //        - WPButton[i].transform.position);

                WPButton[j].transform.position = waypoint.transform.position;
                WPButton[j].SetActive(false); //hide buttons
            }
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
    public void OnMouseUp()
    {
        Debug.Log("clicked");
        Debug.Log(" clicked is " + this.gameObject.name);
        Visibility(this.gameObject, false);
    }

    public void toMakeVisible(int[] list)
    {
        for (int i =0; i< list.Length; i++)
        {
            int j = list[i];
            if (j >= 0)
            {
                //check if special index
                if (j == 81)
                {
                    j = 71;
                }
                else if (j == 82)
                {
                    j = 72;
                }
                else if (j == 84)
                {
                    j = 73;
                }
                Visibility(WPButton[j], true); //make WP button visible
            }
        }
    }

}