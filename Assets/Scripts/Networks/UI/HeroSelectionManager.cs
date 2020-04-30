using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;

//Autor: Vitaly

public class HeroSelectionManager : MonoBehaviourPunCallbacks
{
    public static HeroSelectionManager Instance;

    public Transform[] heroSelectorPositions;
    [SerializeField]
    private Button readyUp;
    [SerializeField]
    private Button normalDifficulty;
    [SerializeField]
    private Button easyDifficulty;
    [SerializeField]
    private GameObject heroSelectCanvas;


    //ready logic
    int readyPlayers = 0;
    public Dictionary<int, HeroType> selectedHeroes;
    public Dictionary<int, int> coinsSplit;
    public Dictionary<int, int> wineSplit;



    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        selectedHeroes = new Dictionary<int, HeroType>();
        coinsSplit = new Dictionary<int, int>();
        wineSplit = new Dictionary<int, int>();

        int id = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        GameObject heroSelectorInstance = PhotonNetwork.Instantiate("HeroSelectionGUI", heroSelectorPositions[id].position, heroSelectorPositions[id].rotation);


        //at run time subsribes the ready up logic. (done on run time because we are dealing with a prefab)
        readyUp.onClick.AddListener(heroSelectorInstance.GetComponent<PlayerSelector>().OnClick_Ready);

    }

    public void OnPlayerReady(int playerID, HeroType type, bool status)
    {

        if (status)
        {
            readyPlayers++;
            selectedHeroes.Add(playerID, type);
            coinsSplit.Add(playerID, 0);
            wineSplit.Add(playerID, 0);
        }
        else
        {
            readyPlayers--;
            selectedHeroes.Remove(playerID);
            coinsSplit.Remove(playerID);
            wineSplit.Remove(playerID);

        }

        print("Hero is : " + type);
        print("Status Recieved: " + status + " || Number ready: " + readyPlayers + " || Players in room: " + PhotonNetwork.CurrentRoom.PlayerCount);
        if (readyPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            if (AreDifferentHeroes())
            {
                //this executes on master by construction

                photonView.RPC("InstantiateSplitResources", RpcTarget.All);

            }
        }
    }

    [PunRPC]
    private void InstantiateSplitResources()
    {
        PlayerSelector[] players = GameObject.FindObjectsOfType<PlayerSelector>();

        foreach (var player in players)
        {
            player.TransitionToResourceDivision();

        }

        readyUp.gameObject.SetActive(false);
    }

    public void EnableDifficultyButtons()
    {
        normalDifficulty.gameObject.SetActive(true);
        easyDifficulty.gameObject.SetActive(true);
    }

    public void DisableDifficultyButtons()
    {
        normalDifficulty.gameObject.SetActive(false);
        easyDifficulty.gameObject.SetActive(false);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        readyPlayers--;
        coinsSplit.Remove(otherPlayer.ActorNumber);

        //TODO: enable coin arrows and wine arrows

        wineSplit.Remove(otherPlayer.ActorNumber);
        selectedHeroes.Remove(otherPlayer.ActorNumber);
    }

    bool AreDifferentHeroes()
    {
        foreach (var player1 in selectedHeroes)
        {
            foreach (var player2 in selectedHeroes)
            {
                if (player1.Key != player2.Key)
                {
                    if (player1.Value == player2.Value) return false;
                }
            }
        }

        return true;
    }

    public void OnClick_NormalDifficulty()
    {
        //RPC
        photonView.RPC("InitializeGameManager", RpcTarget.All, DifficultyLevel.Normal);


    }

    public void OnClick_EasyDifficulty()
    {
        photonView.RPC("InitializeGameManager", RpcTarget.All, DifficultyLevel.Easy, selectedHeroes, coinsSplit, wineSplit);
    }


    [PunRPC]
    void InitializeGameManager(DifficultyLevel level,
                               Dictionary<int, HeroType> selectedHeroes,
                               Dictionary<int, int> coinsSplit,
                               Dictionary<int, int> wineSplit)
    {
        GameManager.Instance.Difficulty = level;

        int playerID = PhotonNetwork.LocalPlayer.ActorNumber;

        GameManager.Instance.SetSelfPlayer(selectedHeroes[playerID]);

        GameManager.Instance.SetIsPlaying(HeroType.Warrior, false);
        GameManager.Instance.SetIsPlaying(HeroType.Archer, false);
        GameManager.Instance.SetIsPlaying(HeroType.Dwarf, false);
        GameManager.Instance.SetIsPlaying(HeroType.Wizard, false);


        foreach (var hero in selectedHeroes)
        {
            switch (hero.Value)
            {
                case HeroType.Warrior:
                    GameManager.Instance.SetIsPlaying(HeroType.Warrior, true);
                    break;
                case HeroType.Archer:
                    GameManager.Instance.SetIsPlaying(HeroType.Archer, true);
                    break;
                case HeroType.Dwarf:
                    GameManager.Instance.SetIsPlaying(HeroType.Dwarf, true);
                    break;
                case HeroType.Wizard:
                    GameManager.Instance.SetIsPlaying(HeroType.Wizard, true);
                    break;
            }
        }

        //TODO: setgold and wineskin

        //TODO: destroy the canvas
    }
}
