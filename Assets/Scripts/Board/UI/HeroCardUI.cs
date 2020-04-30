using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class HeroCardUI : MonoBehaviourPun, Observer
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

            if (GameManager.GetSelfHero() == myHero)
            {
                //set drop buttons as active
            }

            //iterating through the items
            UpdateHeroInventory();

            HeroCard.SetActive(true);

            StatsPanel.SetActive(true);
        }   
    }

    [PunRPC]
    public void dropShieldRPC(HeroType type)
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.Shield);
            Shield.SetActive(false);
        }
    }

    public void dropShield()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("dropShieldRPC", RpcTarget.All, GameManager.GetSelfHero().GetHeroType());
        }
        else
        {
            dropShieldRPC(GameManager.GetSelfHero().GetHeroType());
        }
    }

    [PunRPC]
    public void dropMedHerbRPC(HeroType type)
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.GetHero(type).dropItem(ItemType.MedicinalHerb);
            Medicinalherb.SetActive(false);
        }
    }

    public void dropMedHerb()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("dropMedHerbRPC", RpcTarget.All, GameManager.GetSelfHero().GetHeroType());
        }
        else
        {
            dropMedHerbRPC(GameManager.GetSelfHero().GetHeroType());
        }
    }

    [PunRPC]
    public void dropBowRPC(HeroType type)
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.Bow);
            Bow.SetActive(false);
        }
    }

    public void dropBow()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("dropBowRPC", RpcTarget.All, GameManager.GetSelfHero().GetHeroType());
        }
        else
        {
            dropBowRPC(GameManager.GetSelfHero().GetHeroType());
        }
    }

    [PunRPC]
    public void dropWineskinRPC(HeroType type)
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.Wineskin);
            Wineskin.SetActive(false);
        }

    }

    public void dropWineskin()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("dropWineskinRPC", RpcTarget.All, GameManager.GetSelfHero().GetHeroType());
        }
        else
        {
            dropWineskinRPC(GameManager.GetSelfHero().GetHeroType());
        }
    }

    [PunRPC]
    public void dropTelescopeRPC(HeroType type)
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.Telescope);
            Telescope.SetActive(false);
        }

    }

    public void dropTelescope()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("dropTelescopeRPC", RpcTarget.All, GameManager.GetSelfHero().GetHeroType());
        }
        else
        {
            dropTelescopeRPC(GameManager.GetSelfHero().GetHeroType());
        }
    }

    [PunRPC]
    public void dropWitchbrewRPC(HeroType type)
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.Witchbrew);
            Brew.SetActive(false);
        }
    }
    
    public void dropWitchbrew()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("dropWitchbrewRPC", RpcTarget.All, GameManager.GetSelfHero().GetHeroType());
        }
        else
        {
            dropWitchbrewRPC(GameManager.GetSelfHero().GetHeroType());
        }
    }

    [PunRPC]
    public void dropHelmRPC(HeroType type)
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.Helm);
            Helm.SetActive(false);
        }
        
    }

    public void dropHelm()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("dropHelmRPC", RpcTarget.All, GameManager.GetSelfHero().GetHeroType());
        }
        else
        {
            dropHelmRPC(GameManager.GetSelfHero().GetHeroType());
        }
    }

    [PunRPC]
    public void dropFalconRPC(HeroType type)
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.Falcon);
            Falcon.SetActive(false);
        }
        
    }

    public void dropFalcon()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("dropHelmRPC", RpcTarget.All, GameManager.GetSelfHero().GetHeroType());
        }
        else
        {
            dropHelmRPC(GameManager.GetSelfHero().GetHeroType());
        }
    }

    [PunRPC]
    public void dropYRSRPC(HeroType type)
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.YellowRuneStone);
            YellowRune.SetActive(false);
        }
        
    }

    public void dropYRS()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("dropYRSRPC", RpcTarget.All, GameManager.GetSelfHero().GetHeroType());
        }
        else
        {
            dropYRSRPC(GameManager.GetSelfHero().GetHeroType());
        }
    }

    [PunRPC]
    public void dropBRSRPC(HeroType type)
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.BlueRuneStone);
            BlueRune.SetActive(false);
        }
    }

    public void dropBRS()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("dropBRSRPC", RpcTarget.All, GameManager.GetSelfHero().GetHeroType());
        }
        else
        {
            dropBRSRPC(GameManager.GetSelfHero().GetHeroType());
        }
    }

    [PunRPC]
    public void dropGRSRPC(HeroType type)
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.GreenRuneStone);
            GreenRune.SetActive(false);
        }
       
    }

    public void dropGRS()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("dropGRSRPC", RpcTarget.All, GameManager.GetSelfHero().GetHeroType());
        }
        else
        {
            dropGRSRPC(GameManager.GetSelfHero().GetHeroType());
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
        Wineskin.SetActive(myHero.heroInventory.containsItem(ItemType.Wineskin));
        Medicinalherb.SetActive(myHero.heroInventory.containsItem(ItemType.MedicinalHerb));
        Bow.SetActive(myHero.heroInventory.containsItem(ItemType.Bow));
        Telescope.SetActive(myHero.heroInventory.containsItem(ItemType.Telescope));
        Brew.SetActive(myHero.heroInventory.containsItem(ItemType.Witchbrew));
        Helm.SetActive(myHero.heroInventory.containsItem(ItemType.Helm));
        Falcon.SetActive(myHero.heroInventory.containsItem(ItemType.Falcon));
        Shield.SetActive(myHero.heroInventory.containsItem(ItemType.Shield));
        YellowRune.SetActive(myHero.heroInventory.containsItem(ItemType.YellowRuneStone));
        BlueRune.SetActive(myHero.heroInventory.containsItem(ItemType.BlueRuneStone));
        GreenRune.SetActive(myHero.heroInventory.containsItem(ItemType.GreenRuneStone));
    }

}
