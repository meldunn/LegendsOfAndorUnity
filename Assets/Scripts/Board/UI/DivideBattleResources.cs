using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DivideBattleResources : MonoBehaviourPun
{
    // Start is called before the first frame update
    private GameManager GameManager;
    private HeroManager HeroManager;

    private int TotalWinnings;
    private int WinningsRemaining;

    private Dictionary<HeroType, int> Gold = new Dictionary<HeroType, int>();
    private Dictionary<HeroType, int> WP = new Dictionary<HeroType, int>();

    private HeroType[] Hero;

    private TMPro.TextMeshProUGUI[] GoldAmount;
    private TMPro.TextMeshProUGUI[] WPAmount;
    private TMPro.TextMeshProUGUI TotalWinningsAmount;
    private TMPro.TextMeshProUGUI Error;

    // The hero splitting the loot
    HeroType DividerHeroType;

    // Sections to show or hide based on who is participating
    private GameObject DivideWarriorPanel;
    private GameObject DivideArcherPanel;
    private GameObject DivideDwarfPanel;
    private GameObject DivideWizardPanel;

    List<Hero> Participants;

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        HeroManager = GameObject.Find("HeroManager").GetComponent<HeroManager>();

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

        TotalWinnings = 0;
        WinningsRemaining = 0;

        // Get References to UI Elements
        DivideWarriorPanel = GameObject.Find("DivideWarriorPanel");
        DivideArcherPanel = GameObject.Find("DivideArcherPanel");
        DivideDwarfPanel = GameObject.Find("DivideDwarfPanel");
        DivideWizardPanel = GameObject.Find("DivideWizardPanel");

        TotalWinningsAmount = GameObject.Find("DivideWinningsTotal").GetComponent<TMPro.TextMeshProUGUI>();
        Error = GameObject.Find("DBRError").GetComponent<TMPro.TextMeshProUGUI>();
        GoldAmount = new TMPro.TextMeshProUGUI[4];
        WPAmount = new TMPro.TextMeshProUGUI[4];

        string GoldName = "";
        string WPName = "";
        for (int i = 0; i < 4; i++)
        {
            GoldName = Hero[i].ToString() + "GoldAmount";
            WPName = Hero[i].ToString() + "WPAmount";
            GoldAmount[i] = GameObject.Find(GoldName).GetComponent<TMPro.TextMeshProUGUI>();
            WPAmount[i] = GameObject.Find(WPName).GetComponent<TMPro.TextMeshProUGUI>();
        }
    }

    // Called After a Battle
    public void DivideResources(HeroType DividerHeroType, int Winnings, List<Hero> Participants)
    {
        TotalWinnings = Winnings;
        WinningsRemaining = Winnings;
        this.Participants = Participants;
        this.DividerHeroType = DividerHeroType;

        UpdateRemainingText();

        Error.text = "";

        // Is visible only on the client owned by the divider
        if (PhotonNetwork.IsConnected)
        {
            if (GameManager.GetSelfHero().GetHeroType() == DividerHeroType)
            {
                Vector3 Origin = new Vector3(0, 0, 0);
                transform.Translate(Origin - transform.position);

                UpdateParticipants();
            }
            // TODO: Show buffer screen on other screens
        }
        else
        {
            Vector3 Origin = new Vector3(0, 0, 0);
            transform.Translate(Origin - transform.position);

            UpdateParticipants();
        }
    }

    public void HideDivideResourceMenu()
    {
        Vector3 Origin = new Vector3(200, 0, 0);
        transform.Translate(Origin - transform.position);
    }

    public void IncreaseGoldAmount(int i)
    {
        if (PhotonNetwork.IsConnected) photonView.RPC("IncreaseGoldAmountRPC", RpcTarget.All, i);
        else IncreaseGoldAmountRPC(i);
    }

    [PunRPC]
    public void IncreaseGoldAmountRPC(int i)
    {
        {
            int CurrentAmount = int.Parse(GoldAmount[i].text);
            if (WinningsRemaining > 0)
            {
                Gold[Hero[i]] += 1;

                WinningsRemaining -= 1;
                // Update UI
                GoldAmount[i].text = Gold[Hero[i]].ToString();
                UpdateRemainingText();
            }
        }

    }

    public void IncreaseWPAmount(int i)
    {
        if (PhotonNetwork.IsConnected) photonView.RPC("IncreaseWPAmountRPC", RpcTarget.All, i);
        else IncreaseWPAmountRPC(i);
    }

    [PunRPC]
    public void IncreaseWPAmountRPC(int i)
    {
        {
            int CurrentAmount = int.Parse(WPAmount[i].text);
            if (WinningsRemaining > 0)
            {
                WP[Hero[i]] += 1;

                WinningsRemaining -= 1;
                // Update UI
                WPAmount[i].text = WP[Hero[i]].ToString();
                UpdateRemainingText();
            }
        }

    }

    public void DecreaseWPAmount(int i)
    {
        if (PhotonNetwork.IsConnected) photonView.RPC("DecreaseWPAmountRPC", RpcTarget.All, i);
        else DecreaseWPAmountRPC(i);
    }

    [PunRPC]
    public void DecreaseWPAmountRPC(int i)
    {

        if (WinningsRemaining < TotalWinnings)
        {
            int CurrentAmount = int.Parse(WPAmount[i].text);
            if (WP[Hero[i]] > 0)
            {
                WP[Hero[i]] -= 1;
                WinningsRemaining += 1;
                // Update UI
                WPAmount[i].text = WP[Hero[i]].ToString();
                UpdateRemainingText();
            }
        }
    }

    public void DecreaseGoldAmount(int i)
    {
        if (PhotonNetwork.IsConnected) photonView.RPC("DecreaseGoldAmountRPC", RpcTarget.All, i);
        else DecreaseGoldAmountRPC(i);
    }

    [PunRPC]
    public void DecreaseGoldAmountRPC(int i)
    {

        if (WinningsRemaining < TotalWinnings)
        {
            int CurrentAmount = int.Parse(GoldAmount[i].text);
            if (Gold[Hero[i]] > 0)
            {
                Gold[Hero[i]] -= 1;
                WinningsRemaining += 1;
                // Update UI
                GoldAmount[i].text = Gold[Hero[i]].ToString();
                UpdateRemainingText();
            }
        }
    }

    public void AcceptDivision() 
    {
        if (PhotonNetwork.IsConnected) photonView.RPC("AcceptDivisionRPC", RpcTarget.All);
        else AcceptDivisionRPC();
    }

    [PunRPC]
    public void AcceptDivisionRPC()
    {
        if(WinningsRemaining != 0 && Participants.Count != 0)
        {
            Error.text = "Please divide all resources.";
        }
        else
        {
            // Award the winnings
            for (int i = 0; i < Hero.Length; i++)
            {
                int GoldAmount = Gold[Hero[i]];
                int WillpowerAmount = WP[Hero[i]];

                if (GoldAmount > 0) HeroManager.GetHero(Hero[i]).ReceiveGold(GoldAmount);
                if (WillpowerAmount > 0) HeroManager.GetHero(Hero[i]).IncreaseWillpower(WillpowerAmount);
            }

            Debug.Log("Resources Divided!");

            if (GameManager.GetSelfHero().GetHeroType() == DividerHeroType) HideDivideResourceMenu();
        }
    }

    public void UpdateRemainingText()
    {
        TotalWinningsAmount.text = WinningsRemaining.ToString() + " G or WP";
    }

    public void UpdateParticipants()
    {
        bool Warrior = Participants.IndexOf(HeroManager.GetHero(HeroType.Warrior)) != -1;
        bool Archer = Participants.IndexOf(HeroManager.GetHero(HeroType.Archer)) != -1;
        bool Dwarf = Participants.IndexOf(HeroManager.GetHero(HeroType.Dwarf)) != -1;
        bool Wizard = Participants.IndexOf(HeroManager.GetHero(HeroType.Wizard)) != -1;

        DivideWarriorPanel.SetActive(Warrior);
        DivideArcherPanel.SetActive(Archer);
        DivideDwarfPanel.SetActive(Dwarf);
        DivideWizardPanel.SetActive(Wizard);
    }
}
