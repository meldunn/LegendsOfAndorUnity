using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RuneStoneMenu : MonoBehaviourPun
{
    WaypointManager WaypointManager;
    // list of images
    private string[] IconName;
    private GameObject DiceTen;
    private GameObject DiceOne;
    private GameObject TextTen;
    private GameObject TextOne;
    private GameObject PlacementText;
    private GameObject Menu;

    private bool TensRolled;
    private bool OnesRolled;

    private int RuneStonePosition;
    private int NumRuneStonesPlaced;

    // private bool isCoroutineExecuting = false;

    public void Initialize()
    {
        RuneStonePosition = 0;
        NumRuneStonesPlaced = 0;

        IconName = new string[5];

        for(int i=1; i<=5; i++)
        {
            IconName[i-1] = "HiddenRuneStone"+ i.ToString();
        }

        DiceTen = GameObject.Find("RuneTensDice");
        DiceOne = GameObject.Find("RuneOnesDice");
        TextTen = GameObject.Find("RuneTensText");
        TextOne = GameObject.Find("RuneOnesText");
        PlacementText = GameObject.Find("RuneStonePlacementText");
        
        // Hide placement text to start
        toggleGameObjectVisibility(PlacementText);

        TensRolled = false;
        OnesRolled = false;

        WaypointManager = GameObject.Find("WaypointManager").GetComponent<WaypointManager>();

        Menu = GameObject.Find("RuneStoneMenu");
    }

    public void FinishedRoll(bool OnesDice, int Value)
    {
        if(OnesDice)
        {
            if(TextOne.GetComponent<TMPro.TextMeshProUGUI>() != null)
            {
                TextOne.GetComponent<TMPro.TextMeshProUGUI>().text = Value.ToString();
            }
            RuneStonePosition += Value;
            toggleGameObjectVisibility(DiceOne);
            OnesRolled = true;
        }
        else
        {
            if(TextTen.GetComponent<TMPro.TextMeshProUGUI>() != null)
            {
                TextTen.GetComponent<TMPro.TextMeshProUGUI>().text = Value.ToString();
            }
            RuneStonePosition += 10*Value;
            toggleGameObjectVisibility(DiceTen);
            TensRolled = true;
        }

        if(OnesRolled && TensRolled)
        {
            Invoke("FinishedBothRolls", 2);
        }
    }

    public void FinishedBothRolls()
    {


         if(NumRuneStonesPlaced == 0)
         {
             toggleGameObjectVisibility(PlacementText);
         }
         UpdatePlacementText(RuneStonePosition);

         if(PhotonNetwork.IsConnected)
         {
             photonView.RPC("PlaceRuneStoneRPC", RpcTarget.All, RuneStonePosition, NumRuneStonesPlaced);
         }
         else
         {
             // Place a rune stone
             PlaceRuneStoneRPC(RuneStonePosition, NumRuneStonesPlaced);
         }


         // Reset the Dice
         RuneStonePosition = 0;
         NumRuneStonesPlaced += 1;
         toggleGameObjectVisibility(DiceTen);
         toggleGameObjectVisibility(DiceOne);
         OnesRolled = false;
         TensRolled = false;

         if(NumRuneStonesPlaced == 5)
         {
            Invoke("AllRuneStonesPlaced", 1);
         }
    }

    [PunRPC]
    private void InitializeRuneStone(int i)
    {
    }
    public void AllRuneStonesPlaced()
    {
        Vector3 Origin = new Vector3(200,0,0);

        // Debug.Log(Menu);
        Menu.transform.Translate(Origin -
                Menu.transform.position 
                );
        Debug.Log("Rune Stones Moved");
    }

    public void ShowRuneStoneMenu()
    {
        Vector3 Origin = new Vector3(0,0,0);

        Debug.Log(Menu);
        Menu.transform.Translate(Origin -
                Menu.transform.position 
                );
    }

    private void UpdatePlacementText(int RegionNum)
    {
        PlacementText.GetComponent<TMPro.TextMeshProUGUI>().text = "Rune stone placed at region " + RegionNum.ToString();
    }

    private void toggleGameObjectVisibility(GameObject GameObject)
    {
        if(GameObject != null)
        {
            bool isActive = GameObject.activeSelf;
            GameObject.SetActive(!isActive);
        }
        else
        {
            Debug.Log("Error. GameObject referenced null");
        }
    }
    
    [PunRPC]
    private void PlaceRuneStoneRPC(int RegionNum, int RuneStoneNum)
    {
        // Remove rune stone image and show its new location
        GameObject RuneStoneImage = GameObject.Find(IconName[RuneStoneNum]);
        toggleGameObjectVisibility(RuneStoneImage);

        RuneStoneNum++;
        string RuneStoneName = "runestone" + RuneStoneNum.ToString();
        Debug.Log(RuneStoneName);
        // GameObject RuneStoneImage2 = GameObject.Find(RuneStoneName);
        
        // Debug.Log("Place Rune Stone at Waypoint "+ RuneStoneName);
        Waypoint Waypoint = WaypointManager.GetWaypoint(RegionNum);

        // RuneStoneImage2.transform.Translate(Waypoint.GetLocation() - RuneStoneImage2.transform.position);
        Waypoint.InitializeRuneStone(RuneStoneNum);
    }
}
