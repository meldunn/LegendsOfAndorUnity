using System.Collections;
using System.Collections.Generic;
//using System.Windows.Forms.Timer;
//using System.Windows.Forms;
using UnityEngine;

public enum FogType { EventCard, Strength, Willpower2, Willpower3, Gold, Wineskin, Gor, Brew };

public class FogManager : MonoBehaviour
{
    private CreatureManager CreatureManager;
    private ChatManager ChatManager;
    private static GameManager GM;

    static Waypoint[] possibleWaypoints; //ArrayList of possible fog waypoints
    static Waypoint[] finalWaypoints; //Arraylist of fogwaypoints where fogs are located

    Fog[] foglist; //list of fog

    public static List<FogBack> FogBackList = new List<FogBack>(7);
    //public static List<Waypoint> FogBackList = new List<Waypoint>(7);
    public static List<FogFront> FogFrontList = new List<FogFront>(7);

    static int[] TileWPNum = { 13, 8, 11, 47, 46, 32, 12, 16, 64, 48, 44, 42, 56, 49, 63 }; //in order of fogwp number 


    public void Initialize()
    {

        CreatureManager = GameObject.Find("CreatureManager").GetComponent<CreatureManager>();
        ChatManager = GameObject.Find("ChatManager").GetComponent<ChatManager>();
        GM  = GameObject.Find("GameManager").GetComponent<GameManager>();

        //possibleWaypoints = new Waypoint[15];
        finalWaypoints = new Waypoint[15];
        foglist = new Fog[15];
        string WaypointName;

        //for (int i = 0; i < 15; i++)
        //{
        //    WaypointName = "FogWaypoint" + i;
        //    possibleWaypoints[i] = GameObject.Find(WaypointName).GetComponent<Waypoint>();
        //}

        int numWaypointsSelected = 0;
        System.Random rand = new System.Random();
        int randomInt;
        List<int> SelectedWP = new List<int>(15);
        while (numWaypointsSelected < 15)
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
        for (int i = 0; i < 15; i++)
        {
            WaypointName = "FogWaypoint" + SelectedWP[i];
            finalWaypoints[i] = GameObject.Find(WaypointName).GetComponent<Waypoint>();
        }

        //set fogback list
        for (int i = 1; i < 16; i++)
        {
            WaypointName = "FogBack" + i;
            Debug.Log(WaypointName);
            FogBackList.Add(GameObject.Find(WaypointName).GetComponent<FogBack>());
        }

        //set fogfront list TODO

        //set willpower front fog
        FogFrontList.Add(GameObject.Find("FogFaceWillpower2").GetComponent<FogFront>());
        FogFrontList.Add(GameObject.Find("FogFaceWillpower3").GetComponent<FogFront>());
        //set gor front fog
        FogFrontList.Add(GameObject.Find("FogFaceGor").GetComponent<FogFront>());
        FogFrontList.Add(GameObject.Find("FogFaceGor1").GetComponent<FogFront>());
        //set fog front strength
        FogFrontList.Add(GameObject.Find("FogFaceStrength").GetComponent<FogFront>());
        //set fog face gold
        FogFrontList.Add(GameObject.Find("FogFaceGold (0)").GetComponent<FogFront>());
        FogFrontList.Add(GameObject.Find("FogFaceGold (1)").GetComponent<FogFront>());
        FogFrontList.Add(GameObject.Find("FogFaceGold (2)").GetComponent<FogFront>());
        //set fog face event card
        FogFrontList.Add(GameObject.Find("FogFaceEventCard (0)").GetComponent<FogFront>());
        FogFrontList.Add(GameObject.Find("FogFaceEventCard (1)").GetComponent<FogFront>());
        FogFrontList.Add(GameObject.Find("FogFaceEventCard (2)").GetComponent<FogFront>());
        FogFrontList.Add(GameObject.Find("FogFaceEventCard (3)").GetComponent<FogFront>());
        FogFrontList.Add(GameObject.Find("FogFaceEventCard (4)").GetComponent<FogFront>());
        //set fog face brew
        FogFrontList.Add(GameObject.Find("FogFaceBrew").GetComponent<FogFront>());
        //set fog face wineskin
        FogFrontList.Add(GameObject.Find("FogFaceWineskin").GetComponent<FogFront>());

        //set and place fogs
        for (int i = 0; i < 15; i++)
        {
            if (i == 0)
            {
                Fog newFog = new Fog(TileWPNum[SelectedWP[i]], FogBackList[i], FogFrontList[i], FogType.Willpower2);
                foglist[i] = newFog;
            }
            if (i == 1)
            {
                Fog newFog = new Fog(TileWPNum[SelectedWP[i]], FogBackList[i], FogFrontList[i], FogType.Willpower3);
                foglist[i] = newFog;
            }
            if (i == 2 || i == 3)
            {
                Fog newFog = new Fog(TileWPNum[SelectedWP[i]], FogBackList[i], FogFrontList[i], FogType.Gor);
                foglist[i] = newFog;
            }
            if (i == 4)
            {
                Fog newFog = new Fog(TileWPNum[SelectedWP[i]], FogBackList[i], FogFrontList[i], FogType.Strength);
                foglist[i] = newFog;
            }
            if (i == 5 || i == 6 || i == 7)
            {
                Fog newFog = new Fog(TileWPNum[SelectedWP[i]], FogBackList[i], FogFrontList[i], FogType.Gold);
                foglist[i] = newFog;
            }
            if (i <= 12 && i >= 8)
            {
                Fog newFog = new Fog(TileWPNum[SelectedWP[i]], FogBackList[i], FogFrontList[i], FogType.EventCard);
                foglist[i] = newFog;
            }
            if (i == 13)
            {
                Fog newFog = new Fog(TileWPNum[SelectedWP[i]], FogBackList[i], FogFrontList[i], FogType.Brew);
                foglist[i] = newFog;
            }
            if (i == 14)
            {
                Fog newFog = new Fog(TileWPNum[SelectedWP[i]], FogBackList[i], FogFrontList[i], FogType.Wineskin);
                foglist[i] = newFog;
            }


            //Fog newFog= new Fog(TileWPNum[SelectedWP[i]],FogBackList[i]);
            //foglist[i] = newFog;



            // WaypointName = "Waypoint (" + TileWPNum[SelectedWP[i]] + ")";
            WaypointName = "FogWaypoint" + SelectedWP[i];
           // WaypointName = finalWaypoints[i].ToString();
            Waypoint waypoint = GameObject.Find(WaypointName).GetComponent<Waypoint>();
            foglist[i].GetFogBackCard().transform.position = waypoint.transform.position;
            foglist[i].GetFogFrontCard().transform.position = waypoint.transform.position;


            //FOR TESTING, REMOVE THIS

            foglist[i].GetFogBackCard().gameObject.SetActive(false);


            // TO UNCOMMENT

            //hide fog front
            //foglist[i].GetFogFrontCard().gameObject.SetActive(false);
        }




    }

    public void triggerFogAtWP(int wpnumber)
    {
        for (int i = 0; i < foglist.Length; i++)
        {
            if (wpnumber == foglist[i].GetWPNum())
            {
                Debug.Log("wpnum for fog is " + wpnumber);
                //return foglist[i];
                //foglist[i].GetFogBackCard().HideFogBack();
                foglist[i].GetFogBackCard().gameObject.SetActive(false);

                //show fog front for 5 seconds
                foglist[i].GetFogFrontCard().gameObject.SetActive(true);

                //for (int j =0; j< 10000; j++)
                //{

                //}
                //foglist[i].GetFogFrontCard().gameObject.SetActive(false);

                if (foglist[i].GetFogType() == FogType.Gor)
                {
                    Debug.Log("Fog is Gor type");
                    TriggerFogGor(wpnumber);
                    foglist[i].GetFogFrontCard().gameObject.SetActive(false); //hide front card

                }


               
                return;
            }

        }
        Debug.Log("this wp doesnt have fog");
        //return null;
    }

    //private bool Waited(float seconds)
    //{
    //    timerMax = seconds;

    //    timer += Time.deltaTime;

    //    if (timer >= timerMax)
    //    {
    //        return true; //max reached - waited x - seconds
    //    }
    //}

    public void TriggerFogGor(int wpnumber)
    {
        CreatureManager.Spawn(CreatureType.Gor, wpnumber);
        ChatManager.SendSystemMessage("A fog token has been activated by the" + GM.GetCurrentTurnHero().GetHeroType() + "! A Gor Creature has spawned");
    }



    //for testing
    public void printlist(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log("FogWP list " + list[i]);
        }
    }
}
