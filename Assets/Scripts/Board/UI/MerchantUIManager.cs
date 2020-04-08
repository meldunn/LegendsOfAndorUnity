using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantUIManager : MonoBehaviour
{
    // Stored in numerical order { 18, 57, 71 }
    private GameObject MerchantMenu;

    private int[] Purchased = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
    // Amount of available tokens for each item
    private int[] MaxAmount = { 3, 5, 3, 5, 2, 2, 4 };
    
    private List<GameObject> Merchant = new List<GameObject>(3);
    private List<GameObject> MerchantButton = new List<GameObject>(3);
    private int[] Location = { 18, 57, 71 };


    private int CurrentUpdate = 0;

    private string[] ArticleNames = {"Helm", "Wineskin", "Bow", "WitchBrew", "Falcon", "Telescope", "Shield"};
    

    public void Initialize()
    {
        // Initialize Merchant Menu
        MerchantMenu = GameObject.Find("MerchantMenu");

        // Get References to Merchants

        string IconName = "";
        string ButtonName = "";
        for(int i=0; i<Location.Length; i++)
        {
            IconName = "merchant"+Location[i].ToString();
            ButtonName = "merchant"+Location[i].ToString() + "Button";

            if(GameObject.Find(IconName) == null) Debug.Log("No icon named "+ IconName);
            if(GameObject.Find(ButtonName) == null) Debug.Log("No button named "+ ButtonName);
            Merchant.Add(GameObject.Find(IconName));
            MerchantButton.Add(GameObject.Find(ButtonName));
        }

        // Merchant View Buttons
        for(int i=0; i<MerchantButton.Count; i++) 
            Visibility(MerchantButton[i], false);


        PlaceMerchants();
        // TODO Hide witch at the start

    }

    // Called once in Initialize() to place the merchants
    private void PlaceMerchants()
    {
        string IconName = "";
        string WaypointName = "";

        for(int i=0; i<Location.Length; i++)
        {
            WaypointName = "Waypoint ("+Location[i] + ")";
            
            Waypoint Waypoint = GameObject.Find(WaypointName).GetComponent<Waypoint>();

            Merchant[i].transform.Translate(Waypoint.GetLocation() 
                    - Merchant[i].transform.position);
        }

    }

    private void Visibility(GameObject GameObject, bool Show)
    {
        if(GameObject != null){
            if(Show) GameObject.SetActive(true);
            else GameObject.SetActive(false);
        }
        else Debug.Log("Error in MerchantUIManager. Referenced null");
    }
    public void UpdateMerchantButton(int RegionNumber)
    {
        // Reset all buttons
        for(int i=0; i<MerchantButton.Count; i++) 
            Visibility(MerchantButton[i], false);

        switch (RegionNumber)
        {
            case 18:
                Visibility(MerchantButton[0], true);
                break;
            case 57:
                Visibility(MerchantButton[1], true);
                break;
            case 71:
                Visibility(MerchantButton[2], true);
                break;
            default:
                break;
        }
    }

    public void ShowMerchantMenu(int MerchantNum)
    {
        // Displays the merchant menu at (0,0,0)
        Vector3 Location = new Vector3(0, 0, 0);
        MerchantMenu.transform.Translate(Location - MerchantMenu.transform.position);
        
        // TODO: check current player's gold, put it in total
    }
    
    // Updates current amount shown. Does NOT update the current amount purchased.
    public void IncreaseItemAmount(int Index)
    {

        TMPro.TextMeshProUGUI AmountText = GameObject.Find(ArticleNames[Index]+"Amount").GetComponent<TMPro.TextMeshProUGUI>();
        // Debug.Log(AmountText);

        int CurrentAmount = int.Parse(AmountText.text);

        if(CurrentAmount < MaxAmount[Index] ) CurrentAmount++;

        AmountText.text = CurrentAmount.ToString();
    }

    // Updates current amount shown. Does NOT update the current amount purchased.
    public void DecreaseItemAmount(int Index)
    {

        TMPro.TextMeshProUGUI AmountText = GameObject.Find(ArticleNames[Index]+"Amount").GetComponent<TMPro.TextMeshProUGUI>();

        int CurrentAmount = int.Parse(AmountText.text);

        if(CurrentAmount > 0 ) CurrentAmount--;

        AmountText.text = CurrentAmount.ToString();
    }

    private bool HasEnoughGold()
    {
        return true;
    }

}
