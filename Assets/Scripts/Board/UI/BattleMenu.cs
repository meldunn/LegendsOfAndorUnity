using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Menu used during a battle against a creature
public class BattleMenu : MonoBehaviour, Observer
{
    // References to managers
    private GameManager GameManager;
    private HeroManager HeroManager;
    private CreatureManager CreatureManager;

    // Battle object
    Battle Battle;

    // Menu that launched this menu
    StartBattleMenu StartBattleMenu;

    // References to children components
    // Stats boxes
    [SerializeField]
    GameObject WarriorBox = null;
    [SerializeField]
    GameObject ArcherBox = null;
    [SerializeField]
    GameObject DwarfBox = null;
    [SerializeField]
    GameObject WizardBox = null;
    //[SerializeField]
    //GameObject CreatureBox = null;

    // Creature icons in the creature box (must choose one to show)
    [SerializeField]
    GameObject GorBattleIcon = null;
    [SerializeField]
    GameObject SkralBattleIcon = null;
    [SerializeField]
    GameObject WardrakBattleIcon = null;

    // Strength values
    [SerializeField]
    GameObject WarriorStrength = null;
    [SerializeField]
    GameObject ArcherStrength = null;
    [SerializeField]
    GameObject DwarfStrength = null;
    [SerializeField]
    GameObject WizardStrength = null;
    [SerializeField]
    GameObject CreatureStrength = null;

    // Roll values
    //[SerializeField]
    //GameObject WarriorRoll = null;
    //[SerializeField]
    //GameObject ArcherRoll = null;
    //[SerializeField]
    //GameObject DwarfRoll = null;
    //[SerializeField]
    //GameObject WizardRoll = null;
    //[SerializeField]
    //GameObject CreatureRoll = null;

    // Willpower values
    [SerializeField]
    GameObject WarriorWillpower = null;
    [SerializeField]
    GameObject ArcherWillpower = null;
    [SerializeField]
    GameObject DwarfWillpower = null;
    [SerializeField]
    GameObject WizardWillpower = null;
    [SerializeField]
    GameObject CreatureWillpower = null;

    // Battle values
    //[SerializeField]
    //GameObject HeroBattleValue = null;
    //[SerializeField]
    //GameObject CreatureBattleValue = null;

    // Hero dice
    //[SerializeField]
    //GameObject[] HeroDice = null;

    // Creature dice
    //[SerializeField]
    //GameObject[] CreatureDice = null;

    // Info text and buttons
    [SerializeField]
    GameObject BattleInfoText = null;
    [SerializeField]
    GameObject BattleRollButton = null;
    [SerializeField]
    GameObject BattleKeepRollButton = null;

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
        // Initialize references to managers
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        HeroManager = GameObject.Find("HeroManager").GetComponent<HeroManager>();
        CreatureManager = GameObject.Find("CreatureManager").GetComponent<CreatureManager>();

        // Register as an observer of GameManager
        GameManager.Attach(this);
    }

    // To function correctly, the battle menu must be displayed using this method
    public void Show()
    {
        // If the battle hasn't been set, nothing happens
        if (Battle == null)
        {
            Debug.LogError("The battle menu can't be shown because no battle was set");
            return;
        }

        this.gameObject.SetActive(true);

        InitializeUI();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    // Used in Observer design pattern
    public void UpdateData(string Category)
    {
        if (string.Equals(Category, "TURN"))
        {
            
        }
        else if (string.Equals(Category, "CANCELLED"))
        {
            
        }
        else if (string.Equals(Category, "CONTROL"))
        {
            // When changing the controlled hero, check whether the hero already has an ongoing battle
            // If yes, show it. If no, don't show this menu.

            Hero MainHero = GameManager.GetSelfHero();

            // If the hero already has a battle being created, use it
            Battle HeroBattle = MainHero.GetCurrentBattle();

            if (HeroBattle != null && HeroBattle.IsStarted()) this.Show();
            else this.Hide();
        }
    }

    public void SetBattle(Battle NewBattle)
    {
        // If there was a previous battle, unregister as an observer of it
        if (Battle != null) Battle.Detach(this);

        // Save the new battle
        this.Battle = NewBattle;

        // Register as an observer of the new battle
        Battle.Attach(this);
    }

    private void InitializeUI()
    {
        // Initialize UI
        SetInfoText("");

        EnableButton(BattleRollButton);
        EnableButton(BattleKeepRollButton);
        // BattleRollButton.SetActive(true);
        BattleKeepRollButton.SetActive(false);

        UpdateHeroBoxes();
        UpdateMainCreature();
        UpdateHeroInfo();
        UpdateCreatureInfo();
    }

    public void SetInfoText(string Text)
    {
        SetText(BattleInfoText, Text);
    }

    public void SetText(GameObject Display, string Text)
    {
        TextMeshProUGUI InfoText = Display.GetComponent<TextMeshProUGUI>();
        InfoText.SetText(Text);
    }

    public void EnableButton(GameObject Button)
    {
        Button.GetComponent<Button>().interactable = true;
    }

    public void DisableButton(GameObject Button)
    {
        Button.GetComponent<Button>().interactable = false;
    }

    // Shows the right hero boxes based on which heroes are participating in the battle
    public void UpdateHeroBoxes()
    {
        List<Hero> Participants = Battle.GetParticipants();

        // Hide all boxes by default
        WarriorBox.SetActive(false);
        ArcherBox.SetActive(false);
        DwarfBox.SetActive(false);
        WizardBox.SetActive(false);

        foreach (Hero Participant in Participants)
        {
            HeroType Type = Participant.GetHeroType();

            if      (Type == HeroType.Warrior) WarriorBox.SetActive(true);
            else if (Type == HeroType.Archer) ArcherBox.SetActive(true);
            else if (Type == HeroType.Dwarf) DwarfBox.SetActive(true);
            else if (Type == HeroType.Wizard) WizardBox.SetActive(true);
        }
    }
    
    public void UpdateMainCreature()
    {
        Creature MainCreature = Battle.GetCreature();

        // Hide all icons by default
        GorBattleIcon.SetActive(false);
        SkralBattleIcon.SetActive(false);
        WardrakBattleIcon.SetActive(false);

        if (MainCreature != null)
        {
            CreatureType Type = MainCreature.GetCreatureType();

            if      (Type == CreatureType.Gor) GorBattleIcon.SetActive(true);
            else if (Type == CreatureType.Skral) SkralBattleIcon.SetActive(true);
            else if (Type == CreatureType.Wardrak) WardrakBattleIcon.SetActive(true);
        }
    }

    public void UpdateHeroInfo()
    {
        List<Hero> Participants = Battle.GetParticipants();

        foreach (Hero Participant in Participants)
        {
            HeroType Type = Participant.GetHeroType();

            if (Type == HeroType.Warrior)
            {
                SetText(WarriorStrength, Participant.getStrength().ToString());
                SetText(WarriorWillpower, Participant.getWillpower().ToString());
            }
            else if (Type == HeroType.Archer)
            {
                SetText(ArcherStrength, Participant.getStrength().ToString());
                SetText(ArcherWillpower, Participant.getWillpower().ToString());
            }
            else if (Type == HeroType.Dwarf)
            {
                SetText(DwarfStrength, Participant.getStrength().ToString());
                SetText(DwarfWillpower, Participant.getWillpower().ToString());
            }
            else if (Type == HeroType.Wizard)
            {
                SetText(WizardStrength, Participant.getStrength().ToString());
                SetText(WizardWillpower, Participant.getWillpower().ToString());
            }
        }
    }

    public void UpdateCreatureInfo()
    {
        Creature Enemy = Battle.GetCreature();

        CreatureType Type = Enemy.GetCreatureType();

        SetText(CreatureStrength, Enemy.GetStrength().ToString());
        SetText(CreatureWillpower, Enemy.GetWillpower().ToString());
    }
}
