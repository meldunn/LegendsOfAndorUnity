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
    PhotonView pv;

    public void Initialize()
    {
        HeroManager = GameObject.Find("HeroManager").GetComponent<HeroManager>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        myHero = HeroManager.GetHero(myHeroType);
        this.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
        //pv = photonView.GetComponent<PhotonView>();
        myHero.Attach(this);

    }

    public void displayHeroCard()
    {
        if (HeroCard.activeSelf)
        {
            HeroCard.SetActive(false);
            StatsPanel.SetActive(false);
        }
        else
        {

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
    public void dropShieldRPC()
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
            photonView.RPC("dropShieldRPC", RpcTarget.All);
        }
        else
        {
            dropShieldRPC();
        }
    }

    [PunRPC]
    public void dropMedHerbRPC()
    {
        if (GameManager.GetSelfHero() == myHero)
        {
            myHero.dropItem(ItemType.MedicinalHerb);
            Medicinalherb.SetActive(false);
        }
    }

    public void dropMedHerb()
    {
        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("dropMedHerbRPC", RpcTarget.All);
        }
        else
        {
            dropMedHerbRPC();
        }
    }

    [PunRPC]
    public void dropBowRPC()
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
            photonView.RPC("dropBowRPC", RpcTarget.All);
        }
        else
        {
            dropBowRPC();
        }
    }

    [PunRPC]
    public void dropWineskinRPC()
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
            photonView.RPC("dropWineskinRPC", RpcTarget.All);
        }
        else
        {
            dropWineskinRPC();
        }
    }

    [PunRPC]
    public void dropTelescopeRPC()
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
            photonView.RPC("dropTelescopeRPC", RpcTarget.All);
        }
        else
        {
            dropTelescopeRPC();
        }
    }

    [PunRPC]
    public void dropWitchbrewRPC()
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
            photonView.RPC("dropWitchbrewRPC", RpcTarget.All);
        }
        else
        {
            dropWitchbrewRPC();
        }
    }

    [PunRPC]
    public void dropHelmRPC()
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
            photonView.RPC("dropHelmRPC", RpcTarget.All);
        }
        else
        {
            dropHelmRPC();
        }
    }

    [PunRPC]
    public void dropFalconRPC()
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
            photonView.RPC("dropHelmRPC", RpcTarget.All);
        }
        else
        {
            dropHelmRPC();
        }
    }

    [PunRPC]
    public void dropYRSRPC()
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
            photonView.RPC("dropYRSRPC", RpcTarget.All);
        }
        else
        {
            dropYRSRPC();
        }
    }

    [PunRPC]
    public void dropBRSRPC()
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
            photonView.RPC("dropBRSRPC", RpcTarget.All);
        }
        else
        {
            dropBRSRPC();
        }
    }

    [PunRPC]
    public void dropGRSRPC()
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
            photonView.RPC("dropGRSRPC", RpcTarget.All);
        }
        else
        {
            dropGRSRPC();
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
