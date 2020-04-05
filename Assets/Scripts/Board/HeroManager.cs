using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeroType { Warrior, Archer, Dwarf, Wizard };

public class HeroManager : MonoBehaviour
{
    // Reference to WaypointManager
    private WaypointManager WaypointManager;

    // References to the heroes
    private Hero Warrior;
    private Hero Archer;
    private Hero Dwarf;
    private Hero Wizard;

    // Keeps track of whether each hero has been initialized
    private bool WarriorWasInitialized = false;
    private bool ArcherWasInitialized = false;
    private bool DwarfWasInitialized = false;
    private bool WizardWasInitialized = false;

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
        WaypointManager = GameObject.Find("WaypointManager").GetComponent<WaypointManager>();

        // Initialize references to the Heroes
        Warrior = GameObject.Find("Warrior").GetComponent<Hero>();
        Archer = GameObject.Find("Archer").GetComponent<Hero>();
        Dwarf = GameObject.Find("Dwarf").GetComponent<Hero>();
        Wizard = GameObject.Find("Wizard").GetComponent<Hero>();

        // Initialize the hero types
        Warrior.SetHeroType(HeroType.Warrior);
        Archer.SetHeroType(HeroType.Archer);
        Dwarf.SetHeroType(HeroType.Dwarf);
        Wizard.SetHeroType(HeroType.Wizard);
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

            default:
                Debug.LogError("Cannot get hero; invalid hero type: " + Type);
                return null;
        }
    }

    // Returns a list of the heroes (by type) eligible for a battle against a given creature
    public List<HeroType> GetHeroesEligibleForBattle(Battle Battle)
    {
        List<HeroType> EligibleHeroes = new List<HeroType>();
        Creature Creature = Battle.GetCreature();

        if (Warrior.IsEligibleForBattle(Creature)) EligibleHeroes.Add(Warrior.GetHeroType());
        if (Archer.IsEligibleForBattle(Creature)) EligibleHeroes.Add(Archer.GetHeroType());
        if (Dwarf.IsEligibleForBattle(Creature)) EligibleHeroes.Add(Dwarf.GetHeroType());
        if (Wizard.IsEligibleForBattle(Creature)) EligibleHeroes.Add(Wizard.GetHeroType());

        return EligibleHeroes;
    }

    public List<HeroType> GetAllHeroTypes()
    {
        List<HeroType> HeroTypes = new List<HeroType>();

        HeroTypes.Add(HeroType.Warrior);
        HeroTypes.Add(HeroType.Archer);
        HeroTypes.Add(HeroType.Dwarf);
        HeroTypes.Add(HeroType.Wizard);

        return HeroTypes;
    }

    //public void Move()
    //{
    //    GameManager.GetCurrentTurnHero().Move();
    //}
}
