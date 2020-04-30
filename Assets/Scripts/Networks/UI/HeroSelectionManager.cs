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

    [SerializeField]
    private GameObject SavedGamesPanel;

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

        Debug.Log("Hero Stuffs");
        //at run time subsribes the ready up logic. (done on run time because we are dealing with a prefab)
        readyUp.onClick.AddListener(heroSelectorInstance.GetComponent<PlayerSelector>().OnClick_Ready);

        Vector3 Pos = new Vector3(-250, 0, 0);
        SavedGamesPanel = GameObject.Find("SGP");
        SavedGamesPanel.transform.Translate(Pos - SavedGamesPanel.transform.position);

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
                bool ShowSavedGames = SavedGamesAllowed();
                photonView.RPC("InstantiateSplitResources", RpcTarget.All, ShowSavedGames);
            }
        }
    }

    [PunRPC]
    private void InstantiateSplitResources(bool ShowSavedGames)
    {
        if(PhotonNetwork.IsMasterClient && ShowSavedGames)
        {
            Vector3 Pos = new Vector3(-250, 0, 0);
            SavedGamesPanel = GameObject.Find("SGP");
            SavedGamesPanel.transform.Translate(Pos - SavedGamesPanel.transform.position);

        }

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

    bool SavedGamesAllowed()
    {
        bool Archer = false;
        bool Warrior = false;
        bool Dwarf = false;
        foreach (var player in selectedHeroes)
        {
            if(player.Value == HeroType.Archer) Archer = true;
            if(player.Value == HeroType.Warrior) Warrior = true;
            if(player.Value == HeroType.Dwarf) Dwarf = true;
        }

        if(Archer && Warrior && Dwarf && PhotonNetwork.CurrentRoom.PlayerCount == 3) return true;

        return false;
    }

    public void OnClick_NormalDifficulty()
    {
        //RPC
        photonView.RPC("InitializeGameManager", RpcTarget.All, DifficultyLevel.Normal, -1);
    }

    public void OnClick_EasyDifficulty()
    {
        photonView.RPC("InitializeGameManager", RpcTarget.All, DifficultyLevel.Easy, -1);
    }

    public void LoadGame(int SaveGameID)
    {
        photonView.RPC("InitializeGameManager", RpcTarget.All, DifficultyLevel.Easy, SaveGameID);
    }


    //TODO: send info another way
    //TODO: use setdifficulty function
    //todo: quit to main screen
    [PunRPC]
    void InitializeGameManager(DifficultyLevel level, int SaveGameID)
    {
        GameManager.Instance.SetDifficulty(level);

        int playerID = PhotonNetwork.LocalPlayer.ActorNumber;

        GameManager.Instance.SetSelfHero(selectedHeroes[playerID]);

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

        GameManager.Instance.GetSelfHero().ReceiveGold(coinsSplit[playerID]);

        GameManager.Instance.GetSelfHero().GetInventory()[ItemType.Wineskin] += wineSplit[playerID];
        
        switch (SaveGameID)
        {
            case(1):
                GameManager.Instance.MerchantSavedGame();
                break;
            case(2):
                GameManager.Instance.FightingSavedGame();
                break;
            case(3):
                GameManager.Instance.LoseSavedGame();
                break;
            case(4):
                GameManager.Instance.WinSavedGame();
                break;
            default:
                break;
        }

        //TODO: setgold and wineskin

        //TODO: destroy the canvas
        Destroy(heroSelectCanvas);

    }
}
