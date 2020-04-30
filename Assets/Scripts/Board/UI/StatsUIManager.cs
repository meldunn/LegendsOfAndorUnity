using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatsUIManager : MonoBehaviour, Observer
{
    GameObject warriorStatsPanel;
    GameObject archerStatsPanel;
    GameObject dwarfStatsPanel;
    GameObject wizardStatsPanel;

    Text warriorFarmerText;
    Text warriorWillpowerText;
    Text warriorStrengthText;
    Text warriorGoldText;

    Text archerFarmerText;
    Text archerWillpowerText;
    Text archerStrengthText;
    Text archerGoldText;

    Text dwarfFarmerText;
    Text dwarfWillpowerText;
    Text dwarfStrengthText;
    Text dwarfGoldText;

    Text wizardFarmerText;
    Text wizardWillpowerText;
    Text wizardStrengthText;
    Text wizardGoldText;

    Hero warrior;
    Hero archer;
    Hero dwarf;
    Hero Wizard;

    GameManager gameManager;
    HeroManager heroManager;
    // Start is called before the first frame update
    void Start()
    {
        warriorStatsPanel = GameObject.Find("WarriorStatsPanel");
        archerStatsPanel = GameObject.Find("ArcherStatsPanel");
        dwarfStatsPanel = GameObject.Find("DwarfStatsPanel");
        wizardStatsPanel = GameObject.Find("WizardStatsPanel");

        warriorFarmerText = GameObject.Find("WarriorFarmersText").GetComponent<Text>();
        warriorWillpowerText = GameObject.Find("WarriorWillpowerText").GetComponent<Text>();
        warriorStrengthText = GameObject.Find("WarriorStrengthText").GetComponent<UnityEngine.UI.Text>();
        warriorGoldText = GameObject.Find("WarriorGoldText").GetComponent<UnityEngine.UI.Text>();

        archerFarmerText = GameObject.Find("ArcherFarmersText").GetComponent<UnityEngine.UI.Text>();
        archerWillpowerText = GameObject.Find("ArcherWillpowerText").GetComponent<UnityEngine.UI.Text>();
        archerStrengthText = GameObject.Find("ArcherStrengthText").GetComponent<UnityEngine.UI.Text>();
        archerGoldText = GameObject.Find("ArcherGoldText").GetComponent<UnityEngine.UI.Text>();

        dwarfFarmerText = GameObject.Find("DwarfFarmersText").GetComponent<UnityEngine.UI.Text>();
        dwarfWillpowerText = GameObject.Find("DwarfWillpowerText").GetComponent<UnityEngine.UI.Text>();
        dwarfStrengthText = GameObject.Find("DwarfStrengthText").GetComponent<UnityEngine.UI.Text>();
        dwarfGoldText = GameObject.Find("DwarfGoldText").GetComponent<UnityEngine.UI.Text>();

        wizardFarmerText = GameObject.Find("WizardFarmersText").GetComponent<UnityEngine.UI.Text>();
        wizardWillpowerText = GameObject.Find("WizardWillpowerText").GetComponent<UnityEngine.UI.Text>();
        wizardStrengthText = GameObject.Find("WizardStrengthText").GetComponent<UnityEngine.UI.Text>();
        wizardGoldText = GameObject.Find("WizardGoldText").GetComponent<UnityEngine.UI.Text>();

        heroManager = GameObject.Find("HeroManager").GetComponent<HeroManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        warriorStatsPanel.SetActive(false);
        archerStatsPanel.SetActive(false);
        dwarfStatsPanel.SetActive(false);
        wizardStatsPanel.SetActive(false);

        UpdateHeroStats();

        warrior = heroManager.GetHero(HeroType.Warrior);
        archer = heroManager.GetHero(HeroType.Archer);
        dwarf = heroManager.GetHero(HeroType.Dwarf);
        Wizard = heroManager.GetHero(HeroType.Wizard);

        warrior.Attach(this);
        archer.Attach(this);
        dwarf.Attach(this);
        Wizard.Attach(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {

    }

    // Used in Observer design pattern
    public void UpdateData(string Category)
    {
        if (string.Equals(Category, "HERO_STATS") || string.Equals(Category, "HERO_WILLPOWER") || string.Equals(Category, "HERO_STRENGTH"))
        {
            UpdateHeroStats();
        }
    }

    public void UpdateHeroStats()
    {
        Debug.Log("updating stats");
        warriorFarmerText.text = " Farmers: " + heroManager.GetHero(HeroType.Warrior).getNumFarmers();
        warriorWillpowerText.text = " Willpower: " + heroManager.GetHero(HeroType.Warrior).getWillpower();
        warriorStrengthText.text = " Strength: " + heroManager.GetHero(HeroType.Warrior).getStrength();
        warriorGoldText.text = " Gold: " + heroManager.GetHero(HeroType.Warrior).getGold();

        archerFarmerText.text = " Farmers: " + heroManager.GetHero(HeroType.Archer).getNumFarmers();
        archerWillpowerText.text = " Willpower: " + heroManager.GetHero(HeroType.Archer).getWillpower();
        archerStrengthText.text = " Strength: " + heroManager.GetHero(HeroType.Archer).getStrength();
        archerGoldText.text = " Gold: " + heroManager.GetHero(HeroType.Archer).getGold();

        dwarfFarmerText.text = " Farmers: " + heroManager.GetHero(HeroType.Dwarf).getNumFarmers();
        dwarfWillpowerText.text = " Willpower: " + heroManager.GetHero(HeroType.Dwarf).getWillpower();
        dwarfStrengthText.text = " Strength: " + heroManager.GetHero(HeroType.Dwarf).getStrength();
        dwarfGoldText.text = " Gold: " + heroManager.GetHero(HeroType.Dwarf).getGold();

        wizardFarmerText.text = " Farmers: " + heroManager.GetHero(HeroType.Wizard).getNumFarmers();
        wizardWillpowerText.text = " Willpower: " + heroManager.GetHero(HeroType.Wizard).getWillpower();
        wizardStrengthText.text = " Strength: " + heroManager.GetHero(HeroType.Wizard).getStrength();
        wizardGoldText.text = " Gold: " + heroManager.GetHero(HeroType.Wizard).getGold();
    }
}
