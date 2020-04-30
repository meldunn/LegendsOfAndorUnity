using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class EventCard : MonoBehaviour
{
    private GameManager GameManager;
    private HeroManager HeroManager;
    private WaypointManager WaypointManager;

    private int cardId;
    private string cardQuote;
    private string cardDescription;

    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        HeroManager = GameObject.Find("HeroManager").GetComponent<HeroManager>();
        WaypointManager = GameObject.Find("WaypointManager").GetComponent<WaypointManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setId(int i)
    {
        cardId = i;
    }

    public void setQuote(string s)
    {
        cardQuote = s;
    }

    public void setDescription(string s)
    {
        cardDescription = s;
    }

    public void trigger()
    {
        if (cardId == 1)
        {
            foreach (HeroType Type in HeroManager.GetAllHeroTypes())
            {
                if (HeroManager.GetHero(Type).GetCurrentRegion().GetWaypointNum() < 21)
                {
                    HeroManager.GetHero(Type).DecreaseWillpower(3);
                }
            }
        }
        else if (cardId == 2)
        {
            foreach (HeroType Type in HeroManager.GetAllHeroTypes())
            {
                if (HeroManager.GetHero(Type).GetCurrentRegion().GetWaypointNum() < 71 && HeroManager.GetHero(Type).GetCurrentRegion().GetWaypointNum() > 29)
                {
                    HeroManager.GetHero(Type).DecreaseWillpower(3);
                }
            }
        }
        else if (cardId == 3)
        {
            HeroManager.GetHero(HeroType.Archer).IncreaseWillpower(3);
            HeroManager.GetHero(HeroType.Wizard).IncreaseWillpower(3);
        }
        else if (cardId == 4)
        {
            foreach (HeroType Type in HeroManager.GetAllHeroTypes())
            {
                if (HeroManager.GetHero(Type).getWillpower() < 10)
                {
                    //Essentially sets willpower to 10
                    HeroManager.GetHero(Type).DecreaseWillpower(20);
                    HeroManager.GetHero(Type).IncreaseWillpower(10);
                }
            }
        }
        else if (cardId == 5)
        {
            HeroManager.GetHero(HeroType.Dwarf).IncreaseWillpower(3);
            HeroManager.GetHero(HeroType.Warrior).IncreaseWillpower(3);
        }
        else if (cardId == 6)
        {
            foreach (HeroType Type in HeroManager.GetAllHeroTypes())
            {
                if (HeroManager.GetHero(Type).getWillpower() > 12)
                {
                    //Essentially sets willpower to 10
                    HeroManager.GetHero(Type).DecreaseWillpower(20);
                    HeroManager.GetHero(Type).IncreaseWillpower(12);
                }
            }
        }
        else if (cardId == 7)
        {
            foreach (HeroType Type in HeroManager.GetAllHeroTypes())
            {
                if (HeroManager.GetHero(Type).GetTimeOfDay() == 0)
                {
                    HeroManager.GetHero(Type).IncreaseWillpower(2);
                }
            }
        }
        else if (cardId == 8)
        {
            foreach (HeroType Type in HeroManager.GetAllHeroTypes())
            {
                int num = HeroManager.GetHero(Type).GetCurrentRegion().GetWaypointNum();
                if (num == 0 || num == 71 || num == 72 || num < 26 && num > 21 || num < 64 && num > 46)
                {
                    continue;
                }
                else
                {
                    HeroManager.GetHero(Type).DecreaseWillpower(2);
                }

            }
        }
        else if (cardId == 9)
        {
            foreach (HeroType Type in HeroManager.GetAllHeroTypes())
            {
                if (HeroManager.GetHero(Type).GetTimeOfDay() == 0)
                {
                    HeroManager.GetHero(Type).DecreaseWillpower(2);
                }
            }
        }

    }
}
