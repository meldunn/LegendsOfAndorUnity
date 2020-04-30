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

    public void dropShield() { myHero.dropItem(ItemType.Shield); }
    public void dropMedHerb() { myHero.dropItem(ItemType.MedicinalHerb); }
    public void dropBow() { myHero.dropItem(ItemType.Bow); }
    public void dropWineskin() { myHero.dropItem(ItemType.Wineskin); }
    public void dropTelescope() { myHero.dropItem(ItemType.Telescope); }
    public void dropWitchbrew() { myHero.dropItem(ItemType.Witchbrew); }
    public void dropHelm() { myHero.dropItem(ItemType.Helm); }
    public void dropFalcon() { myHero.dropItem(ItemType.Falcon); }
    public void dropYRS() { myHero.dropItem(ItemType.YellowRuneStone); }
    public void dropBRS() { myHero.dropItem(ItemType.BlueRuneStone); }
    public void dropGRS() { myHero.dropItem(ItemType.GreenRuneStone); }


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
