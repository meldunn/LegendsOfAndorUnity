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
    private ChatManager ChatManager;

    // List of Observers (Observer design pattern)
    List<Observer> Observers = new List<Observer>();

    // Heroes playing status
    private bool WarriorIsPlaying;
    private bool ArcherIsPlaying;
    private bool DwarfIsPlaying;
    private bool WizardIsPlaying;

    // Game player
    private Hero MyHero;

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
        ChatManager = GameObject.Find("ChatManager").GetComponent<ChatManager>();

        // Initialize the non-UI managers
        WaypointManager.Initialize();
        HeroManager.Initialize();
        CreatureManager.Initialize();
        NarratorManager.Initialize();
        ChatManager.Initialize();

        //TONETWORK
        // Initialize game difficulty
        Difficulty = DifficultyLevel.Normal;    // Is later overwritten with real value

        //TONETWORK
        // Initialize which heroes are playing
        WarriorIsPlaying = true;                // Is later overwritten with real value
        ArcherIsPlaying = true;                 // Is later overwritten with real value
        DwarfIsPlaying = true;                  // Is later overwritten with real value
        WizardIsPlaying = true;                 // Is later overwritten with real value

        // Initialize heroes (including their board positions)
        if (WarriorIsPlaying) HeroManager.InitializeHero(HeroType.Warrior, InitialWarrior);
        if (ArcherIsPlaying) HeroManager.InitializeHero(HeroType.Archer, InitialArcher);
        if (DwarfIsPlaying) HeroManager.InitializeHero(HeroType.Dwarf, InitialDwarf);
        if (WizardIsPlaying) HeroManager.InitializeHero(HeroType.Wizard, InitialWizard);

        //TONETWORK
        // Initialize playing character
        MyHero = HeroManager.GetHero(HeroType.Warrior);                // Is later overwritten with real value

        // NETWORKED
        // Initialize turns
        HeroType[] NewTurnOrder;

        // Generate the turn order on one player's machine
        if (photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            Debug.Log("Machine " + GetSelfHero().GetHeroType() + " generated a turn order.");
            NewTurnOrder = GenerateTurnOrder();

            // Extract each hero's turn from the order (this is done because a HeroType[] can't be sent in an RPC)
            int WarriorTurn = Array.FindIndex(NewTurnOrder, e => e == HeroType.Warrior);
            int ArcherTurn = Array.FindIndex(NewTurnOrder, e => e == HeroType.Archer);
            int DwarfTurn = Array.FindIndex(NewTurnOrder, e => e == HeroType.Dwarf);
            int WizardTurn = Array.FindIndex(NewTurnOrder, e => e == HeroType.Wizard);

            // Send the order to the other players
            if (PhotonNetwork.IsConnected) photonView.RPC("SetTurnOrderRPC", RpcTarget.All, WarriorTurn, ArcherTurn, DwarfTurn, WizardTurn);
            else SetTurnOrderRPC(WarriorTurn, ArcherTurn, DwarfTurn, WizardTurn);
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

        //// Generate Initial Merchants
        //for(int i=0; i<MerchantLocation.Length; i++)
        //{
        //    string WaypointName = "Waypoint (" + MerchantLocation[i] + ")";
        //    GameObject Waypoint = GameObject.Find(WaypointName);
        //    Waypoint.AddComponent<Merchant>();

        //    Merchant Merchant = Waypoint.GetComponent<Merchant>();
        //    Merchant.Initialize();
        //}


        ChatManager.SendSystemMessage("Welcome to Legends of Andor!");

        // Initialize the UI manager
        // IMPORTANT: Do this last to ensure all data is available
        UIManager.Initialize(this);


        // Dummy Save Games
        // MerchantSavedGame();
        // FightingSavedGame();
        // LoseSavedGame();
        // WinSavedGame();
    }

    // Sets the difficulty and makes final initializations based on difficulty
    public void SetDifficulty(DifficultyLevel Difficulty)
    {
        this.Difficulty = Difficulty;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Get a reference to the hero of the player controlling this game instance
    public Hero GetSelfHero()
    {
        return MyHero;
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
        // Check whether it's the turn of the hero who wants to move
        Hero MyHero = GetSelfHero();
        HeroType MyHeroType = GetSelfHero().GetHeroType();
        Hero TurnHero = GetCurrentTurnHero();

        // Cannot move out of turn, without willpower, if you've ended your day or if you're already moving
        if (MyHero == TurnHero && MyHero.CanAdvanceTimeMarker(1) && !MyHero.HasEndedDay() && !HeroManager.GetHeroIsMoving())
        {
            if (PhotonNetwork.IsConnected) photonView.RPC("HeroMoveRPC", RpcTarget.All, MyHeroType);
            else HeroMoveRPC(MyHeroType);
        }
    }

    public void MoveThorald()
    {

    }

    public void NotifyHeroMove()
    {
        Notify("HERO_MOVE");
    }
    
    public void NotifyCreatureMove()
    {
        Notify("CREATURE_MOVE");
    }

    /*
     * Dummy Saved Games
     */
    public void MerchantSavedGame()
    {
        if(PhotonNetwork.IsConnected) photonView.RPC("MerchantSavedGameRPC", RpcTarget.All);
        else MerchantSavedGameRPC();
    }

    public void LoseSavedGame()
    {
        // AdvanceCreaturesForAll()
        // AdvanceCreaturesForAll()
        // if(PhotonNetwork.IsConnected) photonView.RPC("LoseSavedGameRPC", RpcTarget.All);
        // else LoseSavedGameRPC();
    }

    public void FightingSavedGame()
    {
        // TODO: Move all heroes to a spot, spawn a wardrak
        if(PhotonNetwork.IsConnected) photonView.RPC("FightingSavedGameRPC", RpcTarget.All);
        else FightingSavedGameRPC();

    }

    [PunRPC]
    public void FightingSavedGameRPC()
    {
        // TODO: Move all heroes to a spot, spawn a wardrak
        CreatureManager.Spawn(CreatureType.Wardrak, 14);
        HeroManager.TeleportRPC(HeroType.Archer, 6);
        HeroManager.TeleportRPC(HeroType.Wizard, 10);
        HeroManager.TeleportRPC(HeroType.Warrior, 14);
    }

    [PunRPC]
    public void LoseSavedGameRPC()
    {
        // TODO: Invoke the move function instead of teleport
        // TODO: Add other values (items?) to simulate a real game

    }
    
    [PunRPC]
    public void MerchantSavedGameRPC()
    {
        // TODO: Invoke the move function
        HeroManager.GetHero(HeroType.Warrior).ReceiveGold(10);
        HeroManager.TeleportRPC(HeroType.Warrior, 18);
        // TODO: Add other values (items?) to simulate a real game
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

        // If the hero can't take the turn because their day is ended, go to the next turn again
        if (HeroManager.GetHero(CurrentTurnHero).HasEndedDay() || !IsPlaying(CurrentTurnHero)) GoToNextHeroTurn();

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

    public void EndDay()
    {
        Debug.Log("It's a new dawn, it's a new day.");

        // Change the hero turn
        CurrentTurnHero = HeroManager.GetRoosterHero().GetHeroType();
        Notify("TURN");

        // Read an event card
        Debug.Log("TODO: Read an event card");

        // When this is done, advance the creatures (IMPORTANT: must be the last step in this function)
        CreatureManager.StartAdvancing(EndDaySecondHalf);       // Provide the second half as a callback when advancing is done
    }

    // This function is provided as a callback after creature advancing in EndDay()
    public void EndDaySecondHalf()
    {
        // Refill all wells
        WaypointManager.ReplenishAllWells();

        // Advance the narrator
        NarratorManager.advanceNarratorRPC(51);
    }

    // Changes control of the current game session to a different player.
    // Note: this is used to simulate local multiplayer (for testing and demonstration purposes) and will NOT be used in the real networked game
    public void SetSelfHero(HeroType ControlledHeroType)
    {
        MyHero = HeroManager.GetHero(ControlledHeroType);

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

    // Sets whether the specified hero is playing
    public void SetIsPlaying(HeroType Type, bool Value)
    {
        if (Type == HeroType.Warrior)
        {
            WarriorIsPlaying = Value;
            HeroManager.GetHero(HeroType.Warrior).gameObject.SetActive(Value);
        }
        else if (Type == HeroType.Archer)
        {
            ArcherIsPlaying = Value;
            HeroManager.GetHero(HeroType.Archer).gameObject.SetActive(Value);
        }
        else if (Type == HeroType.Dwarf)
        {
            DwarfIsPlaying = Value;
            HeroManager.GetHero(HeroType.Dwarf).gameObject.SetActive(Value);
        }
        else if (Type == HeroType.Wizard)
        {
            WizardIsPlaying = Value;
            HeroManager.GetHero(HeroType.Wizard).gameObject.SetActive(Value);
        }

        Notify("PLAYING_HEROES");
    }

    // Returns whether the specified hero is playing
    public bool IsPlaying(HeroType Type)
    {
        if (Type == HeroType.Warrior) return WarriorIsPlaying;
        else if (Type == HeroType.Archer) return ArcherIsPlaying;
        else if (Type == HeroType.Dwarf) return DwarfIsPlaying;
        else if (Type == HeroType.Wizard) return WizardIsPlaying;
        else return false;
    }

    // Used by the interface
    public void ToggleIsPlaying(HeroType Type)
    {
        SetIsPlaying(Type, !IsPlaying(Type));
    }

    // Used by the interface
    public void ToggleIsPlayingForAll(HeroType Type)
    {
        if (PhotonNetwork.IsConnected) photonView.RPC("ToggleIsPlayingRPC", RpcTarget.All, Type);
        else ToggleIsPlayingRPC(Type);
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

    [PunRPC]
    public void pickupFarmerRPC(HeroType type)
    {
        HeroManager.GetHero(type).pickupFarmer();
    }

    public void pickupFarmer()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("pickupFarmerRPC", RpcTarget.All, GetSelfHero().GetHeroType());
        }
        else
        {
            pickupFarmerRPC(GetSelfHero().GetHeroType());
        }
    }

    [PunRPC]
    public void dropFarmerRPC(HeroType type)
    {
        HeroManager.GetHero(type).dropFarmer();
    }
    
    public void dropFarmer()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("dropFarmerRPC", RpcTarget.All, GetSelfHero().GetHeroType());
        }
        else
        {
            dropFarmerRPC(GetSelfHero().GetHeroType());
        }
    }

    [PunRPC]
    public void pickupGoldRPC(HeroType type)
    {
        HeroManager.GetHero(type).pickupGold();
    }

    public void pickupGold()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("pickupGoldRPC", RpcTarget.All, GetSelfHero().GetHeroType());
        }
        else
        {
            pickupGoldRPC(GetSelfHero().GetHeroType());
        }
    }

    [PunRPC]
    public void dropGoldPRC(HeroType type)
    {
        HeroManager.GetHero(type).dropGold();
    }
    
    public void dropGold()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("dropGoldPRC", RpcTarget.All, GetSelfHero().GetHeroType());
        }
        else
        {
            dropGoldPRC(GetSelfHero().GetHeroType());
        }
    }

    // Used by the cheat menu only
    public void AdvanceCreaturesForAll()
    {
        if (PhotonNetwork.IsConnected) photonView.RPC("AdvanceCreaturesRPC", RpcTarget.All);
        else AdvanceCreaturesRPC();
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

    // NEWTORKED
    // Advances creatures on all machines (used in cheat menu)
    [PunRPC]
    public void AdvanceCreaturesRPC()
    {
        CreatureManager.StartAdvancing(null);
    }

    // NEWTORKED
    // Toggles whether the specified player is playing on all machines
    [PunRPC]
    public void ToggleIsPlayingRPC(HeroType Type)
    {
        ToggleIsPlaying(Type);
    }

    // NEWTORKED
    // Moves the specified hero on all machines
    [PunRPC]
    public void HeroMoveRPC(HeroType Type)
    {
        HeroManager.SetHeroIsMoving(true);
        HeroManager.GetHero(Type).Move();
    }
}
