using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMenu : MonoBehaviour, Observer
{
    // Reference to GameManager
    private GameManager GameManager;

    // References to children components
    // Entire hero sections
    [SerializeField]
    GameObject WarriorSection = null;
    [SerializeField]
    GameObject ArcherSection = null;
    [SerializeField]
    GameObject DwarfSection = null;
    [SerializeField]
    GameObject WizardSection = null;

    // Markers for whose turn it is
    [SerializeField]
    GameObject WarriorTurnBox = null;
    [SerializeField]
    GameObject ArcherTurnBox = null;
    [SerializeField]
    GameObject DwarfTurnBox = null;
    [SerializeField]
    GameObject WizardTurnBox = null;

    // Markers for who you're controlling
    [SerializeField]
    GameObject WarriorThisIsMe = null;
    [SerializeField]
    GameObject ArcherThisIsMe = null;
    [SerializeField]
    GameObject DwarfThisIsMe = null;
    [SerializeField]
    GameObject WizardThisIsMe = null;

    // Start is called before the first frame update
    void Start()
    {
        // Not used to ensure UI elements are initialized in the right order.
        // UIManager calls Initialize instead.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        // Initialize reference to GameManager
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Register as an observer of GameManager
        GameManager.Attach(this);

        // Initialize UI
        UpdateTurn();
        UpdateControl();
        UpdateVisibleBoxes();
    }

    // Used in Observer design pattern
    public void UpdateData(string Category)
    {
        if (string.Equals(Category, "TURN"))
        {
            UpdateTurn();
        }
        else if (string.Equals(Category, "CONTROL"))
        {
            UpdateControl();
        }
        else if (string.Equals(Category, "PLAYING_HEROES"))
        {
            UpdateVisibleBoxes();
        }
    }

    private void UpdateTurn()
    {
        // Defaults
        WarriorTurnBox.SetActive(false);
        ArcherTurnBox.SetActive(false);
        DwarfTurnBox.SetActive(false);
        WizardTurnBox.SetActive(false);

        Hero CurrentTurnHero = GameManager.GetCurrentTurnHero();

        if (CurrentTurnHero != null)
        {
            HeroType Type = CurrentTurnHero.GetHeroType();

            if      (Type == HeroType.Warrior) WarriorTurnBox.SetActive(true);
            else if (Type == HeroType.Archer) ArcherTurnBox.SetActive(true);
            else if (Type == HeroType.Dwarf) DwarfTurnBox.SetActive(true);
            else if (Type == HeroType.Wizard) WizardTurnBox.SetActive(true);
            else Debug.LogError("Cannot update turn marker in HeroMenu; invalid hero type: " + Type);
        }
    }

    private void UpdateControl()
    {
        // Defaults
        WarriorThisIsMe.SetActive(false);
        ArcherThisIsMe.SetActive(false);
        DwarfThisIsMe.SetActive(false);
        WizardThisIsMe.SetActive(false);

        Hero CurrentTurnHero = GameManager.GetSelfHero();

        if (CurrentTurnHero != null)
        {
            HeroType Type = CurrentTurnHero.GetHeroType();

            if (Type == HeroType.Warrior) WarriorThisIsMe.SetActive(true);
            else if (Type == HeroType.Archer) ArcherThisIsMe.SetActive(true);
            else if (Type == HeroType.Dwarf) DwarfThisIsMe.SetActive(true);
            else if (Type == HeroType.Wizard) WizardThisIsMe.SetActive(true);
            else Debug.LogError("Cannot update control marker in HeroMenu; invalid hero type: " + Type);
        }
    }

    private void UpdateVisibleBoxes()
    {
        // Warrior
        bool Warrior = GameManager.IsPlaying(HeroType.Warrior);
        bool Archer = GameManager.IsPlaying(HeroType.Archer);
        bool Dwarf = GameManager.IsPlaying(HeroType.Dwarf);
        bool Wizard = GameManager.IsPlaying(HeroType.Wizard);

        WarriorSection.SetActive(Warrior);
        ArcherSection.SetActive(Archer);
        DwarfSection.SetActive(Dwarf);
        WizardSection.SetActive(Wizard);
    }
}
