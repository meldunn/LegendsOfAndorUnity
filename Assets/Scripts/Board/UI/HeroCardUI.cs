using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCardUI : MonoBehaviour, Observer
{
    //TODO: attach game manager for getSelfPlayer
    HeroManager HeroManager;
    GameManager GameManager;

    [SerializeField]
    HeroType myHeroType;

    Hero myHero;
    HeroInventory inventory;

    //HeroCard and StatsPanek
    [SerializeField] GameObject HeroCard;
    [SerializeField] GameObject StatsPanel;

    //Children: Items
    [SerializeField] GameObject Bow;
    [SerializeField] GameObject Brew;
    [SerializeField] GameObject Falcon;
    [SerializeField] GameObject Telescope;
    [SerializeField] GameObject Wineskin;
    [SerializeField] GameObject Shield;
    [SerializeField] GameObject Helm;
    [SerializeField] GameObject GreenRune;
    [SerializeField] GameObject YellowRune;
    [SerializeField] GameObject BlueRune;
    [SerializeField] GameObject Medicinalherb;

    public void Initialize()
    {
        HeroManager = GameObject.Find("HeroManager").GetComponent<HeroManager>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        myHero = HeroManager.GetHero(myHeroType);
        myHero.Attach(this);

    }

    public void displayHeroCard()
    {
        if (HeroCard.activeSelf){
            HeroCard.SetActive(false);
            StatsPanel.SetActive(false);
        } else{
            HeroCard.SetActive(true);
            StatsPanel.SetActive(true);
        }   
    }

    public void UpdateData(string Category)
    {
        if (string.Equals(Category, "HERO_ITEMS"))
        {
            UpdateHeroInventory();
        }
    }

    public void UpdateHeroInventory()
    {
        
    }

    }
