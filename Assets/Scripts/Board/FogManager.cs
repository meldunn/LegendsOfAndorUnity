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
        possibleWaypoints = new Waypoint[14];
        string WaypointName;

        for (int i = 0; i < 15; i++)
        {
            WaypointName = "FogWaypoint(" + i + ")";
            Waypoint[i] = GameObject.Find(WaypointName).GetComponent<Waypoint>();
        }
    }
}
