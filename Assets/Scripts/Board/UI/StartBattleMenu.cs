﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class StartBattleMenu : MonoBehaviourPun, Observer
{
    // References to managers
    private GameManager GameManager;
    private HeroManager HeroManager;
    private CreatureManager CreatureManager;
    private WaypointManager WaypointManager;

    // Battle objects (one per hero who is interacting with this menu)
    Battle WarriorBattle;
    Battle ArcherBattle;
    Battle DwarfBattle;
    Battle WizardBattle;

    // Icon that launched this menu (optional; always handle null case)
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

    // Creature stats
    [SerializeField]
    GameObject CreatureStrength = null;
    [SerializeField]
    GameObject CreatureWillpower = null;

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
        // Briefly show this menu to make Photon recognize its PhotonView
        this.gameObject.SetActive(true);
        this.gameObject.SetActive(false);

        // Initialize references to managers
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        HeroManager = GameObject.Find("HeroManager").GetComponent<HeroManager>();
        CreatureManager = GameObject.Find("CreatureManager").GetComponent<CreatureManager>();
        WaypointManager = GameObject.Find("WaypointManager").GetComponent<WaypointManager>();

        // Initialize reference to BattleMenu
        BattleMenu = BattleMenuObject.GetComponent<BattleMenu>();

        // Register as an observer of GameManager and of the four heroes
        GameManager.Attach(this);
        HeroManager.GetHero(HeroType.Warrior).Attach(this);
        HeroManager.GetHero(HeroType.Archer).Attach(this);
        HeroManager.GetHero(HeroType.Dwarf).Attach(this);
        HeroManager.GetHero(HeroType.Wizard).Attach(this);
    }

    // To function correctly, the start battle menu must be displayed using this method
    public void Show(Creature Creature, StartBattleIcon Icon)
    {
        this.StartBattleIcon = Icon;

        Hero MyHero = GameManager.GetSelfHero();
        HeroType MyHeroType = MyHero.GetHeroType();

        // Create a new battle if necessary (if there is none, or if the old draft was for a different creature)
        if (GetMyBattle() == null || GetMyBattle().GetCreature() != Creature)
        {
            if (Creature == null)
            {
                Debug.LogError("Cannot show StartBattleMenu because no battle draft is available and no creature was provided.");
                return;
            }

            int CreatureRegionNum = Creature.GetRegion().GetWaypointNum();

            // Save a new battle for the played hero, seen by all players (so other players can later receive invitations to this battle)
            // NETWORKED
            if (PhotonNetwork.IsConnected) photonView.RPC("CreateNewHeroBattleRPC", RpcTarget.All, MyHeroType, CreatureRegionNum);
            else CreateNewHeroBattleRPC(MyHeroType, CreatureRegionNum);

            // Register as an observer of the battle
            GetMyBattle().Attach(this);
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
        EnableButton(StartBattleOkButton);
        StartBattleCancelButton.SetActive(true);
        StartBattleStartButton.SetActive(true);
        StartBattleOkButton.SetActive(false);

        WarriorStartBattleInviteIcon.GetComponent<HeroInviteIcon>().Reset();
        ArcherStartBattleInviteIcon.GetComponent<HeroInviteIcon>().Reset();
        DwarfStartBattleInviteIcon.GetComponent<HeroInviteIcon>().Reset();
        WizardStartBattleInviteIcon.GetComponent<HeroInviteIcon>().Reset();

        UpdateMainHero();
        UpdateMainCreature();
        UpdateAvailableHeroes();
        UpdateWaitStatus();
        UpdateIfStarted();
        UpdateIfCancelled();
    }

    // Cancel battle triggered on purpose by the player
    public void CancelBattle()
    {
        Hero MainHero = GameManager.GetSelfHero();
        HeroType MainHeroType = MainHero.GetHeroType();

        // Cancel the battle
        // NETWORKED
        if (PhotonNetwork.IsConnected) photonView.RPC("CancelHeroBattleRPC", RpcTarget.All, MainHeroType);
        else CancelHeroBattleRPC(MainHeroType);
    }

    // Cleanup after a hero acknowledges that the battle has been cancelled
    public void OkCancelBattle()
    {
        Hero MainHero = GameManager.GetSelfHero();
        HeroType MainHeroType = MainHero.GetHeroType();

        // Unregister from observing the battle
        if (GetMyBattle() != null) GetMyBattle().Detach(this);

        // Cleanup after cancelling the battle
        // NETWORKED
        if (PhotonNetwork.IsConnected) photonView.RPC("CleanupHeroBattleRPC", RpcTarget.All, MainHeroType);
        else CleanupHeroBattleRPC(MainHeroType);

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
            UpdateAvailableHeroes();
        }
        else if (string.Equals(Category, "STARTED"))
        {
            UpdateIfStarted();
        }
        else if (string.Equals(Category, "START_CANCELLED"))
        {
            UpdateWaitStatus();
            UpdateIfCancelled();
        }
        else if (string.Equals(Category, "HERO_TIME"))
        {
            UpdateAvailableHeroes();
        }
        else if (string.Equals(Category, "HERO_WILLPOWER"))
        {
            UpdateAvailableHeroes();
        }
        else if (string.Equals(Category, "PLAYING_HEROES"))
        {
            UpdateAvailableHeroes();
        }
        else if (string.Equals(Category, "HERO_MOVE"))
        {
            UpdateAvailableHeroes();
        }
        else if (string.Equals(Category, "CREATURE_MOVE"))
        {
            UpdateAvailableHeroes();
        }
        else if (string.Equals(Category, "CONTROL"))
        {
            // When changing the controlled hero, check whether the hero already has an ongoing battle being created
            // If yes, show it. If no, don't show this menu.

            Hero MyHero = GameManager.GetSelfHero();

            // If the hero already has a battle being created, use it
            Battle HeroBattle = GetMyBattle();

            if (HeroBattle != null && HeroBattle.IsPending() && MyHero.GetBattleInvitation() == null) this.Show(null, null);
            else this.Hide();
        }
    }

    public void SetInfoText(string Text)
    {
        SetText(StartBattleInfoText, Text);
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

        Creature MainCreature = GetMyBattle().GetCreature();

        if (MainCreature != null)
        {
            CreatureType Type = MainCreature.GetCreatureType();

            if      (Type == CreatureType.Gor) GorStartBattleIcon.SetActive(true);
            else if (Type == CreatureType.HerbGor) HerbGorStartBattleIcon.SetActive(true);
            else if (Type == CreatureType.Skral) SkralStartBattleIcon.SetActive(true);
            else if (Type == CreatureType.TowerSkral) TowerSkralStartBattleIcon.SetActive(true);
            else if (Type == CreatureType.Wardrak) WardrakStartBattleIcon.SetActive(true);
            else Debug.LogError("Cannot update opponent icons in StartBattleMenu; invalid creature type: " + Type);

            // Display creature stats
            SetText(CreatureStrength, MainCreature.GetStrength().ToString());
            SetText(CreatureWillpower, MainCreature.GetWillpower().ToString());
        }
    }

    public void UpdateAvailableHeroes()
    {
        // List of all heroes who are elegible for battle
        List<HeroType> AvailableHeroes = HeroManager.GetHeroesEligibleForBattle(GetMyBattle());

        // The main hero starting the battle
        Hero MainHero = GameManager.GetSelfHero();
        HeroType MainHeroType = MainHero.GetHeroType();
        Hero TurnHero = GameManager.GetCurrentTurnHero();
        HeroType TurnHeroType = TurnHero.GetHeroType();

        // All other heroes
        List<HeroType> OtherHeroes = HeroManager.GetAllHeroTypes();
        OtherHeroes.Remove(MainHeroType);

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
        if (MainHeroType != TurnHeroType)
        {
            SetInfoText("It's not your turn.");
            DisableButton(StartBattleStartButton);
        }
        else if (AvailableHeroes.IndexOf(MainHeroType) == -1)
        {
            SetInfoText("You are not close enough to fight this creature.");
            DisableButton(StartBattleStartButton);
        }
        else if (!MainHero.CanAdvanceTimeMarker(1))
        {
            SetInfoText("You don't have enough time (or willpower for overtime hours) to fight.");
            DisableButton(StartBattleStartButton);
        }
        else if (MainHero.HasEndedDay())
        {
            SetInfoText("You can't fight because you've ended your day.");
            DisableButton(StartBattleStartButton);
        }
        else if (!GameManager.IsPlaying(MainHero.GetHeroType()))
        {
            SetInfoText("You can't fight if you're not playing.");
            DisableButton(StartBattleStartButton);
        }

        // Compare the list of eligible heroes to the list of all other heroes and update the UI based on which ones are available
        foreach (HeroType OtherHeroType in OtherHeroes)
        {
            Hero OtherHero = HeroManager.GetHero(OtherHeroType);
            bool OtherConditionsOk = OtherHero.CanAdvanceTimeMarker(1) && !OtherHero.HasEndedDay() && GameManager.IsPlaying(OtherHero.GetHeroType());

            if (AvailableHeroes.IndexOf(OtherHeroType) != -1 && OtherConditionsOk)
            {
                StartBattleNoOtherHeroesText.SetActive(false);
                if (OtherHeroType == HeroType.Warrior) WarriorStartBattleInviteIcon.SetActive(true);
                if (OtherHeroType == HeroType.Archer) ArcherStartBattleInviteIcon.SetActive(true);
                if (OtherHeroType == HeroType.Dwarf) DwarfStartBattleInviteIcon.SetActive(true);
                if (OtherHeroType == HeroType.Wizard) WizardStartBattleInviteIcon.SetActive(true);
            }
        }
    }

    // Displays spinners or checkmarks according to which heroes have responded to their invitations
    public void UpdateWaitStatus()
    {
        if (GetMyBattle() == null) return;

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

        List<BattleInvitation> Invitations = GetMyBattle().GetInvitations();

        foreach (BattleInvitation Invite in Invitations)
        {
            HeroType Type = Invite.GetHero().GetHeroType();

            if (Invite.IsPending() && GetMyBattle().IsPending())
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
        if (GetMyBattle() == null) return;

        // Current hero
        Hero MyHero = GameManager.GetSelfHero();
        HeroType MyHeroType = MyHero.GetHeroType();

        int NumInvites = GetMyBattle().GetNumInvites();
        int NumPending = GetMyBattle().GetNumInvitesPending();
        int NumAccepted = GetMyBattle().GetNumInvitesAccepted();

        if (NumPending > 0)
        {
            SetInfoText("Waiting for invited heroes to respond...");
            DisableButton(StartBattleStartButton);
        }
        else if (NumAccepted == NumInvites && GetMyBattle().InvitationsWereSent())      // This will become true at the right time even for heroes fighting alone
        {
            SetInfoText("Starting battle...");
            DisableButton(StartBattleStartButton);
            DisableButton(StartBattleCancelButton);

            // If this is the battle starter, remove their battle draft reference
            if (MyHero == GetMyBattle().GetBattleStarter())
            {
                // NETWORKED
                if (PhotonNetwork.IsConnected) photonView.RPC("CleanupHeroBattleRPC", RpcTarget.All, MyHeroType);
                else CleanupHeroBattleRPC(MyHeroType);
            }

            this.Hide();
            BattleMenu.Show();
        }
    }

    private void UpdateIfCancelled()
    {
        if (GetMyBattle() == null) return;

        if (GetMyBattle().IsCancelled())
        {
            if (GetMyBattle().DeclinedBySomeone()) SetInfoText("This battle has been cancelled because an invited hero declined.");
            else SetInfoText("This battle has been cancelled.");
            DisableButton(StartBattleStartButton);
            StartBattleStartButton.SetActive(false);
            DisableButton(StartBattleCancelButton);
            StartBattleCancelButton.SetActive(false);
            StartBattleOkButton.SetActive(true);
        }
    }

    private void SetMyBattle(Battle MyBattle)
    {
        HeroType MyHeroType = GameManager.GetSelfHero().GetHeroType();

        SetHeroBattle(MyHeroType, MyBattle);
    }

    private Battle GetMyBattle()
    {
        HeroType MyHeroType = GameManager.GetSelfHero().GetHeroType();

        return GetHeroBattle(MyHeroType);
    }

    private void SetHeroBattle(HeroType Type, Battle HeroBattle)
    {
        if (Type == HeroType.Warrior) WarriorBattle = HeroBattle;
        else if (Type == HeroType.Archer) ArcherBattle = HeroBattle;
        else if (Type == HeroType.Dwarf) DwarfBattle = HeroBattle;
        else if (Type == HeroType.Wizard) WizardBattle = HeroBattle;
    }

    private Battle GetHeroBattle(HeroType Type)
    {
        if (Type == HeroType.Warrior) return WarriorBattle;
        else if (Type == HeroType.Archer) return ArcherBattle;
        else if (Type == HeroType.Dwarf) return DwarfBattle;
        else if (Type == HeroType.Wizard) return WizardBattle;
        else return null;
    }

    // Player-triggered action
    // Adds the target hero to the list of heroes to invite to the battle
    public void AddInvite(HeroType Type)
    {
        HeroType Inviter = GameManager.GetSelfHero().GetHeroType();

        // NETWORKED
        if (PhotonNetwork.IsConnected) photonView.RPC("AddBattleInviteRPC", RpcTarget.All, Inviter, Type);
        else AddBattleInviteRPC(Inviter, Type);
    }

    // Player-triggered action
    // Removes the target hero from the list of heroes to invite to the battle
    public void RemoveInvite(HeroType Type)
    {
        HeroType Inviter = GameManager.GetSelfHero().GetHeroType();

        // NETWORKED
        if (PhotonNetwork.IsConnected) photonView.RPC("RemoveBattleInviteRPC", RpcTarget.All, Inviter, Type);
        else RemoveBattleInviteRPC(Inviter, Type);
    }

    // Player-triggered action
    public void StartBattle()
    {
        HeroType Inviter = GameManager.GetSelfHero().GetHeroType();

        // Send inviations to any heroes that are invited (or none if fighting alone)
        // NETWORKED
        if (PhotonNetwork.IsConnected) photonView.RPC("SendBattleInvitesRPC", RpcTarget.All, Inviter);
        else SendBattleInvitesRPC(Inviter);

        UpdateWaitStatus();
        UpdateIfStarted();
    }

    // NETWORKED
    // Adds the specified Invitee to the list of heroes to invite to the battle being prepared by the Inviter
    [PunRPC]
    public void AddBattleInviteRPC(HeroType Inviter, HeroType Invitee)
    {
        // Get references to the involved heroes
        Hero TargetHero = HeroManager.GetHero(Invitee);

        // Get a reference to the inviter's battle
        Battle TargetBattle = GetHeroBattle(Inviter);

        TargetBattle.AddHeroToInvite(TargetHero);
    }

    // NETWORKED
    // Removes the specified Invitee from the list of heroes to invite to the battle being prepared by the Inviter
    [PunRPC]
    public void RemoveBattleInviteRPC(HeroType Inviter, HeroType Invitee)
    {
        // Get references to the involved heroes
        Hero TargetHero = HeroManager.GetHero(Invitee);

        // Get a reference to the inviter's battle
        Battle TargetBattle = GetHeroBattle(Inviter);

        TargetBattle.RemoveHeroToInvite(TargetHero);
    }

    // NETWORKED
    // Sends out all battle invites for the battle being prepared by the Inviter
    [PunRPC]
    public void SendBattleInvitesRPC(HeroType Inviter)
    {
        // Get a reference to the inviter's battle
        Battle TargetBattle = GetHeroBattle(Inviter);

        // Link the battle to the BattleMenu
        BattleMenu.SetBattle(TargetBattle);

        // Make the invitations visible to the concerned heroes
        TargetBattle.SendInvitations();
    }

    // NETWORKED
    // Creates a new battle to represent the battle being prepared by the specified hero against the specified creature
    [PunRPC]
    public void CreateNewHeroBattleRPC(HeroType Type, int CreatureRegionNum)
    {
        // Get the referenced hero and creature
        Hero TargetHero = HeroManager.GetHero(Type);
        Creature TargetCreature = WaypointManager.GetWaypoint(CreatureRegionNum).GetCreature();

        Battle NewBattle = new Battle(TargetHero, TargetCreature);

        SetHeroBattle(Type, NewBattle);
    }

    // NETWORKED
    // Cancels the battle being prepared by the specified hero
    [PunRPC]
    public void CancelHeroBattleRPC(HeroType Type)
    {
        Battle MyBattle = GetHeroBattle(Type);
        if (MyBattle != null) MyBattle.CancelStart();
    }

    // NETWORKED
    // Cleans up trailing references after cancelling the battle being prepared by the specified hero
    [PunRPC]
    public void CleanupHeroBattleRPC(HeroType Type)
    {
        // Delete the battle that was being created
        SetHeroBattle(Type, null);
    }
}
