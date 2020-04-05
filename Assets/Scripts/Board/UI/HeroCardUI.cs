﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCardUI : MonoBehaviour
{

    GameObject WarriorHeroCard;
    GameObject ArcherHeroCard;
    GameObject DwarfHeroCard;
    GameObject WizardHeroCard;

    public void Initialize()
    {

        GameObject WarriorHeroCard = GameObject.Find("WarriorHeroCard");
        GameObject ArcherHeroCard = GameObject.Find("ArcherHeroCard");
        GameObject DwarfHeroCard = GameObject.Find("DwarfHeroCard");
        GameObject WizardHeroCard = GameObject.Find("WizardHeroCard");

    }

    public void displayWarriorHeroCard()
    {
        if (WarriorHeroCard.activeSelf){
            WarriorHeroCard.SetActive(false);
        } else{
            WarriorHeroCard.SetActive(true);
        }   
    }

    public void displayArcherHeroCard()
    {
        if (ArcherHeroCard.activeSelf){
            ArcherHeroCard.SetActive(false);
        } else{
            ArcherHeroCard.SetActive(true);
        }   
    }

    public void displayDwarfHeroCard()
    {
        if (DwarfHeroCard.activeSelf){
            DwarfHeroCard.SetActive(false);
        } else{
            DwarfHeroCard.SetActive(true);
        }   
    }

    public void displayWizardHeroCard()
    {
        if (WizardHeroCard.activeSelf){
            WizardHeroCard.SetActive(false);
        } else{
            WizardHeroCard.SetActive(true);
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
