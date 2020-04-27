using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCardUI : MonoBehaviour
{
    //TODO: attach game manager for getSelfPlayer
    private GameManager GameManager;

    [SerializeField]
    HeroType myHeroType;

    //HeroCard and StatsPanek
    [SerializeField] GameObject HeroCard;
    [SerializeField] GameObject StatsPanel;

    //Children: Items
    GameObject Bow;
    GameObject Brew;
    GameObject Falcon;
    GameObject Telescope;
    GameObject Wineskin;
    GameObject Shield;
    GameObject Helm;
    GameObject GreenRune;
    GameObject YellowRune;
    GameObject BlueRune;

    public void Initialize()
    {

        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //Hero.Attach(this);

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

   
}
