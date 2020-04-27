using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

// Menu used during a battle against a creature
public class BattleMenu : MonoBehaviourPun, Observer
{
    // References to managers
    private GameManager GameManager;
    private HeroManager HeroManager;
    private CreatureManager CreatureManager;

    // Battle object
    Battle Battle;

    // Controls the UI when the wizard is in the process of flipping a die
    bool CurrentlyFlippingDie = false;

    // Colours of the hero dice
    Color32 White = new Color32(255, 255, 255, 255);
    Color32 WarriorDiceColour = new Color32(71, 170, 217, 255);
    Color32 ArcherDiceColour = new Color32(165, 206, 60, 255);
    Color32 DwarfDiceColour = new Color32(255, 191, 55, 255);
    Color32 WizardDiceColour = new Color32(156, 101, 170, 255);

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

    // Turn markers
    [SerializeField]
    GameObject WarriorTurnMarker = null;
    [SerializeField]
    GameObject ArcherTurnMarker = null;
    [SerializeField]
    GameObject DwarfTurnMarker = null;
    [SerializeField]
    GameObject WizardTurnMarker = null;

    // Creature icons in the creature box (must choose one to show)
    [SerializeField]
    GameObject GorBattleIcon = null;
    [SerializeField]
    GameObject HerbGorBattleIcon = null;
    [SerializeField]
    GameObject SkralBattleIcon = null;
    [SerializeField]
    GameObject TowerSkralBattleIcon = null;
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
    [SerializeField]
    GameObject WarriorRoll = null;
    [SerializeField]
    GameObject ArcherRoll = null;
    [SerializeField]
    GameObject DwarfRoll = null;
    [SerializeField]
    GameObject WizardRoll = null;
    [SerializeField]
    GameObject CreatureRoll = null;

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
    [SerializeField]
    GameObject HeroBattleValue = null;
    [SerializeField]
    GameObject CreatureBattleValue = null;
    [SerializeField]
    GameObject BattleValueOperator = null;
    [SerializeField]
    GameObject HeroWillpowerLoss = null;
    [SerializeField]
    GameObject CreatureWillpowerLoss = null;

    // Hero dice
    [SerializeField]
    GameObject[] HeroDice = null;

    // Creature dice
    [SerializeField]
    GameObject[] CreatureDice = null;

    // Highlight box around the hero dice that is shown when flipping dice
    [SerializeField]
    GameObject FlipDieHighlight = null;

    // Info text and buttons
    [SerializeField]
    GameObject BattleInfoText = null;
    [SerializeField]
    GameObject RoundNumText = null;
    [SerializeField]
    GameObject BattleRollButton = null;
    [SerializeField]
    GameObject BattleNextButton = null;
    [SerializeField]
    GameObject BattleFlipDieButton = null;
    [SerializeField]
    GameObject BattleOkButton = null;
    [SerializeField]
    GameObject BattleLeaveButton = null;

    // Waiting spinner
    [SerializeField]
    GameObject Spinner = null;

    // Dice images
    Sprite[] DiceSides;
    Sprite[] DiceSidesBlack;

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

        // Load the dice sprites
        DiceSides = Resources.LoadAll<Sprite>("Board/Dice/");
        DiceSidesBlack = Resources.LoadAll<Sprite>("Board/BlackDice/");

        // Register as an observer of GameManager
        GameManager.Attach(this);
    }

    // To function correctly, the battle menu must be displayed using this method
    public void Show()
    {
        // If the battle hasn't been set, nothing happens
        if (Battle == null)
        {
            Debug.LogWarning("The battle menu can't be shown because no battle was set.");
            return;
        }

        // If the battle hasn't been started, nothing happens
        if (Battle.IsPending()) return;

        this.gameObject.SetActive(true);

        InitializeUI();
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Toggle()
    {
        if (this.gameObject.activeSelf == false) Show();
        else Hide();
    }

    // Used in Observer design pattern
    public void UpdateData(string Category)
    {
        if (string.Equals(Category, "TURN"))
        {
            
        }
        else if (string.Equals(Category, "BATTLE_TURN"))
        {
            UpdateBattleTurn();
            UpdateBattleRoll();
        }
        else if (string.Equals(Category, "ROLL"))
        {
            UpdateBattleRoll();
        }
        else if (string.Equals(Category, "WILLPOWER"))
        {
            UpdateHeroInfo();
            UpdateCreatureInfo();
            UpdateBattleRoll();     // To disable the leave button when a hero hits 0 willpower
        }
        else if (string.Equals(Category, "BATTLE_WON"))
        {
            UpdateWon();
        }
        else if (string.Equals(Category, "BATTLE_LOST"))
        {
            UpdateLost();
        }
        else if (string.Equals(Category, "BATTLE_PARTICIPANTS"))
        {
            UpdateHeroBoxes();
            UpdateButtonDisplay();
        }
        else if (string.Equals(Category, "BATTLE_CANCELLED"))
        {
            UpdateIfCancelled();
        }
        else if (string.Equals(Category, "CONTROL"))
        {
            // When changing the controlled hero, check whether the hero already has an ongoing battle
            // If yes, show it. If no, don't show this menu.

            Hero MyHero = GameManager.GetSelfHero();

            // If the wizard had started flipping a die, cancel this process
            CurrentlyFlippingDie = false;

            if (Battle != null && Battle.IsParticipating(MyHero) && Battle.IsStarted()) this.Show();
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

        HideButton(BattleOkButton);
        Spinner.SetActive(false);

        UpdateHeroBoxes();
        UpdateMainCreature();
        UpdateHeroInfo();
        UpdateCreatureInfo();
        UpdateBattleTurn();
        UpdateBattleRoll();
        UpdateWon();
        UpdateLost();
        UpdateIfCancelled();
        UpdateButtonDisplay();
    }

    private Color32 GetDiceColour(HeroType Type)
    {
        if (Type == HeroType.Warrior) return WarriorDiceColour;
        else if (Type == HeroType.Archer) return ArcherDiceColour;
        else if (Type == HeroType.Dwarf) return DwarfDiceColour;
        else if (Type == HeroType.Wizard) return WizardDiceColour;
        else return White;
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
        HerbGorBattleIcon.SetActive(false);
        SkralBattleIcon.SetActive(false);
        TowerSkralBattleIcon.SetActive(false);
        WardrakBattleIcon.SetActive(false);

        if (MainCreature != null)
        {
            CreatureType Type = MainCreature.GetCreatureType();

            if      (Type == CreatureType.Gor) GorBattleIcon.SetActive(true);
            else if (Type == CreatureType.HerbGor) HerbGorBattleIcon.SetActive(true);
            else if (Type == CreatureType.Skral) SkralBattleIcon.SetActive(true);
            else if (Type == CreatureType.TowerSkral) TowerSkralBattleIcon.SetActive(true);
            else if (Type == CreatureType.Wardrak) WardrakBattleIcon.SetActive(true);
        }
    }

    // If the current hero is no longer a battle participant, hide all buttons
    private void UpdateButtonDisplay()
    {
        // Current hero
        Hero MyHero = GameManager.GetSelfHero();

        if (Battle.GetParticipants().IndexOf(MyHero) == -1) HideAllButtons();
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

    private void UpdateBattleTurn()
    {
        // Defaults
        WarriorTurnMarker.SetActive(false);
        ArcherTurnMarker.SetActive(false);
        DwarfTurnMarker.SetActive(false);
        WizardTurnMarker.SetActive(false);

        // Get whose turn it is
        Hero TurnHolder = Battle.GetTurnHolder();

        if (TurnHolder != null)
        {
            HeroType Type = TurnHolder.GetHeroType();

            if (Type == HeroType.Warrior) WarriorTurnMarker.SetActive(true);
            else if (Type == HeroType.Archer) ArcherTurnMarker.SetActive(true);
            else if (Type == HeroType.Dwarf) DwarfTurnMarker.SetActive(true);
            else if (Type == HeroType.Wizard) WizardTurnMarker.SetActive(true);
        }

        // Show the round #
        int RoundNum = Battle.GetRoundNum();
        SetText(RoundNumText, "Round #" + RoundNum);
    }

    private void UpdateBattleRoll()
    {
        // Defaults
        HideButton(BattleRollButton);
        HideButton(BattleNextButton);
        HideButton(BattleLeaveButton);
        HideButton(BattleFlipDieButton);
        DisableButton(BattleRollButton);
        DisableButton(BattleNextButton);
        DisableButton(BattleLeaveButton);
        DisableButton(BattleFlipDieButton);
        SetText(HeroBattleValue, "");
        SetText(CreatureBattleValue, "");
        SetText(BattleValueOperator, "");
        SetText(HeroWillpowerLoss, "");
        SetText(CreatureWillpowerLoss, "");
        Spinner.SetActive(false);

        // Current hero
        Hero MyHero = GameManager.GetSelfHero();
        HeroType MyHeroType = MyHero.GetHeroType();

        // Turn holder
        Hero TurnHolder = Battle.GetTurnHolder();
        HeroType TurnHolderType = TurnHolder.GetHeroType();

        // Check if the current hero is the turn holder and can potentially roll
        if (MyHero == TurnHolder)
        {
            // Display roll-related buttons
            ShowButton(BattleRollButton);
            ShowButton(BattleNextButton);

            // If the hero hasn't finished rolling, enable the button to do so
            if (!Battle.HasFinishedRoll(MyHero))
            {
                EnableButton(BattleRollButton);
            }

            // Check if the current turn holder has started their roll (and can click next to finish it)
            if (Battle.HasStartedRoll(MyHero))
            {
                EnableButton(BattleNextButton);
            }
        }

        UpdateWizardFlipDie();

        // If the battle round is finished, give heroes the opportunity to click next (to start a new round) or leave the battle
        if (Battle.CreatureHasRolled() && !Battle.IsFinished())
        {
            ShowButton(BattleNextButton);
            EnableButton(BattleNextButton);
            ShowButton(BattleLeaveButton);
            if (MyHero.getWillpower() > 0) EnableButton(BattleLeaveButton);         // If their willpower is 0, the hero cannot leave (and skip strength deduction / willpower gain)
        }

        // If the hero has consented to continue but is waiting on others to do so, disable the relevant buttons and show a spinner
        if (Battle.HasConsentedToContinue(MyHero))
        {
            DisableButton(BattleNextButton);
            DisableButton(BattleLeaveButton);
            Spinner.SetActive(true);
        }

        // Display the current hero roll results
        SetText(WarriorRoll, Battle.GetLatestRollValue(HeroManager.GetHero(HeroType.Warrior)).ToString());
        SetText(ArcherRoll, Battle.GetLatestRollValue(HeroManager.GetHero(HeroType.Archer)).ToString());
        SetText(DwarfRoll, Battle.GetLatestRollValue(HeroManager.GetHero(HeroType.Dwarf)).ToString());
        SetText(WizardRoll, Battle.GetLatestRollValue(HeroManager.GetHero(HeroType.Wizard)).ToString());

        // Display the hero dice
        int[] DiceValues = Battle.GetLatestHeroRollValues();
        DiceType Type = Battle.GetLatestHeroRollDiceType();

        // Choose the correct image set based on the DiceType
        Sprite[] MyDiceSides;
        if (Type == DiceType.Regular) MyDiceSides = DiceSides;
        else MyDiceSides = DiceSidesBlack;
        
        int i;

        // Set the correct dice images for rolled dice for heroes
        for (i = 0; i < DiceValues.Length; i++)
        {
            Image Renderer = HeroDice[i].GetComponent<Image>();
            
            int DieIndex = -1;

            if (Type == DiceType.Regular) DieIndex = DiceValues[i];
            else if (Type == DiceType.Black)
            {
                     if (DiceValues[i] == 0) DieIndex = 0;
                else if (DiceValues[i] == 6) DieIndex = 1;
                else if (DiceValues[i] == 8) DieIndex = 2;
                else if (DiceValues[i] == 10) DieIndex = 3;
                else if (DiceValues[i] == 12) DieIndex = 4;
            }

            if (DieIndex == -1) Debug.LogError("Could not find sprite for " + Type + " die with value " + DiceValues[i]);
            else
            {
                HeroDice[i].SetActive(true);
                Renderer.sprite = MyDiceSides[DieIndex];
                Renderer.color = GetDiceColour(TurnHolder.GetHeroType());
            }
        }

        // Continue iterating through the remaining die icons for heroes and leave them blank (if available for rolling) or remove their images (if not)
        for (; i < HeroDice.Length; i++)
        {
            Image Renderer = HeroDice[i].GetComponent<Image>();

            if (i < TurnHolder.GetNumOfDice())
            {
                HeroDice[i].SetActive(true);
                Renderer.sprite = MyDiceSides[0];
                Renderer.color = GetDiceColour(TurnHolder.GetHeroType());
            }
            else HeroDice[i].SetActive(false);
        }

        // Display the current creature roll results
        SetText(CreatureRoll, Battle.GetLatestCreatureRollValue().ToString());

        // Display the creature dice
        DiceValues = Battle.GetLatestCreatureRollValues();
        Type = Battle.GetLatestCreatureRollDiceType();

        // Choose the correct image set based on the DiceType
        if (Type == DiceType.Regular) MyDiceSides = DiceSides;
        else if (Type == DiceType.Black) MyDiceSides = DiceSidesBlack;

        // Set the correct dice images for rolled dice for creatures
        for (i = 0; i < DiceValues.Length; i++)
        {
            Image Renderer = CreatureDice[i].GetComponent<Image>();

            int DieIndex = -1;

            if (Type == DiceType.Regular) DieIndex = DiceValues[i];
            else if (Type == DiceType.Black)
            {
                     if (DiceValues[i] == 0) DieIndex = 0;
                else if (DiceValues[i] == 6) DieIndex = 1;
                else if (DiceValues[i] == 8) DieIndex = 2;
                else if (DiceValues[i] == 10) DieIndex = 3;
                else if (DiceValues[i] == 12) DieIndex = 4;
            }

            if (DieIndex == -1) Debug.LogError("Could not find sprite for " + Type + " die with value " + DiceValues[i]);
            else
            {
                CreatureDice[i].SetActive(true);
                Renderer.sprite = MyDiceSides[DieIndex];
            }
        }

        // Continue iterating through the remaining die icons for creatures and leave them blank (if available for rolling) or remove their images (if not)
        for (; i < CreatureDice.Length; i++)
        {
            Image Renderer = CreatureDice[i].GetComponent<Image>();

            if (i < Battle.GetCreature().GetNumOfDice())
            {
                CreatureDice[i].SetActive(true);
                Renderer.sprite = MyDiceSides[0];
            }
            else CreatureDice[i].SetActive(false);
        }

        // Display the hero battle value
        int HeroBV = Battle.GetLatestHeroBattleValue();
        if (HeroBV != 0) SetText(HeroBattleValue, HeroBV.ToString());

        // Display the creature battle value
        int CreatureBV = Battle.GetLatestCreatureBattleValue();
        if (CreatureBV != 0) SetText(CreatureBattleValue, CreatureBV.ToString());

        // Check whether the round is done
        if (Battle.RoundIsDone())
        {
            // Display the battle value comparator
            string Comparator = "";
            if (HeroBV == CreatureBV) Comparator = "=";
            else if (HeroBV < CreatureBV) Comparator = "<";
            else if (HeroBV > CreatureBV) Comparator = ">";
            if (HeroBV != 0 && CreatureBV != 0) SetText(BattleValueOperator, Comparator);

            // If the round is done, display the lost willpower
            int HeroLostWP = Battle.GetHeroLostWillpower();
            int CreatureLostWP = Battle.GetCreatureLostWillpower();

            if (HeroLostWP > 0) SetText(HeroWillpowerLoss, "[-" + HeroLostWP.ToString() + "]");
            if (CreatureLostWP > 0) SetText(CreatureWillpowerLoss, "[-" + CreatureLostWP.ToString() + "]");
        }
    }

    private void UpdateWon()
    {
        if (Battle.IsWon())
        {
            HideAllButtons();
            DisableAllButtons();
            SetInfoText("The battle has been won!");
            BattleOkButton.SetActive(true);
            EnableButton(BattleOkButton);
        }
    }
    
    private void UpdateLost()
    {
        if (Battle.IsLost())
        {
            HideAllButtons();
            DisableAllButtons();
            SetInfoText("The battle has been lost.");
            BattleOkButton.SetActive(true);
            EnableButton(BattleOkButton);
        }
    }

    private void UpdateIfCancelled()
    {
        if (Battle.IsCancelled())
        {
            HideAllButtons();
            DisableAllButtons();
            SetInfoText("The battle has been cancelled because all heroes left.");
            BattleOkButton.SetActive(true);
            EnableButton(BattleOkButton);
        }
    }
    
    // Shows the given button, but only if the current hero is a battle participant
    private void ShowButton(GameObject Button)
    {
        // Current hero
        Hero MyHero = GameManager.GetSelfHero();

        if (Battle.GetParticipants().IndexOf(MyHero) != -1) Button.SetActive(true);
    }

    // Hides the given button
    private void HideButton(GameObject Button)
    {
        Button.SetActive(false);
    }

    private void HideAllButtons()
    {
        HideButton(BattleRollButton);
        HideButton(BattleNextButton);
        HideButton(BattleFlipDieButton);
        HideButton(BattleOkButton);
        HideButton(BattleLeaveButton);
    }

    private void DisableAllButtons()
    {
        DisableButton(BattleRollButton);
        DisableButton(BattleNextButton);
        DisableButton(BattleFlipDieButton);
        DisableButton(BattleOkButton);
        DisableButton(BattleLeaveButton);
    }

    // Player-triggered action
    public void RollDice()
    {
        // Current hero
        Hero MyHero = GameManager.GetSelfHero();
        HeroType MyHeroType = MyHero.GetHeroType();

        // Launch the roll for the current hero
        bool IsNewRoll = Battle.Roll(MyHero);

        // Extract the features of the roll
        Roll MyRoll = Battle.GetRoll(MyHero);
        DiceType DiceType = MyRoll.GetDiceType();
        int NumOfDice = MyRoll.GetNumOfDice();
        bool BowOrArcherRoll = MyRoll.GetBowOrArcherRoll();
        bool HelmOrCreatureRoll = MyRoll.GetHelmOrCreatureRoll();
        int[] RollValues = MyRoll.GetValues();

        // Send the roll to the other players
        // NETWORKED
        if(PhotonNetwork.IsConnected)
        {
            if (IsNewRoll) photonView.RPC("SendNewHeroRollRPC", RpcTarget.All, MyHeroType, DiceType, NumOfDice, BowOrArcherRoll, HelmOrCreatureRoll, RollValues);
            else photonView.RPC("SendHeroRollValuesRPC", RpcTarget.All, MyHeroType, RollValues);
        }
    }

    // Player-triggered action
    // Advances the turn in the battle (either within a round, or by moving to the next round)
    public void Next()
    {
        // Current hero
        Hero MyHero = GameManager.GetSelfHero();
        HeroType MyHeroType = MyHero.GetHeroType();

        // Finalize the hero roll (useful for the archer)
        // NETWORKED
        if (PhotonNetwork.IsConnected) photonView.RPC("FinalizeRollRPC", RpcTarget.All, MyHeroType);
        else FinalizeRollRPC(MyHeroType);

        // Cancel flipping the die (useful for the wizard's UI, does nothing for the other heroes)
        CurrentlyFlippingDie = false;
        UpdateWizardFlipDie();

        // Clear the info text
        SetInfoText("");

        // If the round is done (for heroes), but the battle is not finished, check whether the creature is next to roll
        if (Battle.RoundIsDoneForHeroes() && !Battle.IsFinished())
        {
            // Check whether the creature has rolled
            if (!Battle.CreatureHasRolled())
            {
                // If not, let the creature roll
                Battle.CreatureRoll();

                // Extract the features of the roll
                Roll CreatureRoll = Battle.GetLatestCreatureRoll();
                DiceType DiceType = CreatureRoll.GetDiceType();
                int NumOfDice = CreatureRoll.GetNumOfDice();
                bool BowOrArcherRoll = CreatureRoll.GetBowOrArcherRoll();
                bool HelmOrCreatureRoll = CreatureRoll.GetHelmOrCreatureRoll();
                int[] RollValues = CreatureRoll.GetValues();

                // Send the roll to the other players
                // NETWORKED
                if (PhotonNetwork.IsConnected) photonView.RPC("SendNewCreatureRollRPC", RpcTarget.All, MyHeroType, DiceType, NumOfDice, BowOrArcherRoll, HelmOrCreatureRoll, RollValues);

                // Trigger taking damage on all machines
                // NETWORKED
                if (PhotonNetwork.IsConnected) photonView.RPC("TakeDamageRPC", RpcTarget.All);
                else TakeDamageRPC();
            }
            // If the creature has already rolled, go to the next round
            else
            {
                // Express agreement to go to the next round
                // NETWORKED
                if (PhotonNetwork.IsConnected) photonView.RPC("GoToNextRoundRPC", RpcTarget.All, MyHeroType);
                else GoToNextRoundRPC(MyHeroType);
            }
        }
        // If the round is not done (for heroes), go to the next turn
        else
        {
            // Go to the next turn
            // NETWORKED
            if (PhotonNetwork.IsConnected) photonView.RPC("GoToNextTurnRPC", RpcTarget.All);
            else GoToNextTurnRPC();
        }
    }

    // Player-triggered action
    public void ExpressIntentToFlipDie()
    {
        // If the hero was about to flip the die, cancel this process
        if (CurrentlyFlippingDie)
        {
            CurrentlyFlippingDie = false;
            SetInfoText("");
        }
        else
        {
            CurrentlyFlippingDie = true;
            SetInfoText("Select a die to flip it, or press the same button again to cancel.");
        }
        UpdateWizardFlipDie();
    }

    // Updates the UI based on whether the wizard is allowed to flip a die
    private void UpdateWizardFlipDie()
    {
        // Current hero
        Hero MyHero = GameManager.GetSelfHero();
        HeroType MyHeroType = MyHero.GetHeroType();

        // Check if the wizard has flipped a die this round
        if (MyHeroType == HeroType.Wizard)
        {
            ShowButton(BattleFlipDieButton);
            if (Battle.WizardCanFlipDie()) EnableButton(BattleFlipDieButton);
        }

        // Display or hide the flip die highlight
        if (CurrentlyFlippingDie) FlipDieHighlight.SetActive(true);
        else FlipDieHighlight.SetActive(false);
    }

    // Player-triggered action
    public void FlipDie(int Index)
    {
        // Verify that the hero has begun the process of flipping the die using the flip button
        if (CurrentlyFlippingDie)
        {
            // Identify the hero whose die to flip
            HeroType HeroFlipTarget = Battle.GetTurnHolder().GetHeroType();

            // Determine if the die can be flipped (has been rolled)
            int[] DiceValues = Battle.GetLatestHeroRollValues();
            bool CanFlip = (Index < DiceValues.Length) && (DiceValues[Index] != 0);

            if (CanFlip)
            {
                // Flip the chosen die
                // NETWORKED
                if (PhotonNetwork.IsConnected) photonView.RPC("FlipDieRPC", RpcTarget.All, HeroFlipTarget, Index);
                else FlipDieRPC(HeroFlipTarget, Index);

                // Reset the flip UI
                SetInfoText("");
            }
            else
            {
                SetInfoText("You cannot flip an unrolled die.");
            }

            CurrentlyFlippingDie = false;
            UpdateWizardFlipDie();
        }
    }

    // Player-triggered action
    public void LeaveBattle()
    {
        // Current hero
        Hero MyHero = GameManager.GetSelfHero();
        HeroType MyHeroType = MyHero.GetHeroType();

        // Remove the hero from the battle
        // NETWORKED
        if (PhotonNetwork.IsConnected) photonView.RPC("LeaveBattleRPC", RpcTarget.All, MyHeroType);
        else LeaveBattleRPC(MyHeroType);
        
    }

    // NETWORKED
    // Sends parameters to the other machines to replicate a new roll made by a player.
    [PunRPC]
    public void SendNewHeroRollRPC(HeroType Roller, DiceType DiceType, int NumOfDice, bool BowOrArcherRoll, bool HelmOrCreatureRoll, int[] RollValues)
    {
        // Get a reference to the roller
        Hero RollerHero = HeroManager.GetHero(Roller);

        // Don't make a copy if this is the source machine
        if (RollerHero != GameManager.GetSelfHero())
        {
            // Create a roll object based on the input parameters (to mimic the original roll on the roller's machine)
            Roll MimicRoll = Roll.NewMimicRoll(DiceType, NumOfDice, BowOrArcherRoll, HelmOrCreatureRoll, RollValues);

            // Set the mimic roll as the hero's roll
            Battle.SetHeroRoll(RollerHero, MimicRoll);
        }
    }

    // NETWORKED
    // Sends parameters to the other machines to replicate a new roll made by a creature.
    [PunRPC]
    public void SendNewCreatureRollRPC(HeroType Sender, DiceType DiceType, int NumOfDice, bool BowOrArcherRoll, bool HelmOrCreatureRoll, int[] RollValues)
    {
        // Get a reference to the sender
        Hero SenderHero = HeroManager.GetHero(Sender);

        // Don't make a copy if this is the source machine
        if (SenderHero != GameManager.GetSelfHero())
        {
            // Create a roll object based on the input parameters (to mimic the original roll on the sender's machine)
            Roll MimicRoll = Roll.NewMimicRoll(DiceType, NumOfDice, BowOrArcherRoll, HelmOrCreatureRoll, RollValues);

            // Set the mimic roll as the creature's roll
            Battle.SetCreatureRoll(MimicRoll);
        }
    }

    // NETWORKED
    // Instructs all machines to trigger taking damage (this cascades into calculating the battle outcome if necessary)
    [PunRPC]
    public void TakeDamageRPC()
    {
        Battle.TakeDamage();
    }

    // NETWORKED
    [PunRPC]
    public void SendHeroRollValuesRPC(HeroType Roller, int[] RollValues)
    {
        // Get a reference to the roller
        Hero RollerHero = HeroManager.GetHero(Roller);

        // Don't update the roll if this is the source machine
        if (RollerHero != GameManager.GetSelfHero())
        {
            Battle.UpdateRollValues(RollerHero, RollValues);
        }
    }

    // NETWORKED
    // Finalizes the current roll associated to the given hero
    [PunRPC]
    public void FinalizeRollRPC(HeroType Roller)
    {
        // Get a reference to the roller
        Hero RollerHero = HeroManager.GetHero(Roller);

        // Finalize the roll
        Battle.FinalizeRoll(RollerHero);
    }

    // NETWORKED
    // Moves the battle to the next round
    [PunRPC]
    public void GoToNextRoundRPC(HeroType MyHeroType)
    {
        // Get a reference to the hero
        Hero MyHero = HeroManager.GetHero(MyHeroType);

        Battle.ExpressConsentToContinue(MyHero);
    }

    // NETWORKED
    // Moves the battle to the next turn
    [PunRPC]
    public void GoToNextTurnRPC()
    {
        Battle.GoToNextTurn();
    }

    // NETWORKED
    // Flips the die at the given index for the indicated player's roll
    [PunRPC]
    public void FlipDieRPC(HeroType FlipTarget, int DieIndex)
    {
        // Get a reference to the hero
        Hero FlipTargetHero = HeroManager.GetHero(FlipTarget);

        // Flip the die
        Battle.FlipDie(FlipTargetHero, DieIndex);
    }

    // NETWORKED
    // Removes the specified hero from the battle
    [PunRPC]
    public void LeaveBattleRPC(HeroType Target)
    {
        // Get a reference to the hero
        Hero TargetHero = HeroManager.GetHero(Target);

        // Leave the battle
        Battle.LeaveHero(TargetHero);
    }
}
