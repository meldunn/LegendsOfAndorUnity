using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FogType { EventCard, Strength, Willpower, Gold, Wineskin, Gor };

public class FogManager : MonoBehaviour
{

    Waypoint[] possibleWaypoints; //ArrayList of possible waypoints
    Waypoint[] finalWaypoints; //Arraylist of waypoints where fogs are located

    public void Initialize()
    {
        possibleWaypoints = new Waypoint[15];
        finalWaypoints = new Waypoint[7];
        string WaypointName;

        for (int i = 0; i < 15; i++)
        {
            WaypointName = "FogWaypoint" + i;
            possibleWaypoints[i] = GameObject.Find(WaypointName).GetComponent<Waypoint>();
        }

        int numWaypointsSelected = 0;
        System.Random rand = new System.Random();
        int randomInt;
        List<int> SelectedWP = new List<int>(7);
        while(numWaypointsSelected < 7)
        {
            //pick number between 0 and 14 incl
            randomInt = rand.Next(15);
            if (numWaypointsSelected == 0)
            {
                SelectedWP.Add(randomInt);
                numWaypointsSelected++;
            }
            else if (!SelectedWP.Contains(randomInt))
            {
                SelectedWP.Add(randomInt);
                numWaypointsSelected++;
            }
            

        }
        printlist(SelectedWP);




    }
    //for testing
    public void printlist(List<int> list)
    {
        for (int i = 0; i< list.Count; i++)
        {
            //Debug.Log(list[i]);
        }
    }
}
