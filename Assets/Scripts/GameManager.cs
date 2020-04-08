using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DifficultyLevel { Easy, Normal };

public class GameManager : MonoBehaviour, Subject
{
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
    private AndorPlayer MyPlayer;            // The player currently playing in this session (same value as one of the four player variables below)
    private AndorPlayer WarriorPlayer;
    private AndorPlayer ArcherPlayer;
    private AndorPlayer DwarfPlayer;
    private AndorPlayer WizardPlayer;

    // Turn management
    private AndorPlayer CurrentTurnPlayer;   // The player whose turn it is
    private List<AndorPlayer> TurnOrder;

    // Game mode
    private DifficultyLevel Difficulty;

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

    // Start is called before the first frame update
    void Start()
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

        // Initialize game difficulty
        Difficulty = DifficultyLevel.Normal;    // TODO real value

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

        // Initialize playing character
        MyPlayer = WarriorPlayer;              // TODO real value

        // Initialize turns
        TurnOrder = GenerateTurnOrder();
        CurrentTurnPlayer = TurnOrder[0];

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
        return CurrentTurnPlayer;
    }

    public Hero GetCurrentTurnHero()
    {
        if (CurrentTurnPlayer == null) return null;
        else return CurrentTurnPlayer.GetHero();
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
    private List<AndorPlayer> GenerateTurnOrder()
    {
        int NumOfPlayers = GetNumOfPlayers();
        List<AndorPlayer> NewTurnOrder = new List<AndorPlayer>(NumOfPlayers);

        // Create a grab bag from which to draw
        List<AndorPlayer> GrabBag = new List<AndorPlayer>(4);
        if (WarriorIsPlaying) GrabBag.Add(WarriorPlayer);
        if (ArcherIsPlaying) GrabBag.Add(ArcherPlayer);
        if (DwarfIsPlaying) GrabBag.Add(DwarfPlayer);
        if (WizardIsPlaying) GrabBag.Add(WizardPlayer);

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
            RandomIndex = Random.Range(0, GrabBag.Count);
            NewTurnOrder.Insert(i, GrabBag[RandomIndex]);
            GrabBag.RemoveAt(RandomIndex);
        }

        return NewTurnOrder;
    }

    // Gives the turn to the next hero in the turn order
    public void GoToNextHeroTurn()
    {
        int CurrentIndex = TurnOrder.IndexOf(CurrentTurnPlayer);

        // Validation
        if (CurrentIndex == -1)
        {
            Debug.LogError("Could not find the hero whose turn it is in the turn order.");
            return;
        }

        CurrentTurnPlayer = TurnOrder[ (CurrentIndex + 1) % TurnOrder.Count ];

        // Notify observers to update UI
        Notify("TURN");
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
}
