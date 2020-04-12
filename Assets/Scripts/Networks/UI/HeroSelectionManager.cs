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

    //ready logic
    int readyPlayers = 0;
    Dictionary<int, HeroType> selectedHeroes;



    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        selectedHeroes = new Dictionary<int, HeroType>();

        int id = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        GameObject heroSelectorInstance = PhotonNetwork.Instantiate("HeroSelectionGUI", heroSelectorPositions[id].position, heroSelectorPositions[id].rotation);


        //at run time subsribes the ready up logic. (done on run time because we are dealing with a prefab)

        readyUp.onClick.AddListener(heroSelectorInstance.GetComponent<PlayerSelector>().OnClick_Ready);
       
    }

    public void OnPlayerReady(int playerID, HeroType type, bool status)
    {
        print("INVOKED");
        if (status)
        {
            readyPlayers++;
            selectedHeroes.Add(playerID, type);
        }
        else
        {
            readyPlayers--;
            selectedHeroes.Remove(playerID);
        }

        if (readyPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            if (areDifferentHeroes())
            {
                print("CAN START THE GAME");
            }
        }


    }


    //TODO: to be tested if works properly
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        readyPlayers--;
        selectedHeroes.Remove(otherPlayer.ActorNumber);
    }

    bool areDifferentHeroes()
    {
        for (int i = 0; i < selectedHeroes.Count - 1; i++)
        {
            for (int j = i + 1; j < selectedHeroes.Count; j++)
            {
                if (selectedHeroes[i] == selectedHeroes[j])
                {
                    return false;
                }
            }
        }

        return true;
    }

}
