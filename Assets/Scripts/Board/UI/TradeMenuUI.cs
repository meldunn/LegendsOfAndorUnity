using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TradeMenuUI : MonoBehaviourPun
{
    // References to stuff
    // MyHero Reference
    // List of Other Heroes
    // array of HeroType
    private ItemType[] Type;
    private List<HeroType> Hero;
    private GameManager GameManager;

    private Hero MyHero;

    private HeroType MyHeroType;
    private int CurrentRecipientID;
    private int CurrentTradePartnerID;

    private int MyGold;
    private int TheirGold;

    private PhotonView PV;

    private Dictionary<ItemType, int> MyInventory;
    private Dictionary<ItemType, int> TheirInventory;
    private Dictionary<ItemType, int> TradedItems;

    private int[] TradeOffer;

    // References to all gui parts
    private GameObject TradePopup;
    private GameObject ConfirmationPopup;
    private GameObject TheirPanel;

    private TMPro.TextMeshProUGUI Confirmation;
    private TMPro.TextMeshProUGUI Their;
    private TMPro.TextMeshProUGUI YouGive;
    private TMPro.TextMeshProUGUI YouReceive;
    private TMPro.TextMeshProUGUI[] MyAmount;
    private TMPro.TextMeshProUGUI[] TheirAmount;

    private Vector3 PopupPos;

    public void Initialize()
    {
        // Get References to current players
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        TheirPanel = GameObject.Find("TheirItemsPanel");
        // Debug.Log(GameManager);
        TradePopup = GameObject.Find("IncomingTradePopup");
        ConfirmationPopup = GameObject.Find("TradeConfirmationPopup");

        MyInventory = GameManager.GetSelfHero().GetInventory();
        
        // TODO: Switch to other player
        TheirInventory = GameManager.GetSelfHero().GetInventory();

        MyHero = GameManager.GetSelfHero();

        TradeOffer = new int[11];

        // TODO: Get references only to heroes that are playing
        Hero = new List<HeroType>(4);
        Hero.Add(HeroType.Warrior);
        Hero.Add(HeroType.Archer);
        Hero.Add(HeroType.Dwarf);
        Hero.Add(HeroType.Wizard);

        // Gets references to all game objects
        Type = new ItemType[11];
        Type[0] = ItemType.Helm;
        Type[1] = ItemType.Wineskin;
        Type[2] = ItemType.Gold;
        Type[3] = ItemType.Telescope;
        Type[4] = ItemType.Falcon;
        Type[5] = ItemType.MedicinalHerb;
        Type[6] = ItemType.Witchbrew;
        Type[7] = ItemType.Shield;

        // UI References
        MyAmount = new TMPro.TextMeshProUGUI[11];
        TheirAmount = new TMPro.TextMeshProUGUI[11];
        for(int i=0; i<8; i++)
        {
            MyAmount[i] = GameObject.Find("My"+Type[i].ToString()+"Amount").GetComponent<TMPro.TextMeshProUGUI>();
            TheirAmount[i] = GameObject.Find("Their"+Type[i].ToString()+"Amount").GetComponent<TMPro.TextMeshProUGUI>();
        }

        Confirmation = GameObject.Find("TradeConfirmation").GetComponent<TMPro.TextMeshProUGUI>();

        Their = GameObject.Find("TheirNameText").GetComponent<TMPro.TextMeshProUGUI>();
        YouGive = GameObject.Find("YouGive").GetComponent<TMPro.TextMeshProUGUI>();
        YouReceive = GameObject.Find("YouReceive").GetComponent<TMPro.TextMeshProUGUI>();

        // Initialize Empty Trade
        TradedItems = new Dictionary<ItemType, int>();

        TradedItems[ItemType.Helm] = 0;
        TradedItems[ItemType.Wineskin] = 0;
        TradedItems[ItemType.Bow] = 0;
        TradedItems[ItemType.Telescope] = 0;
        TradedItems[ItemType.Falcon] = 0;
        TradedItems[ItemType.MedicinalHerb] = 0;
        TradedItems[ItemType.Witchbrew] = 0;
        TradedItems[ItemType.Shield] = 0;

        PopupPos = new Vector3(10, 20, 0);
        PV = GetComponent<PhotonView>();


        // Hide Popups by default;
        ConfirmationPopup.SetActive(false);
        TradePopup.SetActive(false);
    }

    public void DisplayTradeMenu()
    {
        // Refresh Inventories
        MyHero = GameManager.GetSelfHero();
        TheirPanel.SetActive(false);

        MyInventory = GameManager.GetSelfHero().GetInventory();
        MyGold = MyHero.GetGold();
        Reset(TradedItems);

        for(int i=0; i<4; i++)
        {
            if(Hero[i] == MyHero.GetHeroType() || !GameManager.IsPlaying(Hero[i]))
            {
                GameObject.Find(Hero[i].ToString()+"Trade").SetActive(false);
            }
        }

        Vector3 Origin = new Vector3(0,0,0);
        transform.Translate(Origin - transform.position);
    }

    public void HideTradeMenu()
    {
        Vector3 Displacement = new Vector3(200,100,0);
        transform.Translate(Displacement - transform.position);

        for(int i=0; i<8; i++)
        {
            MyAmount[i].text = "0";
            TheirAmount[i].text = "0";

            TradeOffer[i] = 0;
        }
        Reset(TradedItems);
        Reset(TheirInventory);
    }

    /*
     * Displaying Item Amounts
     */

    public void IncreaseMyItemAmount(int i)
    {
        int CurrentAmount = int.Parse(MyAmount[i].text);

        if(i == 2 && CurrentAmount < MyGold)
        {
            CurrentAmount++;
            TradedItems[Type[i]] ++;
        }
        else if(i!=2 && CurrentAmount < MyInventory[Type[i]])
        {
            CurrentAmount ++;
            TradedItems[Type[i]] ++;
        }

        MyAmount[i].text = CurrentAmount.ToString();
    }

    public void DecreaseMyItemAmount(int i)
    {
        int CurrentAmount = int.Parse(MyAmount[i].text);

        if(CurrentAmount > 0)
        {
            CurrentAmount --;
            TradedItems[Type[i]] --;
        }
        MyAmount[i].text = CurrentAmount.ToString();
    }
    
    public void IncreaseTheirItemAmount(int i)
    {
        int CurrentAmount = int.Parse(TheirAmount[i].text);

        if(i == 2 && CurrentAmount < TheirGold)
        {
            CurrentAmount ++;
            TradedItems[Type[i]] --;
        }
        else if(i!=2 &&CurrentAmount < TheirInventory[Type[i]])
        {
            CurrentAmount ++;
            TradedItems[Type[i]] --;
        }

        TheirAmount[i].text = CurrentAmount.ToString();
    }

    // Updates current amount shown. 
    public void DecreaseTheirItemAmount(int i)
    {
        int CurrentAmount = int.Parse(TheirAmount[i].text);

        if(CurrentAmount > 0)
        {
            CurrentAmount --;
            TradedItems[Type[i]] ++;
        }

        TheirAmount[i].text = CurrentAmount.ToString();
    }

    public void CancelTrade()
    {
        // Reset Dictionaries
        Reset(TheirInventory);
        Reset(TradedItems);
        HideTradeMenu();
    }

    /*
     * Sending a Trade
     */
    public void SendTrade()
    {
        Debug.Log("Trade between "+MyHeroType+" and "+ Hero[CurrentRecipientID]);

        int[] Traded = new int[Type.Length];

        // Prepare the trade items for the Recipient UI
        string Receive = "You Receive: ";
        string Give = "You Give: ";

        for(int i=0; i<8; i++)
        {
            if(TradedItems[Type[i]] < 0)
            {
                Give += (TradedItems[Type[i]]*-1).ToString() + " "+ Type[i] + ", ";

                Traded[i] = TradedItems[Type[i]];
            }
            else if(TradedItems[Type[i]] > 0)
            {
                Receive += TradedItems[Type[i]].ToString() + " "+ Type[i] + ", ";
                Traded[i] = TradedItems[Type[i]];
            }
            else Traded[i] = 0;
        }

        PV.RPC("SendTradeRPC", RpcTarget.Others, 
                Hero.IndexOf(MyHeroType), CurrentRecipientID, 
                Give, Receive, Traded);
        HideTradeMenu();
    }

    [PunRPC]
    public void SendTradeRPC(int SenderID, int ReceiverID, string Given, string Received, int[] TradeList)
    {
        // Find the person to trade with
        if(MyHeroType == Hero[ReceiverID])
        {
            CurrentTradePartnerID = SenderID;

            // Show Trade popup on other client
            YouGive.text = Given;
            YouReceive.text = Received;

            TradePopup.SetActive(true);

            // Load the trade offer onto the receiving player's client
            for(int j=0; j<TradeList.Length; j++)
            {
                TradeOffer[j] = TradeList[j];
                // Debug.Log(TradeOffer[j] + " "+ Type[j]);
            }
        }
    }

    // Called when another player accepts the current trade offer
    public void AcceptOffer()
    {
        Debug.Log("Trade Accepted with "+ Hero[CurrentTradePartnerID]);

        for(int i=0; i<8; i++)
        {
            if(TradeOffer[i] > 0)
            {
                for(int j=0; j<i; j++)
                {
                  MyHero.ReceiveItemFromTrade(Type[i]); 
                }
            }
            else if(TradeOffer[i] < 0)
            {
                for(int j=0; j>i; j--)
                {
                  MyHero.GiveItemFromTrade(Type[i]); 
                }

            }
        }

        PV.RPC("SendReplyRPC", RpcTarget.Others, CurrentTradePartnerID, true, TradeOffer);
        HideTradePopup();

    }


    public void RejectOffer()
    {
        Debug.Log("Trade Rejected.");

        PV.RPC("SendReplyRPC", RpcTarget.Others, CurrentTradePartnerID, false, TradeOffer);

        for(int i=0; i<TradeOffer.Length; i++)
        {
            TradeOffer[i] = 0;
        }
        HideTradePopup();
    }

    // Sent by the player who receives a trade offer.
    [PunRPC]
    public void SendReplyRPC(int OriginalSenderID, bool Accepted, int[] TradeList)
    {
        if(MyHeroType == Hero[OriginalSenderID])
        {
            if(Accepted)
            {
                Debug.Log("Trade Accepted");
                Confirmation.text = "Trade Accepted!";

                // For Items added I add to inventory
                for(int i=0; i<8; i++)
                {
                    if(TradeList[i] > 0)
                    {
                        for(int j=0; j<i; j++)
                        {
                          MyHero.GiveItemFromTrade(Type[i]); 
                        }
                    }
                    else if(TradeList[i] < 0)
                    {
                        for(int j=0; j>i; j--)
                        {
                          MyHero.ReceiveItemFromTrade(Type[i]); 
                        }

                    }
                }
            }
            else
            {
                Debug.Log("Trade Rejected");
                Confirmation.text = "Trade Rejected!";

                for(int i=0; i<8; i++)
                {
                    TradeOffer[i] = 0;
                }
            }
            ConfirmationPopup.SetActive(true);
            Invoke("HideConfirmation", 2);
        }
    }

    public void HideConfirmation()
    {
        ConfirmationPopup.SetActive(false);
    }
    
    public void HideTradePopup()
    {
        Vector3 Dump = new Vector3(200, 200, 0);
        TradePopup.transform.Translate(Dump - TradePopup.transform.position);
    }


    // Sets all values in the dictionary to 0
    private void Reset(Dictionary<ItemType, int> I)
    {
        for(int i=0; i<8; i++)
        {
            I[Type[i]] = 0;
        }
    }

    /*
     * Switching Trade Recipient
     */

    public void SwitchHeroToTrade(int ReceiverID)
    {
        int SenderID = Hero.IndexOf(MyHeroType);
        PV.RPC("DisplayTheirInventory", RpcTarget.All, 
                ReceiverID, SenderID);
        CurrentRecipientID = ReceiverID;
    }

    [PunRPC]
    public void DisplayTheirInventory(int RecID, int SenderID)
    {

        bool MatchFound = false;
        // Debug.Log("Requesting Hero Inventory");
        MyHeroType = GameManager.GetSelfHero().GetHeroType();
        
        // Debug.Log("My Hero Type: "+MyHeroType);
        Dictionary<ItemType, int> Buffer = new Dictionary<ItemType, int>();
        
        if(Hero[RecID] == MyHeroType)
        {
            Debug.Log("Successful match with"+Hero[RecID]);
            MatchFound = true;
            TheirInventory = GameManager.GetSelfHero().GetInventory();
            TheirGold = GameManager.GetSelfHero().GetGold();
        }

        if(Hero[SenderID] == MyHeroType)
        {
            Their.text = Hero[RecID] + " Items";
            //for(int k=0; k<8; k++)
            //{
            //    TheirAmount[k].text = TheirInventory[Type[k]].ToString();
            //}
            CurrentRecipientID = RecID;
            TheirPanel.SetActive(true);
        }
        
    }

}
