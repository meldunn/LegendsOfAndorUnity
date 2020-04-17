using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMenu : MonoBehaviour, Observer
{
    // Reference to GameManager
    private GameManager GameManager;

    // References to children components
    [SerializeField]
    GameObject WarriorTurnBox = null;
    [SerializeField]
    GameObject ArcherTurnBox = null;
    [SerializeField]
    GameObject DwarfTurnBox = null;
    [SerializeField]
    GameObject WizardTurnBox = null;

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
        // Defaults
        WarriorTurnBox.SetActive(false);
        ArcherTurnBox.SetActive(false);
        DwarfTurnBox.SetActive(false);
        WizardTurnBox.SetActive(false);

        Hero CurrentTurnHero = GameManager.GetCurrentTurnHero();

        if (CurrentTurnHero != null)
        {
            HeroType Type = CurrentTurnHero.GetHeroType();

            if      (Type == HeroType.Warrior) WarriorTurnBox.SetActive(true);
            else if (Type == HeroType.Archer) ArcherTurnBox.SetActive(true);
            else if (Type == HeroType.Dwarf) DwarfTurnBox.SetActive(true);
            else if (Type == HeroType.Wizard) WizardTurnBox.SetActive(true);
            else Debug.LogError("Cannot update turn labels in HeroMenu; invalid hero type: " + Type);
        }
    }
}
