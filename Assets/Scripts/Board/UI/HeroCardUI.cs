using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCardUI : MonoBehaviour
{

    GameObject WarriorHeroCard;
    GameObject ArcherHeroCard;
    GameObject DwarfHeroCard;
    GameObject WizardHeroCard;

    GameObject warriorStatsPanel;
    GameObject archerStatsPanel;
    GameObject dwarfStatsPanel;
    GameObject wizardStatsPanel;

    public void Initialize()
    {

         WarriorHeroCard = GameObject.Find("WarriorHeroCard");
         ArcherHeroCard = GameObject.Find("ArcherHeroCard");
         DwarfHeroCard = GameObject.Find("DwarfHeroCard");
         WizardHeroCard = GameObject.Find("WizardHeroCard");

        warriorStatsPanel = GameObject.Find("WarriorStatsPanel");
        archerStatsPanel = GameObject.Find("ArcherStatsPanel");
        dwarfStatsPanel = GameObject.Find("DwarfStatsPanel");
        wizardStatsPanel = GameObject.Find("WizardStatsPanel");

    }

    public void displayWarriorHeroCard()
    {
        if (WarriorHeroCard.activeSelf){
            WarriorHeroCard.SetActive(false);
            warriorStatsPanel.SetActive(false);
        } else{
            WarriorHeroCard.SetActive(true);
            warriorStatsPanel.SetActive(true);
        }   
    }

    public void displayArcherHeroCard()
    {
        if (ArcherHeroCard.activeSelf){
            ArcherHeroCard.SetActive(false);
            archerStatsPanel.SetActive(false);
        } else{
            ArcherHeroCard.SetActive(true);
            archerStatsPanel.SetActive(true);
        }   
    }

    public void displayDwarfHeroCard()
    {
        if (DwarfHeroCard.activeSelf){
            DwarfHeroCard.SetActive(false);
            dwarfStatsPanel.SetActive(false);
        } else{
            DwarfHeroCard.SetActive(true);
            dwarfStatsPanel.SetActive(true);
        }   
    }

    public void displayWizardHeroCard()
    {
        if (WizardHeroCard.activeSelf){
            WizardHeroCard.SetActive(false);
            wizardStatsPanel.SetActive(false);
        } else{
            WizardHeroCard.SetActive(true);
            wizardStatsPanel.SetActive(true);
        }   
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
