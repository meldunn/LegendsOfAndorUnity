using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FogType { EventCard, Strength, Willpower, Gold, Wineskin, Gor };

public class FogManager : MonoBehaviour
{

    static Waypoint[] possibleWaypoints; //ArrayList of possible fog waypoints
    static Waypoint[] finalWaypoints; //Arraylist of fogwaypoints where fogs are located

    Fog[] foglist; //list of fog

    public static List<FogBack> FogBackList = new List<FogBack>(7);
    //public static List<Waypoint> FogBackList = new List<Waypoint>(7);
    public static List<GameObject> FogFrontList = new List<GameObject>(7);

    static int[] TileWPNum = {13, 8, 11, 47, 46, 32, 12, 16, 64, 48, 44, 42, 56, 49, 63}; //in order of fogwp number 


    public void Initialize()
    {
        possibleWaypoints = new Waypoint[15];
        finalWaypoints = new Waypoint[7];
        //foglist = new List<Fog>(7);
        foglist = new Fog[7];
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
        printlist(SelectedWP);  //for testing

        //place selected fogwaypoints into finalwaypoints

    

        for (int i = 0; i<7; i++)
        {
            WaypointName = "FogWaypoint" + SelectedWP[i];
            finalWaypoints[i] = GameObject.Find(WaypointName).GetComponent<Waypoint>();
        }

        //set fogback list
        for (int i = 1; i < 8; i++)
        {
            WaypointName = "FogBack" + i;
            //FogBackList[i -1] = GameObject.Find(WaypointName).GetComponent<GameObject>();
            Debug.Log(WaypointName);
            //Debug.Log(GameObject.Find(WaypointName).GetComponent<Waypoint>());
            //FogBackList.Add(GameObject.Find(WaypointName).GetComponent<Waypoint>());
            FogBackList.Add(GameObject.Find(WaypointName).GetComponent<FogBack>());
        }

        //set fogfront list TODO




        //set and place fogs
        for (int i =0; i < 7; i++)
        {
            Fog newFod = new Fog(FogBackList[i]);
            foglist[i] = newFod;


            Debug.Log(FogBackList[i]);
            Debug.Log(foglist[i]);



            Debug.Log("SelectedWP[i] " + TileWPNum[SelectedWP[i]]);



            WaypointName = "Waypoint (" + TileWPNum[SelectedWP[i]] + ")";


            
            
            
            Waypoint waypoint = GameObject.Find(WaypointName).GetComponent<Waypoint>();
            foglist[i].GetFogBackCard().transform.position = waypoint.transform.position;
        }




    }
    //for testing
    public void printlist(List<int> list)
    {
        for (int i = 0; i< list.Count; i++)
        {
            Debug.Log("FogWP list " + list[i]);
        }
    }
}
