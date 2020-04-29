using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroControlMenu : MonoBehaviour, Observer
{
    // Reference to GameManager
    private GameManager GameManager;

    // References to children components
    [SerializeField]
    private GameObject WarriorControlFrame = null;
    [SerializeField]
    private GameObject ArcherControlFrame = null;
    [SerializeField]
    private GameObject DwarfControlFrame = null;
    [SerializeField]
    private GameObject WizardControlFrame = null;
    [SerializeField]
    private GameObject WarriorControlIcon = null;
    [SerializeField]
    private GameObject ArcherControlIcon = null;
    [SerializeField]
    private GameObject DwarfControlIcon = null;
    [SerializeField]
    private GameObject WizardControlIcon = null;
    [SerializeField]
    private GameObject WarriorControlIconGrey = null;
    [SerializeField]
    private GameObject ArcherControlIconGrey = null;
    [SerializeField]
    private GameObject DwarfControlIconGrey = null;
    [SerializeField]
    private GameObject WizardControlIconGrey = null;

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
        UpdateControl();
        UpdateHeroIcons();
    }

    // Used in Observer design pattern
    public void UpdateData(string Category)
    {
        if (string.Equals(Category, "CONTROL"))
        {
            UpdateControl();
        }
        else if (string.Equals(Category, "PLAYING_HEROES"))
        {
            UpdateHeroIcons();
        }
    }

    // Updates the UI to show a frame around the hero that is being controlled
    private void UpdateControl()
    {
        Hero ControlledHero = GameManager.GetSelfHero();

        if (ControlledHero != null)
        {
            // Defaults
            WarriorControlFrame.SetActive(false);
            ArcherControlFrame.SetActive(false);
            DwarfControlFrame.SetActive(false);
            WizardControlFrame.SetActive(false);

            HeroType Type = ControlledHero.GetHeroType();

            if (Type == HeroType.Warrior) WarriorControlFrame.SetActive(true);
            else if (Type == HeroType.Archer) ArcherControlFrame.SetActive(true);
            else if (Type == HeroType.Dwarf) DwarfControlFrame.SetActive(true);
            else if (Type == HeroType.Wizard) WizardControlFrame.SetActive(true);
            else Debug.LogError("Cannot update control frames in HeroControlMenu; invalid hero type: " + Type);
        }
    }

    // Updates which icons are greyed out based on which heroes are in the game
    private void UpdateHeroIcons()
    {
        // Warrior
        bool Warrior = GameManager.IsPlaying(HeroType.Warrior);
        bool Archer = GameManager.IsPlaying(HeroType.Archer);
        bool Dwarf = GameManager.IsPlaying(HeroType.Dwarf);
        bool Wizard = GameManager.IsPlaying(HeroType.Wizard);

        WarriorControlIcon.SetActive(Warrior);
        WarriorControlIconGrey.SetActive(!Warrior);

        ArcherControlIcon.SetActive(Archer);
        ArcherControlIconGrey.SetActive(!Archer);

        DwarfControlIcon.SetActive(Dwarf);
        DwarfControlIconGrey.SetActive(!Dwarf);

        WizardControlIcon.SetActive(Wizard);
        WizardControlIconGrey.SetActive(!Wizard);
    }

    // Player-triggered action
    public void ControlWarrior()
    {
        ControlHero(HeroType.Warrior);
    }

    // Player-triggered action
    public void ControlArcher()
    {
        ControlHero(HeroType.Archer);
    }

    // Player-triggered action
    public void ControlDwarf()
    {
        ControlHero(HeroType.Dwarf);
    }

    // Player-triggered action
    public void ControlWizard()
    {
        ControlHero(HeroType.Wizard);
    }

    // Player-triggered action
    public void TogglePlayingWarrior()
    {
        ToggleIsPlaying(HeroType.Warrior);
    }

    // Player-triggered action
    public void TogglePlayingArcher()
    {
        ToggleIsPlaying(HeroType.Archer);
    }

    // Player-triggered action
    public void TogglePlayingDwarf()
    {
        ToggleIsPlaying(HeroType.Dwarf);
    }

    // Player-triggered action
    public void TogglePlayingWizard()
    {
        ToggleIsPlaying(HeroType.Wizard);
    }

    public void ControlHero(HeroType Type)
    {
        GameManager.SetSelfPlayer(Type);
    }

    public void ToggleIsPlaying(HeroType Type)
    {
        GameManager.ToggleIsPlayingForAll(Type);
    }
}
