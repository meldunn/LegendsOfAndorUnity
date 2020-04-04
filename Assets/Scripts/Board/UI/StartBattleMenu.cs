using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartBattleMenu : MonoBehaviour, Observer
{
    // References to managers
    private GameManager GameManager;
    private HeroManager HeroManager;
    private CreatureManager CreatureManager;

    // Battle object
    Battle Battle;

    // Icon that launched this menu
    StartBattleIcon StartBattleIcon;

    // References to children components
    [SerializeField]
    GameObject WarriorStartBattleIcon = null;
    [SerializeField]
    GameObject ArcherStartBattleIcon = null;
    [SerializeField]
    GameObject DwarfStartBattleIcon = null;
    [SerializeField]
    GameObject WizardStartBattleIcon = null;
    [SerializeField]
    GameObject GorStartBattleIcon = null;
    [SerializeField]
    GameObject SkralStartBattleIcon = null;
    [SerializeField]
    GameObject WardrakStartBattleIcon = null;

    [SerializeField]
    GameObject WarriorStartBattleInviteIcon = null;
    [SerializeField]
    GameObject ArcherStartBattleInviteIcon = null;
    [SerializeField]
    GameObject DwarfStartBattleInviteIcon = null;
    [SerializeField]
    GameObject WizardStartBattleInviteIcon = null;
    [SerializeField]
    GameObject StartBattleNoOtherHeroesText = null;

    [SerializeField]
    GameObject StartBattleInfoText = null;
    [SerializeField]
    GameObject StartBattleCancelButton = null;
    [SerializeField]
    GameObject StartBattleStartButton = null;

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
    }

    // To function correctly, the start battle menu must be displayed using this method
    public void Show(Creature Creature, StartBattleIcon Icon)
    {
        this.StartBattleIcon = Icon;

        Hero MainHero = GameManager.GetSelfHero();

        // Create a new battle if necessary
        Battle = new Battle(MainHero, Creature);

        // Register as an observer of Battle
        Battle.Attach(this);

        this.gameObject.SetActive(true);

        InitializeUI();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        StartBattleIcon.Hide();
    }

    private void InitializeUI()
    {
        // Initialize UI
        SetInfoText("");
        EnableButton(StartBattleStartButton);
        EnableButton(StartBattleCancelButton);

        WarriorStartBattleInviteIcon.GetComponent<HeroInviteIcon>().Reset();
        ArcherStartBattleInviteIcon.GetComponent<HeroInviteIcon>().Reset();
        DwarfStartBattleInviteIcon.GetComponent<HeroInviteIcon>().Reset();
        WizardStartBattleInviteIcon.GetComponent<HeroInviteIcon>().Reset();

        UpdateMainHero();
        UpdateMainCreature();
        UpdateAvailableOtherHeroes();
        UpdateWaitStatus();
    }

    public void CancelBattle()
    {
        // TODO cancel battle

        // Delete the new battle that was being created
        Battle = null;

        Hide();
    }

    public void StartBattle()
    {
        SetInfoText("This has not been implemented yet.");
        // Re-validate the battle

        // TODO
    }

    public void UpdateData(string Category)
    {
        if (string.Equals(Category, "INVITE"))
        {
            UpdateWaitStatus();
        }
    }

    public void SetInfoText(string Text)
    {
        TextMeshProUGUI InfoText = StartBattleInfoText.GetComponent<TextMeshProUGUI>();
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

    public void UpdateMainHero()
    {
        Hero MainHero = GameManager.GetSelfHero();

        if (MainHero != null)
        {
            HeroType Type = MainHero.GetHeroType();

            switch (Type)
            {
                case HeroType.Warrior:
                    WarriorStartBattleIcon.SetActive(true);
                    ArcherStartBattleIcon.SetActive(false);
                    DwarfStartBattleIcon.SetActive(false);
                    WizardStartBattleIcon.SetActive(false);
                    break;

                case HeroType.Archer:
                    WarriorStartBattleIcon.SetActive(false);
                    ArcherStartBattleIcon.SetActive(true);
                    DwarfStartBattleIcon.SetActive(false);
                    WizardStartBattleIcon.SetActive(false);
                    break;

                case HeroType.Dwarf:
                    WarriorStartBattleIcon.SetActive(false);
                    ArcherStartBattleIcon.SetActive(false);
                    DwarfStartBattleIcon.SetActive(true);
                    WizardStartBattleIcon.SetActive(false);
                    break;

                case HeroType.Wizard:
                    WarriorStartBattleIcon.SetActive(false);
                    ArcherStartBattleIcon.SetActive(false);
                    DwarfStartBattleIcon.SetActive(false);
                    WizardStartBattleIcon.SetActive(true);
                    break;

                default:
                    Debug.LogError("Cannot update opponent icons in StartBattleMenu; invalid hero type: " + Type);
                    break;
            }
        }
    }
    
    public void UpdateMainCreature()
    {
        Creature MainCreature = Battle.GetCreature();

        if (MainCreature != null)
        {
            CreatureType Type = MainCreature.GetCreatureType();

            switch (Type)
            {
                case CreatureType.Gor:
                    GorStartBattleIcon.SetActive(true);
                    SkralStartBattleIcon.SetActive(false);
                    WardrakStartBattleIcon.SetActive(false);
                    break;

                case CreatureType.Skral:
                    GorStartBattleIcon.SetActive(false);
                    SkralStartBattleIcon.SetActive(true);
                    WardrakStartBattleIcon.SetActive(false);
                    break;

                case CreatureType.Wardrak:
                    GorStartBattleIcon.SetActive(false);
                    SkralStartBattleIcon.SetActive(false);
                    WardrakStartBattleIcon.SetActive(true);
                    break;

                default:
                    Debug.LogError("Cannot update opponent icons in StartBattleMenu; invalid creature type: " + Type);
                    break;
            }
        }
    }

    public void UpdateAvailableOtherHeroes()
    {
        // List of all heroes who are elegible for battle
        List<HeroType> AvailableHeroes = HeroManager.GetHeroesEligibleForBattle(Battle);

        // The main hero starting the battle
        HeroType MainHero = GameManager.GetSelfHero().GetHeroType();
        HeroType TurnHero = GameManager.GetCurrentTurnHero().GetHeroType();

        // All other heroes
        List<HeroType> OtherHeroes = HeroManager.GetAllHeroTypes();
        OtherHeroes.Remove(MainHero);

        // Initially set all invitation icons to inactive
        WarriorStartBattleInviteIcon.SetActive(false);
        ArcherStartBattleInviteIcon.SetActive(false);
        DwarfStartBattleInviteIcon.SetActive(false);
        WizardStartBattleInviteIcon.SetActive(false);
        StartBattleNoOtherHeroesText.SetActive(true);

        // Confirm that our hero is eligible for battle
        if (AvailableHeroes.IndexOf(MainHero) == -1)
        {
            SetInfoText("You are not close enough to fight this creature.");
            DisableButton(StartBattleStartButton);
        }
        else if (MainHero != TurnHero)
        {
            SetInfoText("It's not your turn.");
            DisableButton(StartBattleStartButton);
        }

        // Compare the list of eligible heroes to the list of all other heroes and update the UI based on which ones are available
        foreach (HeroType OtherHero in OtherHeroes)
        {
            if (AvailableHeroes.IndexOf(OtherHero) != -1)
            {
                StartBattleNoOtherHeroesText.SetActive(false);
                if (OtherHero == HeroType.Warrior) WarriorStartBattleInviteIcon.SetActive(true);
                if (OtherHero == HeroType.Archer) ArcherStartBattleInviteIcon.SetActive(true);
                if (OtherHero == HeroType.Dwarf) DwarfStartBattleInviteIcon.SetActive(true);
                if (OtherHero == HeroType.Wizard) WizardStartBattleInviteIcon.SetActive(true);
            }
        }
    }

    public void UpdateWaitStatus()
    {
        // TODO
    }

    // Adds the target hero to the list of heroes to invite to the battle
    public void AddInvite(HeroType Type)
    {
        Hero TargetHero = HeroManager.GetHero(Type);
        Battle.AddHeroToInvite(TargetHero);
    }

    // Removes the target hero from the list of heroes to invite to the battle
    public void RemoveInvite(HeroType Type)
    {
        Hero TargetHero = HeroManager.GetHero(Type);
        Battle.RemoveHeroToInvite(TargetHero);
    }
}
