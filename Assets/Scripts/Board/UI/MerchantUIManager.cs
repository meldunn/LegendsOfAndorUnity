using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MerchantUIManager : MonoBehaviourPun
{
    // Stored in numerical order { 18, 57, 71 }
    private GameObject MerchantMenu;
    private GameManager GameManager;
    private HeroManager HeroManager;
    private GameObject GoldError;

    private Hero MyHero;

    // Total amount in a client's "cart". Updated each time merchant menu is opened
    private int[] Purchased = {0, 0, 0, 0, 0, 0, 0};
    // Amount of available tokens for each item in the entire game.
    private int[] MaxAmount = { 3, 5, 3, 100, 2, 2, 4 };
    
    private List<GameObject> Merchant = new List<GameObject>(3);
    private List<GameObject> MerchantButton = new List<GameObject>(3);
    private int[] Location = { 18, 57, 71 };

    private int CostOfPurchase = 0;
    private int CurrentUpdate = 0;

    private bool DwarfBeastMode = false;

    private PhotonView PV;

    // Wineskin, Shield, Falcon, Bow, Helm, Telescope, WitchBrew (only from witch), strenght points
    private string[] ArticleNames = {"Helm", "Wineskin", "Bow", "StrengthPoints", "Falcon", "Telescope", "Shield"};
    private ItemType[] Items = 
    {
        ItemType.Helm,
        ItemType.Wineskin,
        ItemType.Bow,
        ItemType.StrengthPoints,
        ItemType.Falcon,
        ItemType.Telescope,
        ItemType.Shield,
    };

    public void Initialize()
    {

        // Initialize Merchant Menu
        MerchantMenu = GameObject.Find("MerchantMenu");
        // Debug.Log("MerchantMenu");
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        HeroManager = GameObject.Find("HeroManager").GetComponent<HeroManager>();

        GoldError = GameObject.Find("NotEnough");
        GoldError.SetActive(false);

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


        // PlaceMerchants();
        // TODO Hide witch at the start
        
        PV = GetComponent<PhotonView>();
    }


    private void Visibility(GameObject GameObject, bool Show)
    {
        if(GameObject != null){
            if(Show) GameObject.SetActive(true);
            else GameObject.SetActive(false);
        }
        else Debug.Log("Error in MerchantUIManager. Referenced null");
    }

    // Called when a hero moves, the UI updates depending on which region the hero lands on
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

    // Called when the "show merchants" button is clicked. This button is only shown when a hero is on a region with a merchant.
    public void ShowMerchantMenu(int MerchantNum)
    {

        MyHero = GameManager.GetSelfHero();

        Vector3 Location = new Vector3(0, 0, 0);
        MerchantMenu.transform.Translate(Location - MerchantMenu.transform.position);

        TMPro.TextMeshProUGUI MyGoldText = GameObject.Find("YourGold").GetComponent<TMPro.TextMeshProUGUI>();
        TMPro.TextMeshProUGUI StrengthText = GameObject.Find("StrengthText").GetComponent<TMPro.TextMeshProUGUI>();

        if(MerchantNum == 71 && GameManager.GetSelfHero().GetHeroType() == HeroType.Dwarf)
        {
            StrengthText.text = "Strength (1g)";
            DwarfBeastMode = true;
        }
        else
        {
            StrengthText.text = "Strength (2g)";
            DwarfBeastMode = false;
        }
        
        // Get the gold of the player who opens the merchant menu
        int PlayerGold = MyHero.getGold();

        // Debug.Log("Player has "+PlayerGold);
        MyGoldText.text = "Your Gold: "+PlayerGold+"g";
    }
    
    // Updates current amount shown. 
    public void IncreaseItemAmount(int Index)
    {

        TMPro.TextMeshProUGUI AmountText = GameObject.Find(ArticleNames[Index]+"Amount").GetComponent<TMPro.TextMeshProUGUI>();
        // Debug.Log(AmountText);

        TMPro.TextMeshProUGUI CostText = GameObject.Find("Cost").GetComponent<TMPro.TextMeshProUGUI>();

        int CurrentAmount = int.Parse(AmountText.text);
        int CurrentCost = int.Parse(CostText.text);

        if(CurrentAmount < MaxAmount[Index] && MyHero.CanCarry(Items[Index]))
        {
            CurrentAmount++;
            Purchased[Index] += 1;

            if(DwarfBeastMode && Index == 3) CurrentCost += 1;
            else CurrentCost += 2;
        }

        AmountText.text = CurrentAmount.ToString();
        CostText.text = CurrentCost.ToString();

        CostOfPurchase = CurrentCost;
    }

    // Updates current amount shown. 
    public void DecreaseItemAmount(int Index)
    {

        TMPro.TextMeshProUGUI AmountText = GameObject.Find(ArticleNames[Index]+"Amount").GetComponent<TMPro.TextMeshProUGUI>();
        // Debug.Log(AmountText.text);

        TMPro.TextMeshProUGUI CostText = GameObject.Find("Cost").GetComponent<TMPro.TextMeshProUGUI>();

        int CurrentAmount = int.Parse(AmountText.text);
        int CurrentCost = int.Parse(CostText.text);

        if(CurrentAmount > 0 )
        {
            CurrentAmount--;

            Purchased[Index] -= 1;

            if(DwarfBeastMode && Index == 3) CurrentCost -= 1;
            else CurrentCost -= 2;

        }

        AmountText.text = CurrentAmount.ToString();
        CostText.text = CurrentCost.ToString();

        CostOfPurchase = CurrentCost;
    }

    public void CloseMerchantMenu()
    {
        // Hide the menu
        Vector3 Location = new Vector3(200, 0, 0);
        MerchantMenu.transform.Translate(Location - MerchantMenu.transform.position);

        // Reset purchased to 0.
        for(int i=0; i<Purchased.Length; i++) Purchased[i] = 0;

        // MerchantManager.Purchase(<Item, Amount>)
        GoldError.SetActive(false);
    }

    public void RequestPurchase()
    {
        int PlayerGold = MyHero.getGold();

        bool Error = false;
        // Called when Player has enough gold, and purchases the items
        // Debug.Log(PlayerGold);
        if(CostOfPurchase <= PlayerGold)
        {
            // Confirm the request does not exceed the total
            for(int i=0; i<Purchased.Length; i++)
            {
                if(Purchased[i] > MaxAmount[i])
                {
                    Error = true;
                }
            }
            if( !Error ){
                ConfirmPurchase();
                CloseMerchantMenu();
            }
            // TODO: display error message if someone bought the item before you did
        }
        else
        {
            GoldError.SetActive(true);
        }
    }

    private void ConfirmPurchase()
    {
        HeroManager.DecreaseGold(CostOfPurchase);
        for(int i=0; i<Purchased.Length; i++)
        {
            if(Purchased[i] > 0)
            {
                HeroManager.BuyFromMerchant(Items[i], Purchased[i]);
            }
        }
        if(PhotonNetwork.IsConnected)
            photonView.RPC("ConfirmPurchaseRPC", RpcTarget.All, MyHero.GetHeroType(), Purchased);
        else ConfirmPurchaseRPC(MyHero.GetHeroType(), Purchased);
    }

    [PunRPC]
    public void ConfirmPurchaseRPC(HeroType TargetHeroType, int[] BoughtItems)
    {
        for(int i=0; i<BoughtItems.Length; i++)
        {
            // Subtract from total available
            MaxAmount[i] -= BoughtItems[i];
        }
    }

}
