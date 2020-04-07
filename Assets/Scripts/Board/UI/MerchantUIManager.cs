using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantUIManager : MonoBehaviour
{
    // Stored in numerical order { 18, 57, 71 }
    
    private List<GameObject> Merchant = new List<GameObject>(3);
    private List<GameObject> MerchantButton = new List<GameObject>(3);
    int[] Location = { 18, 57, 71 };

    public void Initialize()
    {
        // Initialize data
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

        PlaceMerchants();
        for(int i=0; i<MerchantButton.Count; i++) 
            Visibility(MerchantButton[i], false);
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
                Debug.Log("Error in UpdateMerchant. No merchant on region "
                        + RegionNumber);
                break;
        }
    }
}
