using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private NetworkSettings networkSettings;

    //TODO: fix creation of rooms on leave
    //TODO: make so that only master player can start the game.

    //1. Connect to master
    void Start()
    {
        print("Connecting...");
        PhotonNetwork.NickName = networkSettings.NickName;
        PhotonNetwork.GameVersion = networkSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        print("Connected");
        print(PhotonNetwork.LocalPlayer.NickName);

        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected" + cause.ToString());
    }

}
