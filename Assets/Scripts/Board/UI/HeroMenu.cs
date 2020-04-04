using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMenu : MonoBehaviour, Observer
{
    // Reference to GameManager
    private GameManager GameManager;

    // References to children components
    GameObject WarriorTurnText;
    GameObject ArcherTurnText;
    GameObject DwarfTurnText;
    GameObject WizardTurnText;

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
        WarriorTurnText = GameObject.Find("WarriorTurnText");
        ArcherTurnText = GameObject.Find("ArcherTurnText");
        DwarfTurnText = GameObject.Find("DwarfTurnText");
        WizardTurnText = GameObject.Find("WizardTurnText");

        // Initialize UI
        UpdateTurn();
    }

    // Used in Observer design pattern
    public void UpdateData(string Category)
    {
        if (string.Equals(Category, "TURN"))
        {
            UpdateTurn();
        }
    }

    private void UpdateTurn()
    {
        Hero CurrentTurnHero = GameManager.GetCurrentTurnHero();

        if (CurrentTurnHero != null)
        {
            HeroType Type = CurrentTurnHero.GetHeroType();

            switch (Type)
            {
                case HeroType.Warrior:
                    WarriorTurnText.SetActive(true);
                    ArcherTurnText.SetActive(false);
                    DwarfTurnText.SetActive(false);
                    WizardTurnText.SetActive(false);
                    break;

                case HeroType.Archer:
                    WarriorTurnText.SetActive(false);
                    ArcherTurnText.SetActive(true);
                    DwarfTurnText.SetActive(false);
                    WizardTurnText.SetActive(false);
                    break;

                case HeroType.Dwarf:
                    WarriorTurnText.SetActive(false);
                    ArcherTurnText.SetActive(false);
                    DwarfTurnText.SetActive(true);
                    WizardTurnText.SetActive(false);
                    break;

                case HeroType.Wizard:
                    WarriorTurnText.SetActive(false);
                    ArcherTurnText.SetActive(false);
                    DwarfTurnText.SetActive(false);
                    WizardTurnText.SetActive(true);
                    break;

                default:
                    Debug.LogError("Cannot update turn labels in HeroMenu; invalid hero type: " + Type);
                    break;
            }
        }
    }
}
