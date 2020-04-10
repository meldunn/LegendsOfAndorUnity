using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WPButtonMoveUI : MonoBehaviour
{
    int[] Location = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9 , 10, 11,12,13,14,15,16,17,18,
                        19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35};
    public List<GameObject> WPButton = new List<GameObject>(7); //TODO: change number in bracket

    public void Initialize()
    {
        Debug.Log("in WPButtonMoveUI initializer");
        string ButtonName = "";
        for (int i = 0; i < 7; i++)
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



    public void placeWPButtons()
    {
        Debug.Log("in placeWPButtons");
        string WaypointName = "";
        Debug.Log("WPButon count in placebuttons is " + WPButton.Count);
        for (int i = 0; i < WPButton.Count; i++)
        {
            WaypointName = "Waypoint (" + Location[i] + ")";
            Waypoint waypoint = GameObject.Find(WaypointName).GetComponent<Waypoint>();
         
            //WPButton[i].transform.Translate(Waypoint.GetLocation()
            //        - WPButton[i].transform.position);

            WPButton[i].transform.position = waypoint.transform.position;
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
    public void OnMouseUp()
    {
        Debug.Log("clicked");
        Debug.Log(" clicked is " + this.gameObject.name);
        Visibility(this.gameObject, false);
    }

}
