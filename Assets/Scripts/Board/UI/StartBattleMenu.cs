﻿using System.Collections;
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

    // Battle menu launched by this menu
    [SerializeField]
    GameObject BattleMenuObject = null;
    BattleMenu BattleMenu = null;

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
    GameObject HerbGorStartBattleIcon = null;
    [SerializeField]
    GameObject SkralStartBattleIcon = null;
    [SerializeField]
    GameObject TowerSkralStartBattleIcon = null;
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
    GameObject WarriorStartBattleInviteSpinner = null;
    [SerializeField]
    GameObject ArcherStartBattleInviteSpinner = null;
    [SerializeField]
    GameObject DwarfStartBattleInviteSpinner = null;
    [SerializeField]
    GameObject WizardStartBattleInviteSpinner = null;

    [SerializeField]
    GameObject WarriorStartBattleInviteCheck = null;
    [SerializeField]
    GameObject ArcherStartBattleInviteCheck = null;
    [SerializeField]
    GameObject DwarfStartBattleInviteCheck = null;
    [SerializeField]
    GameObject WizardStartBattleInviteCheck = null;

    [SerializeField]
    GameObject WarriorStartBattleInviteX = null;
    [SerializeField]
    GameObject ArcherStartBattleInviteX = null;
    [SerializeField]
    GameObject DwarfStartBattleInviteX = null;
    [SerializeField]
    GameObject WizardStartBattleInviteX = null;

    [SerializeField]
    GameObject StartBattleInfoText = null;
    [SerializeField]
    GameObject StartBattleCancelButton = null;
    [SerializeField]
    GameObject StartBattleStartButton = null;
    [SerializeField]
    GameObject StartBattleCloseButton = null;
    [SerializeField]
    GameObject StartBattleOkButton = null;

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

        // Initialize reference to BattleMenu
        BattleMenu = BattleMenuObject.GetComponent<BattleMenu>();

        // Register as an observer of GameManager
        GameManager.Attach(this);
    }

    // To function correctly, the start battle menu must be displayed using this method
    public void Show(Creature Creature, StartBattleIcon Icon)
    {
        this.StartBattleIcon = Icon;

        Hero MainHero = GameManager.GetSelfHero();

        // If the hero already has a battle being created, use it
        Battle HeroBattle = MainHero.GetCurrentBattle();

        if (HeroBattle != null)
        {
            Battle = HeroBattle;
        }
        // Create a new battle if necessary
        else
        {
            Battle = new Battle(MainHero, Creature);

            // Register as an observer of this battle
            Battle.Attach(this);

            // Save the battle in the hero class
            MainHero.SetCurrentBattle(Battle);
        }

        this.gameObject.SetActive(true);

        InitializeUI();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
        if (StartBattleIcon != null) StartBattleIcon.Hide();
    }

    private void InitializeUI()
    {
        // Initialize UI
        SetInfoText("");

        EnableButton(StartBattleCancelButton);
        EnableButton(StartBattleStartButton);
        EnableButton(StartBattleCloseButton);
        EnableButton(StartBattleOkButton);
        StartBattleCancelButton.SetActive(true);
        StartBattleStartButton.SetActive(true);
        StartBattleCloseButton.SetActive(true);
        StartBattleOkButton.SetActive(false);

        WarriorStartBattleInviteIcon.GetComponent<HeroInviteIcon>().Reset();
        ArcherStartBattleInviteIcon.GetComponent<HeroInviteIcon>().Reset();
        DwarfStartBattleInviteIcon.GetComponent<HeroInviteIcon>().Reset();
        WizardStartBattleInviteIcon.GetComponent<HeroInviteIcon>().Reset();

        UpdateMainHero();
        UpdateMainCreature();
        UpdateAvailableOtherHeroes();
        UpdateWaitStatus();
        UpdateIfStarted();
        UpdateIfCancelled();
    }

    // Cancel battle triggered on purpose by the player
    public void CancelBattle()
    {
        // Cancel the battle
        Battle.Cancel();

        OkCancelBattle();       // Call the same function as when a hero clicks ok to acknowledge that a battle was cancelled
    }

    // Cleanup after a hero acknowledges that the battle has been cancelled
    public void OkCancelBattle()
    {
        Hero MainHero = GameManager.GetSelfHero();

        // Unregister from observing the battle
        Battle.Detach(this);

        // Remove the battle from the hero class
        MainHero.SetCurrentBattle(null);

        // Delete the battle that was being created
        Battle = null;

        this.Hide();
    }

    // Used in Observer design pattern
    public void UpdateData(string Category)
    {
        if (string.Equals(Category, "INVITE_STATUS"))
        {
            UpdateWaitStatus();
        }
        else if (string.Equals(Category, "TURN"))
        {
            UpdateAvailableOtherHeroes();
        }
        else if (string.Equals(Category, "STARTED"))
        {
            UpdateIfStarted();
        }
        else if (string.Equals(Category, "CANCELLED"))
        {
            UpdateIfCancelled();
        }
        else if (string.Equals(Category, "CONTROL"))
        {
            // When changing the controlled hero, check whether the hero already has an ongoing battle being created
            // If yes, show it. If no, don't show this menu.

            Hero MainHero = GameManager.GetSelfHero();

            // If the hero already has a battle being created, use it
            Battle HeroBattle = MainHero.GetCurrentBattle();

            if (HeroBattle != null && HeroBattle.IsPending() && MainHero.GetBattleInvitation() == null) this.Show(null, null);
            else this.Hide();
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
        // Default
        GorStartBattleIcon.SetActive(false);
        HerbGorStartBattleIcon.SetActive(false);
        SkralStartBattleIcon.SetActive(false);
        TowerSkralStartBattleIcon.SetActive(false);
        WardrakStartBattleIcon.SetActive(false);

        Creature MainCreature = Battle.GetCreature();

        if (MainCreature != null)
        {
            CreatureType Type = MainCreature.GetCreatureType();

            if      (Type == CreatureType.Gor) GorStartBattleIcon.SetActive(true);
            else if (Type == CreatureType.HerbGor) HerbGorStartBattleIcon.SetActive(true);
            else if (Type == CreatureType.Skral) SkralStartBattleIcon.SetActive(true);
            else if (Type == CreatureType.TowerSkral) TowerSkralStartBattleIcon.SetActive(true);
            else if (Type == CreatureType.Wardrak) WardrakStartBattleIcon.SetActive(true);
            else Debug.LogError("Cannot update opponent icons in StartBattleMenu; invalid creature type: " + Type);
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

        // Defaults
        WarriorStartBattleInviteIcon.SetActive(false);
        ArcherStartBattleInviteIcon.SetActive(false);
        DwarfStartBattleInviteIcon.SetActive(false);
        WizardStartBattleInviteIcon.SetActive(false);
        StartBattleNoOtherHeroesText.SetActive(true);
        SetInfoText("");
        EnableButton(StartBattleStartButton);
        EnableButton(StartBattleCancelButton);

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

    // Displays spinners or checkmarks according to which heroes have responded to their invitations
    public void UpdateWaitStatus()
    {
        // Reset all the icons
        WarriorStartBattleInviteSpinner.SetActive(false);
        ArcherStartBattleInviteSpinner.SetActive(false);
        DwarfStartBattleInviteSpinner.SetActive(false);
        WizardStartBattleInviteSpinner.SetActive(false);

        WarriorStartBattleInviteCheck.SetActive(false);
        ArcherStartBattleInviteCheck.SetActive(false);
        DwarfStartBattleInviteCheck.SetActive(false);
        WizardStartBattleInviteCheck.SetActive(false);

        WarriorStartBattleInviteX.SetActive(false);
        ArcherStartBattleInviteX.SetActive(false);
        DwarfStartBattleInviteX.SetActive(false);
        WizardStartBattleInviteX.SetActive(false);

        List<BattleInvitation> Invitations = Battle.GetInvitations();

        foreach (BattleInvitation Invite in Invitations)
        {
            HeroType Type = Invite.GetHero().GetHeroType();

            if (Invite.IsPending())
            {
                if (Type == HeroType.Warrior) WarriorStartBattleInviteSpinner.SetActive(true);
                if (Type == HeroType.Archer) ArcherStartBattleInviteSpinner.SetActive(true);
                if (Type == HeroType.Dwarf) DwarfStartBattleInviteSpinner.SetActive(true);
                if (Type == HeroType.Wizard) WizardStartBattleInviteSpinner.SetActive(true);
            }
            else if (Invite.WasAccepted())
            {
                if (Type == HeroType.Warrior) WarriorStartBattleInviteCheck.SetActive(true);
                if (Type == HeroType.Archer) ArcherStartBattleInviteCheck.SetActive(true);
                if (Type == HeroType.Dwarf) DwarfStartBattleInviteCheck.SetActive(true);
                if (Type == HeroType.Wizard) WizardStartBattleInviteCheck.SetActive(true);
            }
            else if (Invite.WasDeclined())
            {
                if (Type == HeroType.Warrior) WarriorStartBattleInviteX.SetActive(true);
                if (Type == HeroType.Archer) ArcherStartBattleInviteX.SetActive(true);
                if (Type == HeroType.Dwarf) DwarfStartBattleInviteX.SetActive(true);
                if (Type == HeroType.Wizard) WizardStartBattleInviteX.SetActive(true);
            }
        }
    }

    private void UpdateIfStarted()
    {
        int NumInvites = Battle.GetNumInvites();
        int NumPending = Battle.GetNumInvitesPending();
        int NumAccepted = Battle.GetNumInvitesAccepted();

        if (NumPending > 0)
        {
            SetInfoText("Waiting for invited heroes to respond...");
            DisableButton(StartBattleStartButton);
        }
        else if (NumAccepted == NumInvites && Battle.InvitationsWereSent())      // This will become true at the right time even for heroes fighting alone
        {
            SetInfoText("Starting battle...");
            DisableButton(StartBattleStartButton);
            DisableButton(StartBattleCancelButton);

            BattleMenu.SetBattle(Battle);
            this.Hide();
            BattleMenu.Show();
        }
    }

    private void UpdateIfCancelled()
    {
        if (Battle.IsCancelled())
        {
            if (Battle.DeclinedBySomeone()) SetInfoText("This battle has been cancelled because an invited hero declined.");
            else SetInfoText("This battle has been cancelled");
            DisableButton(StartBattleStartButton);
            StartBattleStartButton.SetActive(false);
            DisableButton(StartBattleCancelButton);
            StartBattleCancelButton.SetActive(false);
            StartBattleCloseButton.SetActive(false);
            StartBattleOkButton.SetActive(true);
        }
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

    // Player-triggered action
    public void StartBattle()
    {
        // Send inviations to any heroes that are invited (or none if fighting alone)
        Battle.SendInvitations();
        UpdateWaitStatus();
    }
}
