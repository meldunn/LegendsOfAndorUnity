using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneStoneMenu : MonoBehaviour
{
    // list of images
    private string[] IconName;
    private GameObject DiceTen;
    private GameObject DiceOne;
    private GameObject TextTen;
    private GameObject TextOne;
    private GameObject PlacementText;

    private bool TensRolled;
    private bool OnesRolled;

    private int RuneStonePosition;
    private int NumRuneStonesPlaced;

    private bool isCoroutineExecuting = false;

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
        
        //Debug.Log("Rune Stone Menu Initialized");
        toggleGameObjectVisibility(PlacementText);

        TensRolled = false;
        OnesRolled = false;
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
        // Animate the hidden stone image
        // Text: Rune Stone Placed on __
        // Add Rune Stone to the Waypoin
        // Make the dice visible again
         if(NumRuneStonesPlaced == 5)
         {
             Invoke("AllRuneStonesPlaced", 4);
         }
         else
         {

             // Remove rune stone image and show its new location
             GameObject RuneStoneImage = GameObject.Find(IconName[NumRuneStonesPlaced]);
             toggleGameObjectVisibility(RuneStoneImage);

             if(NumRuneStonesPlaced == 0)
             {
                 toggleGameObjectVisibility(PlacementText);
             }
             UpdatePlacementText(RuneStonePosition);

             // Place a rune stone
             Debug.Log("Place Rune Stone at Waypoint "+RuneStonePosition);
             // GetWayPoint(RollNum);
             // PlaceRuneStone(Waypoint);

             // Reset the Dice
             RuneStonePosition = 0;
             NumRuneStonesPlaced += 1;
             toggleGameObjectVisibility(DiceTen);
             toggleGameObjectVisibility(DiceOne);
             OnesRolled = false;
             TensRolled = false;

         }
        
    }
     public void AllRuneStonesPlaced()
     {
         GameObject RuneStoneMenu = GameObject.Find("RuneStoneMenu");
         toggleGameObjectVisibility(RuneStoneMenu);
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
}
