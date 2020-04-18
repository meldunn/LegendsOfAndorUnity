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
    //[SerializeField]
    //GameObject CreatureBox = null;

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

    // Info text and buttons
    [SerializeField]
    GameObject BattleInfoText = null;
    [SerializeField]
    GameObject BattleRollButton = null;
    [SerializeField]
    GameObject BattleNextButton = null;
    [SerializeField]
    GameObject BattleFlipDieButton = null;
    [SerializeField]
    GameObject BattleOkButton = null;

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
            Debug.LogError("The battle menu can't be shown because no battle was set.");
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

        BattleOkButton.SetActive(false);

        UpdateHeroBoxes();
        UpdateMainCreature();
        UpdateHeroInfo();
        UpdateCreatureInfo();
        UpdateBattleTurn();
        UpdateBattleRoll();
        UpdateWon();
        UpdateLost();
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
        HeroType Type = TurnHolder.GetHeroType();

        if (Type == HeroType.Warrior) WarriorTurnMarker.SetActive(true);
        else if (Type == HeroType.Archer) ArcherTurnMarker.SetActive(true);
        else if (Type == HeroType.Dwarf) DwarfTurnMarker.SetActive(true);
        else if (Type == HeroType.Wizard) WizardTurnMarker.SetActive(true);
    }

    private void UpdateBattleRoll()
    {
        // Defaults
        BattleRollButton.SetActive(false);
        BattleNextButton.SetActive(false);
        BattleFlipDieButton.SetActive(false);
        DisableButton(BattleRollButton);
        DisableButton(BattleNextButton);
        DisableButton(BattleFlipDieButton);
        SetText(HeroBattleValue, "");
        SetText(CreatureBattleValue, "");
        SetText(BattleValueOperator, "");
        SetText(HeroWillpowerLoss, "");
        SetText(CreatureWillpowerLoss, "");

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
            BattleRollButton.SetActive(true);
            BattleNextButton.SetActive(true);

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

        // Check if the wizard has flipped a die this round
        if (MyHeroType == HeroType.Wizard)
        {
            BattleFlipDieButton.SetActive(true);
            if (Battle.WizardCanFlipDie()) EnableButton(BattleFlipDieButton);
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
                     if (DiceValues[i] == 6) DieIndex = 1;
                else if (DiceValues[i] == 8) DieIndex = 2;
                else if (DiceValues[i] == 10) DieIndex = 3;
                else if (DiceValues[i] == 12) DieIndex = 4;
            }

            if (DieIndex == -1) Debug.LogError("Could not find sprite for " + Type + " die with value " + DiceValues[i]);
            else
            {
                HeroDice[i].SetActive(true);
                Renderer.sprite = MyDiceSides[DieIndex];
                if (Type == DiceType.Regular) Renderer.color = GetDiceColour(TurnHolder.GetHeroType());
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
                if (Type == DiceType.Regular) Renderer.color = GetDiceColour(TurnHolder.GetHeroType());
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
                if (DiceValues[i] == 6) DieIndex = 1;
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

        // Check whether the round is done
        if (Battle.RoundIsDone())
        {
            // If the round is done, display the battle values
            int HeroBV = Battle.GetLatestHeroBattleValue();
            int CreatureBV = Battle.GetLatestCreatureBattleValue();

            if (HeroBV != 0) SetText(HeroBattleValue, HeroBV.ToString());
            if (CreatureBV != 0) SetText(CreatureBattleValue, CreatureBV.ToString());

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
            BattleRollButton.SetActive(false);
            BattleNextButton.SetActive(false);
            BattleFlipDieButton.SetActive(false);
            DisableButton(BattleRollButton);
            DisableButton(BattleNextButton);
            DisableButton(BattleFlipDieButton);
            SetInfoText("The battle has been won!");
            BattleOkButton.SetActive(true);
        }
    }
    
    private void UpdateLost()
    {
        if (Battle.IsLost())
        {
            BattleRollButton.SetActive(false);
            BattleNextButton.SetActive(false);
            BattleFlipDieButton.SetActive(false);
            DisableButton(BattleRollButton);
            DisableButton(BattleNextButton);
            DisableButton(BattleFlipDieButton);
            SetInfoText("The battle has been lost.");
            BattleOkButton.SetActive(true);
        }
    }

    // Player-triggered action
    public void Roll()
    {
        // Current hero
        Hero MyHero = GameManager.GetSelfHero();

        // Launch the roll for the current hero
        Battle.Roll(MyHero);
    }

    // Player-triggered action
    public void Next()
    {
        Battle.Next();
    }
    
    // Player-triggered action
    public void FlipDie()
    {
        SetInfoText("Flipping dice has not been implemented yet.");
    }
}
