﻿using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


//Autor: Vitaly

public class PlayerSelector : MonoBehaviourPun
{
    [SerializeField]
    private GameObject readyToken;
    [SerializeField]
    private Button next;
    [SerializeField]
    private Button prev;
    [SerializeField]
    private Image hero;
    [SerializeField]
    private Text nickName;

    [Header("HeroSprites")]
    [SerializeField]
    private Sprite[] heroSprites;
    private int currentHero;


    private HeroType heroSelectedType;

    public bool isReady { get; private set; }



    public void OnEnable()
    {
        if (photonView.IsMine)
        {
            isReady = false;

            currentHero = 0;
            hero.sprite = heroSprites[currentHero];

            next.gameObject.SetActive(true);
            prev.gameObject.SetActive(true);


            heroSelectedType = HeroType.Warrior;
            photonView.RPC("InstantiateSelector", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber - 1);



        }
    }


    //adjusts the tranforms
    [PunRPC]
    void InstantiateSelector(int id)
    {
        nickName.text = photonView.Owner.NickName;

        gameObject.transform.SetParent(HeroSelectionManager.Instance.heroSelectorPositions[id]);
        gameObject.transform.localScale = Vector3.one;
    }

    public void OnClick_NextHero()
    {

        //executes only if the owner of the button clicked
       if (photonView.IsMine && !readyToken.activeSelf)
       {
            photonView.RPC("NextHero", RpcTarget.All);
       }
    }

    [PunRPC]
    void NextHero()
    {
        currentHero += 1;
        if(currentHero == 4)
        {
            currentHero = 0;
        }
        hero.sprite = heroSprites[currentHero];
        heroSelectedType = (HeroType)currentHero;
    }


    public void OnClick_PrevHero()
    {
        
        //executes only if the owner of the button clicked
        if (photonView.IsMine && !readyToken.activeSelf)
        {
            photonView.RPC("PrevHero", RpcTarget.All);
        }
    }

    [PunRPC]
    void PrevHero()
    {
        currentHero -= 1;
        if (currentHero == -1)
        {
            currentHero = 3;
        }


        hero.sprite = heroSprites[currentHero];
        heroSelectedType = (HeroType)currentHero;
    }

    public void OnClick_Ready()
    {
        isReady = !isReady;

        //set next & previous buttons inactive
        next.gameObject.SetActive(!next.gameObject.activeSelf);
        prev.gameObject.SetActive(!prev.gameObject.activeSelf);


        photonView.RPC("ReadyUp", RpcTarget.All);


        //sends an rpc only for master client to avoid cloging the network
        if (PhotonNetwork.IsMasterClient)
        {
            //PhotonNetwork.LocalPlayer.ActorNumber
            //HeroType
            HeroSelectionManager.Instance.OnPlayerReady(PhotonNetwork.LocalPlayer.ActorNumber, heroSelectedType, isReady);
        }
        else
        {
            photonView.RPC("ReadyUpMaster", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    void ReadyUp()
    {
        //TODO: Non critical error of some prefab not being assigned a ViewID
        
        bool isEnabled = readyToken.activeSelf;
        readyToken.SetActive(!isEnabled);
    }

    [PunRPC]
    void ReadyUpMaster()
    {
        //checks for all the players being ready
        HeroSelectionManager.Instance.OnPlayerReady(PhotonNetwork.LocalPlayer.ActorNumber, heroSelectedType, isReady);
    }

}
