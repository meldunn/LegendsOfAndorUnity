using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCardUI : MonoBehaviour, Observer
{
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

            if (GameManager.GetSelfHero() == myHero)
            {
                //set drop buttons as active
            }
            
            //iterating through the items
            if (!myHero.heroInventory.containsItem(ItemType.MedicinalHerb)) Medicinalherb.SetActive(false);
            if (!myHero.heroInventory.containsItem(ItemType.Bow)) Bow.SetActive(false);
            if (!myHero.heroInventory.containsItem(ItemType.Wineskin)) Wineskin.SetActive(false);
            if (!myHero.heroInventory.containsItem(ItemType.Telescope)) Telescope.SetActive(false);
            if (!myHero.heroInventory.containsItem(ItemType.Witchbrew)) Brew.SetActive(false);
            if (!myHero.heroInventory.containsItem(ItemType.Helm)) Helm.SetActive(false);
            if (!myHero.heroInventory.containsItem(ItemType.Falcon)) Falcon.SetActive(false);
            if (!myHero.heroInventory.containsItem(ItemType.Shield)) Shield.SetActive(false);
            if (!myHero.heroInventory.containsItem(ItemType.YellowRuneStone)) YellowRune.SetActive(false);
            if (!myHero.heroInventory.containsItem(ItemType.BlueRuneStone)) BlueRune.SetActive(false);
            if (!myHero.heroInventory.containsItem(ItemType.GreenRuneStone)) GreenRune.SetActive(false);




            StatsPanel.SetActive(true);
        }   
    }

    public void dropShield() {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.Shield);
            Shield.SetActive(false);
        }
    }

    public void dropMedHerb()
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.MedicinalHerb);
            Medicinalherb.SetActive(false);
        }
    }

    public void dropBow()
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.Bow);
            Bow.SetActive(false);
        }
    }

    public void dropWineskin()
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.Wineskin);
            Wineskin.SetActive(false);
        }
    }

    public void dropTelescope()
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.Telescope);
            Telescope.SetActive(false);
        }
    }

    public void dropWitchbrew()
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.Witchbrew);
            Brew.SetActive(false);
        }
    }

    public void dropHelm()
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.Helm);
            Helm.SetActive(false);
        }
    }

    public void dropFalcon()
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.Falcon);
            Falcon.SetActive(false);
        }
    }
    public void dropYRS()
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.YellowRuneStone);
            YellowRune.SetActive(false);
        }
    }

    public void dropBRS()
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.BlueRuneStone);
            BlueRune.SetActive(false);
        }
    }

    public void dropGRS()
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.GreenRuneStone);
            GreenRune.SetActive(false);
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
