using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum DifficultyLevel { Easy, Normal };
public enum LoseReason { Castle };

public class GameManager : MonoBehaviourPun, Subject
{
    public static GameManager Instance;

    // Other managers
    private WaypointManager WaypointManager;
    private UIManager UIManager;
    private HeroManager HeroManager;
    private CreatureManager CreatureManager;
    private NarratorManager NarratorManager;

    // List of Observers (Observer design pattern)
    List<Observer> Observers = new List<Observer>();

    // Heroes playing status
    private bool WarriorIsPlaying;
    private bool ArcherIsPlaying;
    private bool DwarfIsPlaying;
    private bool WizardIsPlaying;

    // Game players
    private AndorPlayer MyPlayer;               // The player currently playing in this session (same value as one of the four player variables below)
    private AndorPlayer WarriorPlayer;
    private AndorPlayer ArcherPlayer;
    private AndorPlayer DwarfPlayer;
    private AndorPlayer WizardPlayer;

    // Turn management
    private HeroType CurrentTurnHero;           // The hero whose turn it is
    private HeroType[] TurnOrder;

    // Game mode
    //TONETWORK
    [HideInInspector]
    public DifficultyLevel Difficulty;

    // Initial board element locations
    private int[] InitialGorLocation = { 8, 20, 21, 26, 48 };
    private int InitialSkralLocation = 19;
    private int[] InitialEasyFarmerLocation = { 24, 36 };
    private int[] InitialNormalFarmerLocation = { 24 };
    private int[] MerchantLocation = { 18, 57, 71 };
    private int InitialWarrior = 14;
    private int InitialArcher = 25;
    private int InitialDwarf = 7;
    private int InitialWizard = 34;
    private static NarratorLetter curLetter = NarratorLetter.A;


    void Start()
    { 
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        
        Initialize();
    }



    // Start is called before the first frame update
    void Initialize()
    {
        // Initialize references to other managers
        WaypointManager = GameObject.Find("WaypointManager").GetComponent<WaypointManager>();
        UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        HeroManager = GameObject.Find("HeroManager").GetComponent<HeroManager>();
        CreatureManager = GameObject.Find("CreatureManager").GetComponent<CreatureManager>();
        NarratorManager = GameObject.Find("NarratorManager").GetComponent<NarratorManager>();

        // Initialize the non-UI managers
        WaypointManager.Initialize();
        HeroManager.Initialize();
        CreatureManager.Initialize();
        NarratorManager.Initialize();


        //TONETWORK
        // Initialize game difficulty
        //Difficulty = DifficultyLevel.Normal;    // TODO real value


        //TONETWORK
        // Initialize which heroes are playing
        WarriorIsPlaying = true;                // TODO real value
        ArcherIsPlaying = true;                 // TODO real value
        DwarfIsPlaying = true;                  // TODO real value
        WizardIsPlaying = true;                 // TODO real value

        // Initialize players and their corresponding heroes (including their board positions)
        if (WarriorIsPlaying)
        {
            WarriorPlayer = GameObject.Find("GameManager").AddComponent<AndorPlayer>();       // TODO real value
            WarriorPlayer.SetHero(HeroManager.GetHero(HeroType.Warrior));
            HeroManager.InitializeHero(HeroType.Warrior, InitialWarrior);
        }
        if (ArcherIsPlaying)
        {
            ArcherPlayer = GameObject.Find("GameManager").AddComponent<AndorPlayer>();       // TODO real value
            ArcherPlayer.SetHero(HeroManager.GetHero(HeroType.Archer));
            HeroManager.InitializeHero(HeroType.Archer, InitialArcher);
        }
        if (DwarfIsPlaying)
        {
            DwarfPlayer = GameObject.Find("GameManager").AddComponent<AndorPlayer>();       // TODO real value
            DwarfPlayer.SetHero(HeroManager.GetHero(HeroType.Dwarf));
            HeroManager.InitializeHero(HeroType.Dwarf, InitialDwarf);
        }
        if (WizardIsPlaying)
        {
            WizardPlayer = GameObject.Find("GameManager").AddComponent<AndorPlayer>();       // TODO real value
            WizardPlayer.SetHero(HeroManager.GetHero(HeroType.Wizard));
            HeroManager.InitializeHero(HeroType.Wizard, InitialWizard);
        }


        //TONETWORK
        // Initialize playing character
        MyPlayer = WarriorPlayer;              // TODO real value

        // NETWORKED
        // Initialize turns
        if (PhotonNetwork.IsConnected)
        {
            HeroType[] NewTurnOrder;

            // Generate the turn order on one player's machine
            if (photonView.IsMine)
            {
                Debug.Log("Machine " + GetSelfHero().GetHeroType() + " generated a turn order.");
                NewTurnOrder = GenerateTurnOrder();

                // Extract each hero's turn from the order (this is done because a HeroType[] can't be sent in an RPC)
                int WarriorTurn = Array.FindIndex(NewTurnOrder, e => e == HeroType.Warrior);
                int ArcherTurn = Array.FindIndex(NewTurnOrder, e => e == HeroType.Archer);
                int DwarfTurn = Array.FindIndex(NewTurnOrder, e => e == HeroType.Dwarf);
                int WizardTurn = Array.FindIndex(NewTurnOrder, e => e == HeroType.Wizard);

                // Send the order to the other players
                photonView.RPC("SetTurnOrderRPC", RpcTarget.All, WarriorTurn, ArcherTurn, DwarfTurn, WizardTurn);
            }
        }
        else
        {
            TurnOrder = GenerateTurnOrder();
            CurrentTurnHero = HeroType.Dwarf;       // Default when playing offline (4 players)
        }

        // Initialize the castle
        WaypointManager.GetCastle().Initialize(GetNumOfPlayers());

        // Generate initial gors
        for (int i = 0; i < InitialGorLocation.Length; i++)
        {
            CreatureManager.Spawn(CreatureType.Gor, InitialGorLocation[i]);
        }

        // Generate initial skrall
        CreatureManager.Spawn(CreatureType.Skral, InitialSkralLocation);

        // Generate initial farmers
        switch (Difficulty)
        {
            case DifficultyLevel.Easy:
                // TODO Use InitialEasyFarmerLocation to generate farmers
                break;
            case DifficultyLevel.Normal:
                // TODO Use InitialNormalFarmerLocation to generate farmers
                break;
            default:
                Debug.LogError("Cannot spawn farmers; invalid difficulty level");
                break;
        }

        // Generate Initial Merchants
        for(int i=0; i<MerchantLocation.Length; i++)
        {
            string WaypointName = "Waypoint (" + MerchantLocation[i] + ")";
            GameObject Waypoint = GameObject.Find(WaypointName);
            Waypoint.AddComponent<Merchant>();

            Merchant Merchant = Waypoint.GetComponent<Merchant>();
            Merchant.Initialize();
        }

        // Initialize the UI manager
        // IMPORTANT: Do this last to ensure all data is available
        UIManager.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Get a reference to the player controlling this game instance
    public AndorPlayer GetSelfPlayer()
    {
        return MyPlayer;
    }

    // Get a reference to the hero of the player controlling this game instance
    public Hero GetSelfHero()
    {
        if (MyPlayer == null) return null;
        else return MyPlayer.GetHero();
    }

    public AndorPlayer GetCurrentTurnPlayer()
    {
        if (CurrentTurnHero == HeroType.Warrior) return WarriorPlayer;
        else if (CurrentTurnHero == HeroType.Archer) return ArcherPlayer;
        else if (CurrentTurnHero == HeroType.Dwarf) return DwarfPlayer;
        else if (CurrentTurnHero == HeroType.Wizard) return WizardPlayer;
        else return null;
    }

    public Hero GetCurrentTurnHero()
    {
        if (CurrentTurnHero == null) return null;
        else return HeroManager.GetHero(CurrentTurnHero);
    }

    // Returns the number of players in this game
    public int GetNumOfPlayers()
    {
        int Count = 0;

        if (WarriorIsPlaying) Count++;
        if (ArcherIsPlaying) Count++;
        if (DwarfIsPlaying) Count++;
        if (WizardIsPlaying) Count++;

        return Count;
    }

    public void HeroMove()
    {
        GetCurrentTurnHero().Move();
    }

    // Returns a randomly generated turn order
    private HeroType[] GenerateTurnOrder()
    {
        int NumOfPlayers = GetNumOfPlayers();
        HeroType[] NewTurnOrder = new HeroType[NumOfPlayers];

        // Create a grab bag from which to draw
        List<HeroType> GrabBag = new List<HeroType>(4);
        if (WarriorIsPlaying) GrabBag.Add(HeroType.Warrior);
        if (ArcherIsPlaying) GrabBag.Add(HeroType.Archer);
        if (DwarfIsPlaying) GrabBag.Add(HeroType.Dwarf);
        if (WizardIsPlaying) GrabBag.Add(HeroType.Wizard);

        // Quick validation
        if (NumOfPlayers != GrabBag.Count)
        {
            Debug.LogError("Error: could not generate turn order; inconsistency in number of players");
            return null;
        }

        int RandomIndex;

        // While the grab bag isn't empty, draw a random player from it and add the player to the new order
        for (int i = 0; i < NumOfPlayers; i++)
        {
            RandomIndex = UnityEngine.Random.Range(0, GrabBag.Count);
            NewTurnOrder[i] = GrabBag[RandomIndex];
            GrabBag.RemoveAt(RandomIndex);
        }

        return NewTurnOrder;
    }

    // Gives the turn to the next hero in the turn order
    public void GoToNextHeroTurn()
    {
        int CurrentIndex = Array.FindIndex(TurnOrder, e => e == CurrentTurnHero);

        // Validation
        if (CurrentIndex == -1)
        {
            Debug.LogError("Could not find the hero whose turn it is in the turn order.");
            return;
        }

        CurrentTurnHero = TurnOrder[ (CurrentIndex + 1) % TurnOrder.Length ];

        // Notify observers to update UI
        Notify("TURN");
    }

    // Used only by the UI button that advances the turn. For all other purposes, call GoToNextHeroTurn() from within an RPC
    public void GoToNextHeroTurnForAll()
    {
        // NETWORKED
        // Use an RPC to advance the turn on all computers
        if (PhotonNetwork.IsConnected) photonView.RPC("AdvanceTurnRPC", RpcTarget.All);
        else AdvanceTurnRPC();
    }

    // Used to follow the turn order during a battle. Returns whose turn it is in the order after the specified Hero.
    public Hero GetTurnHeroAfter(Hero Hero)
    {
        for (int i = 0; i < TurnOrder.Length; i++)
        {
            if (TurnOrder[i] == Hero.GetHeroType()) return HeroManager.GetHero(TurnOrder[(i + 1) % TurnOrder.Length]);
        }
        return null;
    }

    // Changes control of the current game session to a different player.
    // Note: this is used to simulate local multiplayer (for testing and demonstration purposes) and will NOT be used in the real networked game
    public void SetSelfPlayer(HeroType NewControlledPlayersHero)
    {
        switch (NewControlledPlayersHero)
        {
            case HeroType.Warrior:
                MyPlayer = WarriorPlayer;
                break;

            case HeroType.Archer:
                MyPlayer = ArcherPlayer;
                break;

            case HeroType.Dwarf:
                MyPlayer = DwarfPlayer;
                break;

            case HeroType.Wizard:
                MyPlayer = WizardPlayer;
                break;

            default:
                Debug.LogError("Cannot set controlled player; invalid hero type: " + NewControlledPlayersHero);
                return;
        }

        // Notify observers to update UI
        Notify("CONTROL");
    }

    public void LoseGame(LoseReason Reason)
    {
        if (Reason == LoseReason.Castle)
        {
            Debug.LogWarning("The game has been lost because too many creatures entered the castle.");
        }
        else
        {
            Debug.LogWarning("The game has been lost.");
        }

        // TODO connect this notification to UI
        Notify("LOSE");
    }

    public DifficultyLevel GetDifficulty()
    {
        return Difficulty;
    }

    // Used in Observer design pattern
    public void Attach(Observer o)
    {
        Observers.Add(o);
    }

    // Used in Observer design pattern
    public void Detach(Observer o)
    {
        Observers.Remove(o);
    }

    // Used in Observer design pattern
    public void Notify(string Category)
    {
        // Iterate through a copy of the observer list in case observers detach themselves during notify
        var ObserversCopy = new List<Observer>(Observers);

        foreach (Observer o in ObserversCopy)
        {
            o.UpdateData(Category);
        }
    }

    public void pickupFarmer()
    {
        GetCurrentTurnHero().pickupFarmer();
    }

    public void dropFarmer()
    {
        GetCurrentTurnHero().dropFarmer();
    }

    public void pickupGold()
    {
        GetCurrentTurnHero().pickupGold();
    }

    public void dropGold()
    {
        GetCurrentTurnHero().dropGold();
    }

    // NETWORKED
    // Sets the turn order on all machines
    [PunRPC]
    public void SetTurnOrderRPC(int WarriorTurn, int ArcherTurn, int DwarfTurn, int WizardTurn)
    {
        int NumOfPlayers = GetNumOfPlayers();
        TurnOrder = new HeroType[NumOfPlayers];

        if (WarriorTurn != -1) TurnOrder[WarriorTurn] = HeroType.Warrior;
        if (ArcherTurn != -1) TurnOrder[ArcherTurn] = HeroType.Archer;
        if (DwarfTurn != -1) TurnOrder[DwarfTurn] = HeroType.Dwarf;
        if (WizardTurn != -1) TurnOrder[WizardTurn] = HeroType.Wizard;

        // Initialize the first turn to the hero with lowest rank
        int LowestRank = 99999;
        HeroType HeroWithLowestRank = TurnOrder[0];     // Default; will be overwritten

        for (int i = 0; i < TurnOrder.Length; i++)
        {
            Hero HeroToCheck = HeroManager.GetHero(TurnOrder[i]);

            if (HeroToCheck.GetRank() < LowestRank)
            {
                LowestRank = HeroToCheck.GetRank();
                HeroWithLowestRank = HeroToCheck.GetHeroType();
                break;
            }
        }

        CurrentTurnHero = HeroWithLowestRank;

        // Notify observers to update UI
        Notify("TURN");
    }

    // NETWORKED
    // Advances the turn order on all machines
    [PunRPC]
    public void AdvanceTurnRPC()
    {
        GoToNextHeroTurn();
    }
}
