using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    // Array of waypoints
    private Waypoint[] Waypoint;

    // Adjacency list of tiles for hero move
    int[][] AdjListHero = new int[][]
    {
        new int[] {-1},
        new int[] {0, 2, 3, 4},
        new int[] {0, 1, 3, 6, 14},
        new int[] {1, 2, 4, 10, 14, 19, 20},
        new int[] {0, 1, 3, 5, 20, 21},
        new int[] {0, 4, 21}, //waypoint 5, ie if a hero is on wp 5 they are allowed to move to tiles 0, 4, 21
        new int[] {0, 2, 11, 13, 14, 17},
        new int[] {0, 8, 9, 11, 15},
        new int[] {7, 9 , 11},
        new int[] {7, 8, 15},
        new int[] {3, 14, 18, 19}, //waypoint 10
        new int[] {0, 6, 7, 8, 12},
        new int[] {11, 13},
        new int[] {6, 11, 12, 16, 17},
        new int[] {2, 3, 6, 10, 17, 18},
        new int[] {7, 9}, //waypoint 15
        new int[] {13, 17, 32, 36, 38, 48},
        new int[] {6, 13, 14, 16, 18, 36},
        new int[] {19, 14, 17, 28, 36, 72},
        new int[] {3, 10, 18, 20, 22, 23, 72},
        new int[] {3, 4, 19, 21, 22}, //waypoint 20
        new int[] {4, 5, 20, 22, 24},
        new int[] {19, 20, 21, 23, 24},
        new int[] {19, 22, 24, 25, 31, 34, 35, 72},
        new int[] {21, 22, 23, 25},
        new int[] {23, 24, 26, 27, 31}, //waypoint 25
        new int[] {25, 27},
        new int[] {25, 26, 31},
        new int[] {29, 72, 18, 36, 38},
        new int[] {30, 34, 72, 28},
        new int[] {33, 35, 34, 29}, // waypoint 30
        new int[] {27, 25, 23, 35, 33},
        new int[] {16, 38},
        new int[] {30, 31, 35},
        new int[] {23, 72, 29, 30, 35},
        new int[] {23, 31, 33, 30, 34}, // waypoint 35
        new int[] {17, 18, 28, 38, 16},
        new int[] {41},
        new int[] {39, 28, 26, 16, 32},
        new int[] {38, 40, 42, 43},
        new int[] {29, 41}, // waypoint 40
        new int[] {37, 40},
        new int[] {29, 43, 44},
        new int[] {71, 45, 44, 42, 39},
        new int[] {42, 43, 45, 46},
        new int[] {44, 46, 43, 64, 65}, // waypoint 45
        new int[] {44, 45, 47, 64},
        new int[] {46, 48, 53, 54, 56},
        new int[] {16, 49, 50, 51, 53, 47},
        new int[] {50, 48},
        new int[] {48, 49, 51, 52}, // waypoint 50
        new int[] {48, 50, 52, 55, 53},
        new int[] {50, 51, 55},
        new int[] {51, 48, 47, 54, 55},
        new int[] {47, 53, 55, 57, 56},
        new int[] {52, 51, 53, 54, 57}, // waypoint 55
        new int[] {47, 54, 57, 63},
        new int[] {55, 54, 56, 63, 58, 59},
        new int[] {59, 60, 62, 61, 63},
        new int[] {57, 58, 60},
        new int[] {58, 59, 62}, // waypoint 60
        new int[] {62, 58, 63, 64},
        new int[] {60, 58, 61},
        new int[] {56, 57, 58, 61, 64},
        new int[] {63, 61, 65, 45, 46},
        new int[] {45, 64, 66}, // waypoint 65
        new int[] {65, 67},
        new int[] {68, 66},
        new int[] {69, 67},
        new int[] {70, 68},
        new int[] {81, 69}, // waypoint 70
        new int[] {-2},
        new int[] { -2},
        new int[] { -2},
        new int[] { -2},
        new int[] { -2}, // waypoint 75
        new int[] { -2},
        new int[] { -2},
        new int[] { -2},
        new int[] { -2},
        new int[] {-2}, // waypoint 80
        new int[] {82, 70},
        new int[] {84, 81},
        new int[] {-2},
        new int[] {82}, // waypoint 84

    };

    // Adjacency list of tiles for creature move (ie where the arrow allows creature to move to)
    // Adj of -1 means ur at castle, adj of -2 means tile doesnt exist ie null
    int[] AdjListCreature = new int[]
    {
        -1, //castle ie wp 0
        0,
        0,
        1,
        0,
        0, //waypoint 5, example: if monster is on waypoint 5, at end of day they will move to wp 0
        0,
        0,
        7,
        7,
        3, //waypoint 10
        0,
        11,
        6,
        2,
        7, //waypoint 15
        13,
        6,
        14,
        3,
        3, //waypoint 20
        4,
        19,
        19,
        21,
        24, //waypoint 25
        25,
        25,
        18,
        28,
        29, //waypoint 30
        23,
        16,
        30,
        23,
        23, //waypoint 35
        16,
        41,
        16,
        38,
        39, //waypoint 40
        40,
        39,
        39,
        42,
        43, //waypoint 45
        44,
        46,
        16,
        48,
        48, //waypoint 50
        48,
        50,
        47,
        47,
        51, //waypoint 55
        47,
        54,
        57,
        57,
        59, //waypoint 60
        58,
        58,
        56,
        45,
        45, //waypoint 65
        65,
        66,
        67,
        68,
        69, //waypoint 70
        -2,
        -2,
        -2,
        -2,
        -2, //waypoint 75
        -2,
        -2,
        -2,
        -2,
        -2, //waypoint 80
        70,
        81,
        -2,
        82, //waypoint 84
    };

    // Start is called before the first frame update
    void Start()
    {
        // Not used to ensure Managers are started in the right order.
        // GameManager calls Initialize instead.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        // Initialize references to the waypoints and set their numbers
        Waypoint = new Waypoint[85];

        string WaypointName;

        for (int i = 0; i <= 84; i++)
        {
            if (IsValidWaypoint(i)) // Skip waypoints that don't exist
            {
                WaypointName = "Waypoint (" + i + ")";
                Waypoint[i] = GameObject.Find(WaypointName).GetComponent<Waypoint>();
                Waypoint[i].SetWaypointNum(i);  // Set the waypoint's number
                Waypoint[i].SetWPAdjList(GetWPAdjList(i));

            }
        }
    }

    public int[] GetWPAdjList(int WaypointNum)
    {
        int[] list = new int[AdjListHero[WaypointNum].Length];

        //Debug.Log("length of AdjListHero[WaypointNum] is " + AdjListHero[WaypointNum].Length);
        //Debug.Log("WP num is " + WaypointNum);
        ////Debug.Log("AdjListHero[WaypointNum][0] " + AdjListHero[WaypointNum][0]);

        //Debug.Log("in WPManager GetWPAdjList fn");
        //int i = 0;
        //while (AdjListHero[WaypointNum][i] != null)
        //{
        //    Debug.Log("index is " + i);
        //    list[i] = AdjListHero[WaypointNum][i];
        //    i++;
        //}
        for (int i =0; i< AdjListHero[WaypointNum].Length; i++)
        {
            //Debug.Log("index is " + i);
            list[i] = AdjListHero[WaypointNum][i];
        }

       // Debug.Log("length of adj list is "+ list.Length);
        return list;

        
    }
    public bool IsValidWaypoint(int WaypointNum)
    {
        return ( (WaypointNum >= 0 && WaypointNum <= 72) || (WaypointNum >= 80 && WaypointNum <= 84) );
    }

    // Returns the waypoint corresponding to the given region number
    public Waypoint GetWaypoint(int RegionNum)
    {
        if (IsValidWaypoint(RegionNum)) return Waypoint[RegionNum];
        else
        {
            Debug.LogError("Cannot get waypoint #" + RegionNum + ", invalid region number.");
            return null;
        }
    }

    // Returns a reference to the waypoint after the specified one when following the monster advancement arrows
    public Waypoint GetNext(Waypoint Region)
    {
        int NumOfNext = AdjListCreature[Region.GetWaypointNum()];

        if (NumOfNext < 0) return null;         // Return null if the given location doesn't have a valid next region
        else return GetWaypoint(NumOfNext);
    }

    // Returns whether the two waypoints are adjacent based on the Hero adjacency list
    public bool AreAdjacent(Waypoint FirstRegion, Waypoint SecondRegion)
    {
        if (FirstRegion == null || SecondRegion == null) return false;

        int FirstRegionNum = FirstRegion.GetWaypointNum();
        int SecondRegionNum = SecondRegion.GetWaypointNum();

        int IndexOfSecondRegion = Array.IndexOf(AdjListHero[FirstRegionNum], SecondRegionNum);     

        if (IndexOfSecondRegion < 0) return false;
        else return true;
    }

    public void ReplenishAllWells()
    {
        // TODO: Fix Null pointer exception for wells array
        //
        // Waypoints[5].ReplenishWell();
        // Waypoints[35].ReplenishWell();
        // Waypoints[45].ReplenishWell();
        // Waypoints[55].ReplenishWell();
        Debug.Log("Request to Replenish All Wells");
    }

    //public int[] GetRow(int[,] matrix, int rowNumber)
    //{
    //    return Enumerable.Range(0, matrix.GetLength(1))
    //            .Select(x => matrix[rowNumber, x])
    //            .ToArray();
    //}
    
}
