using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DivideBattleResources : MonoBehaviourPun
{
    // TODO: get re:
    // Start is called before the first frame update
    private GameManager GameManager;
    private int TotalGold;
    private int TotalWP;

    private int GoldRemaining;
    private int WPRemaining;

    private Dictionary<HeroType, int> Gold = new Dictionary<HeroType, int>();
    private Dictionary<HeroType, int> WP = new Dictionary<HeroType, int>();

    private HeroType[] Hero;

    private TMPro.TextMeshProUGUI[] GoldAmount;
    private TMPro.TextMeshProUGUI[] WPAmount;
    private TMPro.TextMeshProUGUI TotalWPAmount;
    private TMPro.TextMeshProUGUI TotalGoldAmount;
    private TMPro.TextMeshProUGUI Error;
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Gold[HeroType.Warrior] = 0;
        Gold[HeroType.Archer] = 0;
        Gold[HeroType.Wizard] = 0;
        Gold[HeroType.Dwarf] = 0;

        WP[HeroType.Dwarf] = 0;
        WP[HeroType.Wizard] = 0;
        WP[HeroType.Archer] = 0;
        WP[HeroType.Warrior] = 0;

        Hero = new HeroType[4];
        Hero[0] = HeroType.Warrior;
        Hero[1] = HeroType.Archer;
        Hero[2] = HeroType.Wizard;
        Hero[3] = HeroType.Dwarf;

        TotalGold = 15;
        TotalWP = 15;
        GoldRemaining = 0;
        WPRemaining = 0;

        // Get References to UI Elements
        TotalWPAmount = GameObject.Find("DivideWPTotal").GetComponent<TMPro.TextMeshProUGUI>();
        TotalGoldAmount = GameObject.Find("DivideGoldTotal").GetComponent<TMPro.TextMeshProUGUI>();
        Error = GameObject.Find("DBRError").GetComponent<TMPro.TextMeshProUGUI>();
        GoldAmount = new TMPro.TextMeshProUGUI[4];
        WPAmount = new TMPro.TextMeshProUGUI[4];

        string GoldName = "";
        string WPName = "";
        for(int i=0; i<4; i++)
        {
            GoldName = Hero[i].ToString() + "GoldAmount";
            WPName = Hero[i].ToString() + "WPAmount";
            GoldAmount[i] = GameObject.Find(GoldName).GetComponent<TMPro.TextMeshProUGUI>();
            WPAmount[i] = GameObject.Find(WPName).GetComponent<TMPro.TextMeshProUGUI>();
        }
    }

    // Called After a Battle
    public void DivideResources(int Gold, int WP)
    {

        TotalGold = Gold;
        TotalWP = WP;
        GoldRemaining = Gold;
        WPRemaining = WP;

        TotalGoldAmount.text = GoldRemaining.ToString()+ " G";
        TotalWPAmount.text = WPRemaining.ToString()+ " WP";

        Error.text = "";

        // Only appears on master client
        if(PhotonNetwork.IsConnected)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                Vector3 Origin = new Vector3(0,0,0);
                transform.Translate(Origin - transform.position);
            }
            // TODO: Show buffer screen on other screens
        }
        else
        {
            Vector3 Origin = new Vector3(0,0,0);
            transform.Translate(Origin - transform.position);
        }
    }

    public void HideDivideResourceMenu()
    {
        Vector3 Origin = new Vector3(200,0,0);
        transform.Translate(Origin - transform.position);
    }

    public void IncreaseGoldAmount(int i)
    {
        {
            int CurrentAmount = int.Parse(GoldAmount[i].text);
            if(GoldRemaining > 0)
            {
                Gold[Hero[i]] += 1;

                GoldRemaining -= 1;
                // Update UI
                GoldAmount[i].text = Gold[Hero[i]].ToString();
                TotalGoldAmount.text = GoldRemaining.ToString() + " G";
            }
        }

    }

    public void IncreaseWPAmount(int i)
    {
        {
            int CurrentAmount = int.Parse(WPAmount[i].text);
            if(WPRemaining > 0)
            {
                WP[Hero[i]] += 1;

                WPRemaining -= 1;
                // Update UI
                WPAmount[i].text = WP[Hero[i]].ToString();
                TotalWPAmount.text = WPRemaining.ToString() + " WP";
            }
        }

    }

    public void DecreaseWPAmount(int i)
    {
        
        if(WPRemaining < TotalWP)
        {
            int CurrentAmount = int.Parse(WPAmount[i].text);
            if(WP[Hero[i]] > 0)
            {
                WP[Hero[i]] -= 1;
                WPRemaining += 1;
                // Update UI
                WPAmount[i].text = WP[Hero[i]].ToString();

                TotalWPAmount.text = WPRemaining.ToString() + " G";
            }
        }
    }

    public void DecreaseGoldAmount(int i)
    {
        
        if(GoldRemaining < TotalGold)
        {
            int CurrentAmount = int.Parse(GoldAmount[i].text);
            if(Gold[Hero[i]] > 0)
            {
                Gold[Hero[i]] -= 1;
                GoldRemaining += 1;
                // Update UI
                GoldAmount[i].text = Gold[Hero[i]].ToString();

                TotalGoldAmount.text = GoldRemaining.ToString() + " G";
            }
        }
    }

    public void AcceptDivision()
    {
        if(GoldRemaining != 0 || WPRemaining != 0)
        {
            Error.text = "Please divide all resources.";
        }
        else
        {
            Debug.Log("Resources Divided!");
            // Call to HeroManager() to divide resources
            HideDivideResourceMenu();
        }
    }


}
