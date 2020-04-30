using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WPButtonMoveUI : MonoBehaviourPun
{
    [SerializeField]
    private GameObject EndMoveButton = null;

    private static GameManager GM;
    private static HeroManager HeroManager;
    private WaypointManager WaypointManager;

    int[] Location = {0, 1, 2, 3, 4, 5, 6, 7, 8, 9 , 10, 11,12,13,14,15,16,17,18,
                        19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50,
                        51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 81, 82, 84 };

    public static List<GameObject> WPButton = new List<GameObject>(75);
    public static List<GameObject> PathButton = new List<GameObject>(10);



    void Start()
    {
        // Initialize reference to WaypointManager
        WaypointManager = GameObject.Find("WaypointManager").GetComponent<WaypointManager>();
        HeroManager = GameObject.Find("HeroManager").GetComponent<HeroManager>();
       

    }

    public void Initialize(GameManager GameManager)
    {
        Debug.Log("in WPButtonMoveUI initializer");

        //GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        GM = GameManager;


        //initialize WPmovebuttons
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
                    j = 73;
                }
                else if (i == 82)
                {
                    j = 74;
                }
                else if (i == 84)
                {
                    j = 75;
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

        //initiliaze path buttons
        string PathButtonName = "";
        for (int i = 0; i < 10; i++)
        {
            PathButtonName = "PathButton (" + i + ")";
            if (GameObject.Find(PathButtonName) != null)
            {
                PathButton.Add(GameObject.Find(PathButtonName));
                Visibility(PathButton[i], false);
            }
        }
        Debug.Log("pathbutton count is " + PathButton.Count);

    }

    public bool isValidIndex(int i)
    {
        if (i == 73|| i == 74 || i == 75 || i == 76 || i == 77 || i == 78 || i == 79 || i == 80 || i == 83)
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

    public void ClickToMove(int RegionNum)
    {
        if (PhotonNetwork.IsConnected) photonView.RPC("ClickToMoveRPC", RpcTarget.All, RegionNum);
        else ClickToMoveRPC(RegionNum);
    }

    [PunRPC]
    public void ClickToMoveRPC(int RegionNum)
    {
        Debug.Log("clicked");
        Debug.Log(" clicked is " + RegionNum);
        Debug.Log(GM);

        Hero selfHero = GM.GetSelfHero();
        Hero currHero = GM.GetCurrentTurnHero();

        //add to path
        //get path index
        int index = 0;
        while (true)
        {
            if (currHero.path[index] == -1)
            {
                currHero.path[index] = RegionNum;
                Debug.Log("hero path index is " + index + "wp number is " + currHero.path[index]);

                int i = currHero.path[index];
                int j;
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
                string WaypointName = "";
                WaypointName = "Waypoint (" + Location[j]+ ")";
                Debug.Log("wp name is " + WaypointName);
                Debug.Log("index is " + index);
                Debug.Log(index);


                Debug.Log("pathbutton size is " + PathButton.Count);
                Debug.Log("check 0 pathbutton index is " + PathButton[0]);
                Debug.Log("pathbutton index is " + PathButton[index]);
                Waypoint waypoint = GameObject.Find(WaypointName).GetComponent<Waypoint>();
                PathButton[index].transform.position = waypoint.transform.position;
                Visibility(PathButton[index], true);
                Debug.Log("Before hiding wp buttons");
                HideWPButtons();
                Debug.Log("After hiding wp buttons");

                // Deduct the hero's time (this will automatically update the time track)
                currHero.AdvanceTimeMarker(1);

                // Kill farmers if this space contains a creature
                if (WaypointManager.GetWaypoint(RegionNum).GetCreature() != null) currHero.DestroyCarriedFarmers();

                // Show the end move button (to the moving hero only) to allow the hero to end their move
                if (selfHero == currHero) EndMoveButton.SetActive(true);

                // Display adjacent waypoints to allow the hero to continue their move
                ContinueMove();
                break;
            }
            else
            {
                index++;
            }
        }
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

    public void HideWPButtons()
    {
        for (int i = 0; i < 74; i++)
        {
            WPButton[i].SetActive(false); //hide
        }
    }

    public void ExecuteMove()
    {
        if (PhotonNetwork.IsConnected) photonView.RPC("ExecuteMoveRPC", RpcTarget.All);
        else ExecuteMoveRPC();
    }

    [PunRPC]
    public void ExecuteMoveRPC()
    {
        EndMoveButton.SetActive(false);

        GM.GetCurrentTurnHero().ExecuteMove();

        //make invisible pathbutton
        for (int i = 0; i < 10; i++)
        {
            PathButton[i].SetActive(false);
        }

        // Hide the waypoint buttons
        HideWPButtons();

        HeroManager.SetHeroIsMoving(false);

        // Advance to the next hero's turn
        GM.GoToNextHeroTurn();
    }

    public void ContinueMove()
    {
        Hero SelfHero = GM.GetSelfHero();
        Hero TurnHero = GM.GetCurrentTurnHero();

        //show adj wp to most recent tile path selected
        int mostRecentTileNum = -1;

        for (int i =9 ; i >= 0; i--)
        {
            if (TurnHero.path[i] != -1)
            {
                mostRecentTileNum = TurnHero.path[i];
                break;
            }
        }

        // If this is the moving hero's machine and the hero still has time to move, show the adjacent waypoints
        if (SelfHero == TurnHero && SelfHero.CanAdvanceTimeMarker(1)) WaypointManager.GetWaypoint(mostRecentTileNum).ShowAdjWP();
    }

}
