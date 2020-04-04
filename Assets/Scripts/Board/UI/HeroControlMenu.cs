using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroControlMenu : MonoBehaviour, Observer
{
    // Reference to GameManager
    private GameManager GameManager;

    // References to children components
    GameObject WarriorControlFrame;
    GameObject ArcherControlFrame;
    GameObject DwarfControlFrame;
    GameObject WizardControlFrame;
    HeroControlIcon WarriorControlIcon;
    HeroControlIcon ArcherControlIcon;
    HeroControlIcon DwarfControlIcon;
    HeroControlIcon WizardControlIcon;

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

        // Initialize references to children components
        WarriorControlFrame = GameObject.Find("WarriorControlFrame");
        ArcherControlFrame = GameObject.Find("ArcherControlFrame");
        DwarfControlFrame = GameObject.Find("DwarfControlFrame");
        WizardControlFrame = GameObject.Find("WizardControlFrame");
        WarriorControlIcon = GameObject.Find("WarriorControlIcon").GetComponent<HeroControlIcon>();
        ArcherControlIcon = GameObject.Find("ArcherControlIcon").GetComponent<HeroControlIcon>();
        DwarfControlIcon = GameObject.Find("DwarfControlIcon").GetComponent<HeroControlIcon>();
        WizardControlIcon = GameObject.Find("WizardControlIcon").GetComponent<HeroControlIcon>();

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
            HeroType Type = ControlledHero.GetHeroType();

            switch (Type)
            {
                case HeroType.Warrior:
                    WarriorControlFrame.SetActive(true);
                    ArcherControlFrame.SetActive(false);
                    DwarfControlFrame.SetActive(false);
                    WizardControlFrame.SetActive(false);
                    break;

                case HeroType.Archer:
                    WarriorControlFrame.SetActive(false);
                    ArcherControlFrame.SetActive(true);
                    DwarfControlFrame.SetActive(false);
                    WizardControlFrame.SetActive(false);
                    break;

                case HeroType.Dwarf:
                    WarriorControlFrame.SetActive(false);
                    ArcherControlFrame.SetActive(false);
                    DwarfControlFrame.SetActive(true);
                    WizardControlFrame.SetActive(false);
                    break;

                case HeroType.Wizard:
                    WarriorControlFrame.SetActive(false);
                    ArcherControlFrame.SetActive(false);
                    DwarfControlFrame.SetActive(false);
                    WizardControlFrame.SetActive(true);
                    break;

                default:
                    Debug.LogError("Cannot update control frames in HeroControlMenu; invalid hero type: " + Type);
                    break;
            }
        }
    }
}
