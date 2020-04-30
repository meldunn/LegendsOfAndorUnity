using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum NarratorLetter
{
    A = 1,
    B = 2,
    C = 3,
    D = 4,
    E = 5,
    F = 6,
    G = 7,
    H = 8,
    I = 9,
    J = 10,
    K = 11,
    L = 12,
    M = 13,
    N = 14,
    Z = 15
};

public class NarratorManager : MonoBehaviourPun
{
    // Array of waypoints
    private NarratorWaypoint[] waypoints = new NarratorWaypoint[16];

    // Marker placed on narration track
    public static NarratorMarker marker;

    private NarratorLetter RuneStoneLetter;

    public static NarratorLetter curLetter = NarratorLetter.A;

    LegendCardManager legendCardManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize()
    {
        legendCardManager = GameObject.Find("LegendCardManager").GetComponent<LegendCardManager>();
        marker = GameObject.Find("NarratorMarker").GetComponent<NarratorMarker>();
        string WaypointName;
        for (int i = 1; i <= 15; i++)
        {
            NarratorLetter name = (NarratorLetter)i;
            WaypointName = "Waypoint " + name.ToString();
            waypoints[i] = GameObject.Find(WaypointName).GetComponent<NarratorWaypoint>();
            //Debug.Log(waypoints[i].GetLocation());
            //Debug.Log(i);
        }
        //Debug.Log(waypoints[2].GetLocation());
        marker.transform.SetPositionAndRotation(waypoints[1].GetLocation(), Quaternion.identity);
    }

    [PunRPC]
    public void advanceNarratorRPC(int regionNumber)
    {
        curLetter++;
        if(curLetter == RuneStoneLetter)
        {
            legendCardManager.activateRuneStoneLegendCard();
        }
        switch (curLetter)
        {
            case NarratorLetter.B:
                marker.transform.SetPositionAndRotation(waypoints[2].GetLocation(), Quaternion.identity);
                return;
            case NarratorLetter.C:
                marker.transform.SetPositionAndRotation(waypoints[3].GetLocation(), Quaternion.identity);
                legendCardManager.activateLegendCard_C(regionNumber);
                return;
            case NarratorLetter.D:
                marker.transform.SetPositionAndRotation(waypoints[4].GetLocation(), Quaternion.identity);
                legendCardManager.activateRuneStoneLegendCard();
                return;
            case NarratorLetter.E:
                marker.transform.SetPositionAndRotation(waypoints[5].GetLocation(), Quaternion.identity);
                return;
            case NarratorLetter.F:
                marker.transform.SetPositionAndRotation(waypoints[6].GetLocation(), Quaternion.identity);
                return;
            case NarratorLetter.G:
                marker.transform.SetPositionAndRotation(waypoints[7].GetLocation(), Quaternion.identity);
                legendCardManager.activateLegendCard_G();
                return;
            case NarratorLetter.H:
                marker.transform.SetPositionAndRotation(waypoints[8].GetLocation(), Quaternion.identity);
                return;
            case NarratorLetter.I:
                marker.transform.SetPositionAndRotation(waypoints[9].GetLocation(), Quaternion.identity);
                return;
            case NarratorLetter.J:
                marker.transform.SetPositionAndRotation(waypoints[10].GetLocation(), Quaternion.identity);
                return;
            case NarratorLetter.K:
                marker.transform.SetPositionAndRotation(waypoints[11].GetLocation(), Quaternion.identity);
                return;
            case NarratorLetter.L:
                marker.transform.SetPositionAndRotation(waypoints[12].GetLocation(), Quaternion.identity);
                return;
            case NarratorLetter.M:
                marker.transform.SetPositionAndRotation(waypoints[13].GetLocation(), Quaternion.identity);
                return;
            case NarratorLetter.N:
                marker.transform.SetPositionAndRotation(waypoints[14].GetLocation(), Quaternion.identity);
                legendCardManager.activateLegendCard_N();
                return;
            case NarratorLetter.Z:
                marker.transform.SetPositionAndRotation(waypoints[15].GetLocation(), Quaternion.identity);
                return;
            default:
                Debug.LogError("ERROR!");
                return;
        }
    }

    public void advanceNarrator()
    {
        if (PhotonNetwork.IsConnected && photonView.IsMine)
        {
            int RegionNum = UnityEngine.Random.Range(51, 57);
            if (PhotonNetwork.IsConnected)
            {
                photonView.RPC("advanceNarratorRPC", RpcTarget.All, RegionNum);
            }
            else
            {
                advanceNarratorRPC(RegionNum);
            }
        } 
        else
        {
            //Debug.LogError("ADVANCE NARRATOR");
            advanceNarratorRPC(51);
        }

    }

    public void AddRuneStoneCard(NarratorLetter Letter)
    {
        RuneStoneLetter = Letter;
        Debug.Log("Rune stone card placed at "+Letter);
    }

    public void advanceToN()
    {
        curLetter = NarratorLetter.N;
        marker.transform.SetPositionAndRotation(waypoints[14].GetLocation(), Quaternion.identity);
    }
}
