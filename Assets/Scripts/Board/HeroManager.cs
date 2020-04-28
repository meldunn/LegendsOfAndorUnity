using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

//!!!DISCLAMER: DO NOT CHANGE THE ORDER OF THIS ENUM!!!
public enum HeroType { Warrior, Archer, Dwarf, Wizard, PrinceThorald };

public class HeroManager : MonoBehaviourPun
{
    // Reference to managers
    private GameManager GameManager;
    private WaypointManager WaypointManager;

    // References to the heroes
    private Hero Warrior;
    private Hero Archer;
    private Hero Dwarf;
    private Hero Wizard;
    private Hero PrinceThorald;

    // Keeps track of whether each hero has been initialized
    private bool WarriorWasInitialized = false;
    private bool ArcherWasInitialized = false;
    private bool DwarfWasInitialized = false;
    private bool WizardWasInitialized = false;
    private bool PrinceWasInitialized = false;

    // Start is called before the first frame update
    void Start()
    {
        // Not used to ensure Managers are started in the right order.
        // GameManager calls Initialize instead.   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        // Initialize reference to WaypointManager
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        WaypointManager = GameObject.Find("WaypointManager").GetComponent<WaypointManager>();

        // Initialize references to the Heroes
        Warrior = GameObject.Find("Warrior").GetComponent<Hero>();
        Archer = GameObject.Find("Archer").GetComponent<Hero>();
        Dwarf = GameObject.Find("Dwarf").GetComponent<Hero>();
        Wizard = GameObject.Find("Wizard").GetComponent<Hero>();
        PrinceThorald = GameObject.Find("PrinceThorald").GetComponent<Hero>();

        // Initialize the hero types
        Warrior.SetHeroType(HeroType.Warrior);
        Archer.SetHeroType(HeroType.Archer);
        Dwarf.SetHeroType(HeroType.Dwarf);
        Wizard.SetHeroType(HeroType.Wizard);
        PrinceThorald.SetHeroType(HeroType.PrinceThorald);
    }

    // Initializes the given hero by placing them on the correct region, and returns a reference to the hero
    public void InitializeHero(HeroType Type, int RegionNum)
    {
        // Get a reference to the correct hero
        Hero HeroToInitialize = null;

        switch (Type)
        {
            case HeroType.Warrior:

                if (WarriorWasInitialized)
                {
                    Debug.LogError("Error: the warrior has already been initialized.");
                    return;
                }
                HeroToInitialize = Warrior;
                WarriorWasInitialized = true;
                break;

            case HeroType.Archer:

                if (ArcherWasInitialized)
                {
                    Debug.LogError("Error: the acher has already been initialized.");
                    return;
                }
                HeroToInitialize = Archer;
                ArcherWasInitialized = true;
                break;

            case HeroType.Dwarf:

                if (DwarfWasInitialized)
                {
                    Debug.LogError("Error: the dwarf has already been initialized.");
                    return;
                }
                HeroToInitialize = Dwarf;
                DwarfWasInitialized = true;
                break;

            case HeroType.Wizard:

                if (WizardWasInitialized)
                {
                    Debug.LogError("Error: the wizard has already been initialized.");
                    return;
                }
                HeroToInitialize = Wizard;
                WizardWasInitialized = true;
                break;

            case HeroType.PrinceThorald:

                if (PrinceWasInitialized)
                {
                    Debug.LogError("Error: Prince Thorald has already been initialized.");
                    return;
                }
                HeroToInitialize = PrinceThorald;
                PrinceWasInitialized = true;
                break;

            default:
                Debug.LogError("Cannot initialize hero; invalid hero type: " + Type);
                return;
        }

        // Get a reference to the target waypoint
        Waypoint Target = WaypointManager.GetWaypoint(RegionNum);

        // Move the hero to the waypoint
        HeroToInitialize.transform.SetPositionAndRotation(Target.GetLocation(),     // Destination
            Quaternion.identity);                                                   // No rotation

        // Set the references between Hero and Waypoint
        HeroToInitialize.SetWaypoint(Target);
        Target.AddHero(HeroToInitialize);
    }

    public Hero GetHero(HeroType Type)
    {
        switch (Type)
        {
            case HeroType.Warrior:
                return Warrior;

            case HeroType.Archer:
                return Archer;

            case HeroType.Dwarf:
                return Dwarf;

            case HeroType.Wizard:
                return Wizard;

            case HeroType.PrinceThorald:
                return PrinceThorald;

            default:
                Debug.LogError("Cannot get hero; invalid hero type: " + Type);
                return null;
        }
    }

    // Returns a list of the heroes (by type) eligible for a battle against a given creature
    public List<HeroType> GetHeroesEligibleForBattle(Battle Battle)
    {
        List<HeroType> EligibleHeroes = new List<HeroType>();

        if (Battle != null)
        {
            Creature Creature = Battle.GetCreature();

            if (Warrior.IsEligibleForBattle(Creature)) EligibleHeroes.Add(Warrior.GetHeroType());
            if (Archer.IsEligibleForBattle(Creature)) EligibleHeroes.Add(Archer.GetHeroType());
            if (Dwarf.IsEligibleForBattle(Creature)) EligibleHeroes.Add(Dwarf.GetHeroType());
            if (Wizard.IsEligibleForBattle(Creature)) EligibleHeroes.Add(Wizard.GetHeroType());
        }

        return EligibleHeroes;
    }

    public List<HeroType> GetAllHeroTypes()
    {
        List<HeroType> HeroTypes = new List<HeroType>();

        HeroTypes.Add(HeroType.Warrior);
        HeroTypes.Add(HeroType.Archer);
        HeroTypes.Add(HeroType.Dwarf);
        HeroTypes.Add(HeroType.Wizard);
        // Do not return Prince Thorald

        return HeroTypes;
    }

    //public void Move()
    //{
    //    GameManager.GetCurrentTurnHero().Move();
    //}

    // Skips the turn of the current hero and advances their time marker by an hour
    public void PassTurn()
    {
        HeroType CurrentTurnHeroType = GameManager.GetCurrentTurnHero().GetHeroType();
        HeroType MyHeroType = GameManager.GetSelfHero().GetHeroType();

        // Only the hero whose turn it is can pass their turn
        if (MyHeroType == CurrentTurnHeroType)
        {
            if (PhotonNetwork.IsConnected) photonView.RPC("PassHeroTurnRPC", RpcTarget.All, MyHeroType);
            else PassHeroTurnRPC(MyHeroType);
        }
    }

    public void EndHeroDay()
    {
        HeroType CurrentTurnHeroType = GameManager.GetCurrentTurnHero().GetHeroType();
        HeroType MyHeroType = GameManager.GetSelfHero().GetHeroType();

        // Only the hero whose turn it is can end their day
        if (MyHeroType == CurrentTurnHeroType)
        {
            if (PhotonNetwork.IsConnected) photonView.RPC("EndHeroDayRPC", RpcTarget.All, MyHeroType);
            else EndHeroDayRPC(MyHeroType);
        }
    }

    // Tests whether to end the day for all, and triggers ending the day if necessary.
    // Returns whether the end-of-day was triggered.
    public bool TestEndDay()
    {
        bool EndDay = true;

        // Check whether all the heroes have ended their day
        foreach (HeroType Type in GetAllHeroTypes())
        {
            if (!GetHero(Type).HasEndedDay()) EndDay = false;
        }

        // Trigger ending day if necessary
        if (EndDay)
        {
            // Clear the ended day field for all (to allow turns to be set correctly)
            foreach (HeroType Type in GetAllHeroTypes()) GetHero(Type).StartHeroDay();

            // End the day
            GameManager.EndDay();
        }

        return EndDay;
    }

    // Returns the hero who is in the rooster box, or null if there is none
    public Hero GetRoosterHero()
    {
        Hero RoosterHero = null;

        // Check whether a hero is in the rooster box
        foreach (HeroType Type in GetAllHeroTypes())
        {
            if (GetHero(Type).IsInRoosterBox()) RoosterHero = GetHero(Type);
        }

        return RoosterHero;
    }

    // Moves the current hero to the region specified in the input field with no regards to game rules
    public void Teleport(GameObject Input)
    {
        // Get the region string to use based on the input
        string RegionString = Input.GetComponent<TMP_InputField>().text;

        // Get the hero type of the hero to teleport
        HeroType MyHeroType = GameManager.GetSelfHero().GetHeroType();

        // Try to convert the region string to a number
        int RegionNum;
        try
        {
            RegionNum = Int32.Parse(RegionString);

            if (WaypointManager.IsValidWaypoint(RegionNum))
            {
                if (PhotonNetwork.IsConnected) photonView.RPC("TeleportRPC", RpcTarget.All, MyHeroType, RegionNum);
                else TeleportRPC(MyHeroType, RegionNum);
            }
            else throw new FormatException();
        }
        catch (FormatException e)
        {
            Debug.LogError("Cannot teleport to region \"" + RegionString + "\"; invalid region number.");
        }
    }

    // Use for testing only; increments the current hero's time of day by the specified amount
    public void IncrementSelfHeroTime(int Amount)
    {
        HeroType MyHeroType = GameManager.GetSelfHero().GetHeroType();

        if (PhotonNetwork.IsConnected) photonView.RPC("IncrementHeroTimeRPC", RpcTarget.All, MyHeroType, Amount);
        else IncrementHeroTimeRPC(MyHeroType, Amount);
    }

    // NETWORKED
    [PunRPC]
    private void TeleportRPC(HeroType TargetHeroType, int RegionNum)
    {
        GetHero(TargetHeroType).Teleport(RegionNum);
    }

    // NETWORKED
    [PunRPC]
    private void IncrementHeroTimeRPC(HeroType TargetHeroType, int Amount)
    {
        GetHero(TargetHeroType).AdvanceTimeMarker(Amount);
    }

    // NETWORKED
    [PunRPC]
    private void PassHeroTurnRPC(HeroType TargetHeroType)
    {
        bool OperationSuccess = GetHero(TargetHeroType).AdvanceTimeMarker(1);
        if (OperationSuccess) GameManager.GoToNextHeroTurn();
    }

    // NETWORKED
    [PunRPC]
    private void EndHeroDayRPC(HeroType TargetHeroType)
    {
        // Check whether another hero is already in the rooster box
        Hero RoosterBoxHero = GetRoosterHero();
        bool GoToRoosterBox = RoosterBoxHero == null;

        // End the hero's day
        GetHero(TargetHeroType).EndHeroDay(GoToRoosterBox);

        // Test whether to end the day for all
        bool DayChanged = TestEndDay();

        // If the day hasn't changed, move the turn along
        if (!DayChanged) GameManager.GoToNextHeroTurn();
    }
}
