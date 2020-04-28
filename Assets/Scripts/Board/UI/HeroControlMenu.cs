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
    private HeroControlIcon WarriorControlIcon = null;
    [SerializeField]
    private HeroControlIcon ArcherControlIcon = null;
    [SerializeField]
    private HeroControlIcon DwarfControlIcon = null;
    [SerializeField]
    private HeroControlIcon WizardControlIcon = null;

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

        // Initialize children
        WarriorControlIcon.Initialize(HeroType.Warrior);
        ArcherControlIcon.Initialize(HeroType.Archer);
        DwarfControlIcon.Initialize(HeroType.Dwarf);
        WizardControlIcon.Initialize(HeroType.Wizard);

        // Initialize UI
        UpdateControl();
    }

    // Used in Observer design pattern
    public void UpdateData(string Category)
    {
        if (string.Equals(Category, "CONTROL"))
        {
            UpdateControl();
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
}
