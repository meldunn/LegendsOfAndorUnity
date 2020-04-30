using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections.Generic;
using System;
using Photon.Realtime;


//Autor: Vitaly

public class PlayerSelector : MonoBehaviourPun
{
    [SerializeField]
    private GameObject readyToken;
    [SerializeField]
    private Text nickName;


    [Header("SpriteControl")]
    [SerializeField]
    private Button next;
    [SerializeField]
    private Button prev;
    [SerializeField]
    private Image hero;


    [Header("CoinControl")]
    [SerializeField]
    private GameObject coins;
    [SerializeField]
    private Text coinsText;
    [SerializeField]
    private Button nextCoin;
    [SerializeField]
    private Button prevCoin;

    [Header("WineSkinControl")]
    [SerializeField]
    private GameObject wineSkin;
    [SerializeField]
    private Text wineSkinText;
    [SerializeField]
    private Button nextWine;
    [SerializeField]
    private Button prevWine;


    [Header("HeroSprites")]
    [SerializeField]
    private Sprite[] heroSprites;
    private int currentHero;


    private HeroType heroSelectedType;
    private int numberOfCoins = 0;
    private int numberOfWine = 0;


    int maxCoins = 5;
    int maxWine = 2;
    int playerID;

    private bool isReady = false;



    public void OnEnable()
    {

        if (photonView.IsMine)
        {
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
        playerID = id + 1;

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
        if (currentHero == 4)
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
        //TODO: isReady not  updated onother clients than master
        isReady = !isReady;

        //set next & previous buttons inactive
        next.gameObject.SetActive(!next.gameObject.activeSelf);
        prev.gameObject.SetActive(!prev.gameObject.activeSelf);


        photonView.RPC("ReadyUp", RpcTarget.All);


        //sends an rpc only for master client to avoid cloging the network
        if (PhotonNetwork.IsMasterClient)
        {

            HeroSelectionManager.Instance.OnPlayerReady(PhotonNetwork.LocalPlayer.ActorNumber, heroSelectedType, isReady);
        }
        else
        {

            //TODO: the RPC has to include isReady
            //google rpc parameters sending local variables.
            photonView.RPC("ReadyUpMaster", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer.ActorNumber, heroSelectedType, isReady);
        }
    }

    [PunRPC]
    void ReadyUp()
    {
        bool isEnabled = readyToken.activeSelf;
        readyToken.SetActive(!isEnabled);
    }

    [PunRPC]
    void ReadyUpMaster(int playerID, HeroType hero, bool isReady)
    {
        //checks for all the players being ready
        HeroSelectionManager.Instance.OnPlayerReady(playerID, hero, isReady);
    }

    public void TransitionToResourceDivision()
    {
        readyToken.SetActive(false);

        //enable coins and wine
        coins.SetActive(true);
        wineSkin.SetActive(true);

        //enable buttons
        if (PhotonNetwork.IsMasterClient)
        {
            nextWine.gameObject.SetActive(true);
            nextCoin.gameObject.SetActive(true);
        }
    }

    public void OnClick_NextCoin()
    {
        int count = CountCoins();


        if (count < maxCoins)
        {

            //we are about to hit 5
            if (count == maxCoins - 1)
            {
                PlayerSelector[] players = GameObject.FindObjectsOfType<PlayerSelector>();

                foreach (var player in players)
                {
                    player.nextCoin.gameObject.SetActive(false);
                }
            }

            int value = HeroSelectionManager.Instance.coinsSplit[playerID];
            value += 1; //increment the value

            if (value == 1)
            {
                prevCoin.gameObject.SetActive(true); // enable previous button
            }

            HeroSelectionManager.Instance.coinsSplit[playerID] = value;

            photonView.RPC("IncrementCoins", RpcTarget.All);

            if (isFullySplit())
            {
                HeroSelectionManager.Instance.EnableDifficultyButtons();
            }

        }
        else
        {
            //shouldnt be able to increase coins, since the buttons are disabled
        }
    }

    bool isFullySplit()
    {
        int coinCount = CountCoins();
        int wineCount = CountWineSkins();

        if (coinCount == maxCoins && wineCount == maxWine)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnClick_PrevCoin()
    {
        if (isFullySplit())
        {
            HeroSelectionManager.Instance.DisableDifficultyButtons();
        }


        int amount = Int32.Parse(coinsText.text);

        //we can decrease
        if (amount > 0)
        {
            //need to enable the next buttons if we decrease from max
            int count = CountCoins();

            //we are about to hit 4
            if (count == maxCoins)
            {
                PlayerSelector[] players = GameObject.FindObjectsOfType<PlayerSelector>();

                foreach (var player in players)
                {
                    player.nextCoin.gameObject.SetActive(true);
                }
            }

            int value = HeroSelectionManager.Instance.coinsSplit[playerID];
            value -= 1;
            HeroSelectionManager.Instance.coinsSplit[playerID] = value;

            if (value == 0)
            {
                prevCoin.gameObject.SetActive(false); // disable previous button
            }

            photonView.RPC("DecreaseCoins", RpcTarget.All);
        }
        else
        {
            //can't decrease anymore
            //The button should be hidden
        }
    }

    int CountCoins()
    {
        Dictionary<int, int> coins = HeroSelectionManager.Instance.coinsSplit;
        int count = 0;
        foreach (var player in coins)
        {
            count += player.Value;
        }

        return count;
    }

    int CountWineSkins()
    {
        Dictionary<int, int> wine = HeroSelectionManager.Instance.wineSplit;
        int count = 0;
        foreach (var player in wine)
        {
            count += player.Value;
        }

        return count;
    }

    [PunRPC]
    void DecreaseCoins()
    {
        coinsText.text = (Int32.Parse(coinsText.text) - 1).ToString();
    }

    [PunRPC]
    void IncrementCoins()
    {
        coinsText.text = (Int32.Parse(coinsText.text) + 1).ToString(); //increment the text
    }

    public void OnClick_NextWine()
    {
        int count = CountWineSkins();


        if (count < maxWine)
        {

            //we are about to hit 5
            if (count == maxWine - 1)
            {
                PlayerSelector[] players = GameObject.FindObjectsOfType<PlayerSelector>();

                foreach (var player in players)
                {
                    player.nextWine.gameObject.SetActive(false);
                }
            }

            int value = HeroSelectionManager.Instance.wineSplit[playerID];
            value += 1; //increment the value

            if (value == 1)
            {
                prevWine.gameObject.SetActive(true); // enable previous button
            }

            HeroSelectionManager.Instance.wineSplit[playerID] = value;

            photonView.RPC("IncrementWine", RpcTarget.All);

            if (isFullySplit())
            {
                HeroSelectionManager.Instance.EnableDifficultyButtons();
            }
        }
        else
        {
            //shouldnt be able to increase coins, since the buttons are disabled
        }
    }

    public void OnClick_PrevWine()
    {
        if (isFullySplit())
        {
            HeroSelectionManager.Instance.DisableDifficultyButtons();
        }

        int amount = Int32.Parse(wineSkinText.text);

        //we can decrease
        if (amount > 0)
        {
            //need to enable the next buttons if we decrease from max
            int count = CountWineSkins();

            //we are about to hit 4
            if (count == maxWine)
            {
                PlayerSelector[] players = GameObject.FindObjectsOfType<PlayerSelector>();

                foreach (var player in players)
                {
                    player.nextWine.gameObject.SetActive(true);
                }
            }

            int value = HeroSelectionManager.Instance.wineSplit[playerID];
            value -= 1;
            HeroSelectionManager.Instance.wineSplit[playerID] = value;

            if (value == 0)
            {
                prevWine.gameObject.SetActive(false); // disable previous button
            }

            photonView.RPC("DecreaseWine", RpcTarget.All);


        }
        else
        {
            //can't decrease anymore
            //The button should be hidden
        }
    }

    [PunRPC]
    void IncrementWine()
    {
        wineSkinText.text = (Int32.Parse(wineSkinText.text) + 1).ToString(); //increment the text
    }

    [PunRPC]
    void DecreaseWine()
    {
        wineSkinText.text = (Int32.Parse(wineSkinText.text) - 1).ToString();
    }
}
