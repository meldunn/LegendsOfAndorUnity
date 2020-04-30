using System.Collections;
using System.Collections.Generic;
//using System.Windows.Forms.Timer;
//using System.Windows.Forms;
using UnityEngine;
using Photon.Pun;

public enum FogType { EventCard, Strength, Willpower2, Willpower3, Gold, Wineskin, Gor, Brew };

public class FogManager : MonoBehaviourPun
{
    private CreatureManager CreatureManager;
    private ChatManager ChatManager;
    private EventCardManager EventCardManager;
    private static GameManager GM;
    private int[] WAYS = new int[15];

    static Waypoint[] possibleWaypoints; //ArrayList of possible fog waypoints
    static Waypoint[] finalWaypoints; //Arraylist of fogwaypoints where fogs are located

    Fog[] foglist; //list of fog

    public static List<FogBack> FogBackList = new List<FogBack>(7);
    //public static List<Waypoint> FogBackList = new List<Waypoint>(7);
    public static List<FogFront> FogFrontList = new List<FogFront>(7);
    List<int> SelectedWP = new List<int>(15);

    static int[] TileWPNum = { 13, 8, 11, 47, 46, 32, 12, 16, 64, 48, 44, 42, 56, 49, 63 }; //in order of fogwp number

    public void Initialize()
    {
        CreatureManager = GameObject.Find("CreatureManager").GetComponent<CreatureManager>();
        ChatManager = GameObject.Find("ChatManager").GetComponent<ChatManager>();
        GM  = GameObject.Find("GameManager").GetComponent<GameManager>();
        EventCardManager = GameObject.Find("EventCardManager").GetComponent<EventCardManager>();
        //possibleWaypoints = new Waypoint[15];

        finalWaypoints = new Waypoint[15];
        foglist = new Fog[15];
        string WaypointName;

        // "Random" selection
        int seed = (int) System.DateTime.Now.Minute;
        List<int> SelectedWP = new List<int>(15);
        for(int i=0; i<15; i++)
        {
            SelectedWP.Add((seed+i)%15);
        }

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
            //Debug.Log(WaypointName);
            FogBackList.Add(GameObject.Find(WaypointName).GetComponent<FogBack>());
        }


        //set and place fogs
        for (int i = 0; i < 15; i++)
        {
            if (i == 0)
            {
                Fog newFog = new Fog(TileWPNum[SelectedWP[i]], 
                        FogBackList[i], 
                        FogFrontList[i], 
                        FogType.Willpower2);
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


            // WaypointName = "Waypoint (" + TileWPNum[SelectedWP[i]] + ")";
            WaypointName = "FogWaypoint" + SelectedWP[i];
           // WaypointName = finalWaypoints[i].ToString();
            Waypoint waypoint = GameObject.Find(WaypointName).GetComponent<Waypoint>();
            foglist[i].GetFogBackCard().transform.position = waypoint.transform.position;
            foglist[i].GetFogFrontCard().transform.position = waypoint.transform.position;

            

            foglist[i].GetFogBackCard().gameObject.SetActive(true);

            //hide fog front
            foglist[i].GetFogFrontCard().gameObject.SetActive(false);
        }

    }


    public void UnveilFogTelescope(int[] PosWPList)
    {
        for (int i =0; i < PosWPList.Length; i++)
        {
            for (int j =0; j< foglist.Length; j++)
            {
                if (PosWPList[i] == foglist[j].GetWPNum())
                {
                    foglist[j].UncoverBack();
                }
            }
        }
    }

    public void triggerFogAtWP(int wpnumber)
    {

        for (int i = 0; i < foglist.Length; i++)
        {
            if (wpnumber == foglist[i].GetWPNum())
            {

                if (foglist[i].IsFogUnused == true)
                {
                    if(PhotonNetwork.IsConnected) photonView.RPC("ActivateFogRPC", RpcTarget.All, i, wpnumber);
                    else ActivateFogRPC(i, wpnumber);
                }
                Debug.Log("fog already used");
                return;
            }

        }
        Debug.Log("this wp doesnt have fog");
        //return null;
    }

    [PunRPC]
    private void ActivateFogRPC(int i, int wpnumber)
    {
        string Message = "";
        foglist[i].GetFogBackCard().gameObject.SetActive(false);

        //show fog front 
        foglist[i].GetFogFrontCard().gameObject.SetActive(true);

        // foglist[i].GetFogFrontCard().gameObject.SetActive(false);  //TO REMOVE


        if (foglist[i].GetFogType() == FogType.Gor)
        {
            Debug.Log("Fog is Gor type");
            // TriggerFogGor(wpnumber);
            CreatureManager.Spawn(CreatureType.Gor, wpnumber);
            Message = "A fog token has been activated by the " + GM.GetCurrentTurnHero().GetHeroType() + "! A Gor Creature has spawned";
        }
        if (foglist[i].GetFogType() == FogType.Wineskin)
        {
            Debug.Log("fog is type winesking");
            GM.GetCurrentTurnHero().addItem(ItemType.Wineskin);
            Message = "A fog token has been activated by the " + GM.GetCurrentTurnHero().GetHeroType() + "! They have received a Wineskin";
        }
        if (foglist[i].GetFogType() == FogType.Brew)
        {
            Debug.Log("fog is type brew");
            GM.GetCurrentTurnHero().addItem(ItemType.Witchbrew);
            Message = "A fog token has been activated by the " + GM.GetCurrentTurnHero().GetHeroType() + "! They have received a Witch's Brew";
        }
        if (foglist[i].GetFogType() == FogType.Willpower2)
        {
            Debug.Log("fog is type wp2");
            GM.GetCurrentTurnHero().IncreaseWillpower(2);
            Message = "A fog token has been activated by the " + GM.GetCurrentTurnHero().GetHeroType() + "! They have received 2 Willpower";
        }
        if (foglist[i].GetFogType() == FogType.Willpower3)
        {
            Debug.Log("fog is type wp3");
            GM.GetCurrentTurnHero().IncreaseWillpower(3);
            Message = "A fog token has been activated by the " + GM.GetCurrentTurnHero().GetHeroType() + "! They have received 3 Willpower";
        }
        if (foglist[i].GetFogType() == FogType.Strength)
        {
            Debug.Log("fog is type strength");
            GM.GetCurrentTurnHero().IncreaseStrength(1);
            Message = "A fog token has been activated by the " + GM.GetCurrentTurnHero().GetHeroType() + "! They have received 1 Strength";
        }
        if (foglist[i].GetFogType() == FogType.Gold)
        {
            Debug.Log("fog is type gold");
            GM.GetCurrentTurnHero().ReceiveGold(1);
            Message = "A fog token has been activated by the " + GM.GetCurrentTurnHero().GetHeroType() + "! They have received 1 Gold";
        }
        if (foglist[i].GetFogType() == FogType.EventCard)
        {
            Debug.Log("fog is type eventcard");
            EventCardManager.triggerRandom();
            Message = "A fog token has been activated by the " + GM.GetCurrentTurnHero().GetHeroType() + "! It is an event card!";
        }

        foglist[i].GetFogFrontCard().gameObject.SetActive(false); //hide front card
        foglist[i].IsFogUnused = false;

        if(PhotonNetwork.IsMasterClient) ChatManager.SendSystemMessage(Message);
        return;

    }

    
    public void TriggerFogGor(int wpnumber)
    {
        if(PhotonNetwork.IsConnected) photonView.RPC("TriggerFogGorRPC", RpcTarget.All, wpnumber);
        else TriggerFogGorRPC(wpnumber);
    }

    [PunRPC]
    private void TriggerFogGorRPC(int wpnumber)
    {
        CreatureManager.Spawn(CreatureType.Gor, wpnumber);
        if(PhotonNetwork.IsMasterClient)
        {
            ChatManager.SendSystemMessage("A fog token has been activated by the" + GM.GetCurrentTurnHero().GetHeroType() + "! A Gor Creature has spawned");
        }

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
