using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


//followed an online tutorial https://www.youtube.com/watch?v=0eAo4CIpiSw&list=PLkx8oFug638oMagBH2qj1fXOkvBr6nhzt&index=10
//autor: Vitaly
//TODO: update the list of rooms even if the rooms are not being created or updated.
public class PlayerListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform content;
    [SerializeField]
    private PlayerListing playerListing;
    [SerializeField]
    private GameObject startGameButton;

    private List<PlayerListing> currentPlayers = new List<PlayerListing>();

    public override void OnEnable()
    {
        base.OnEnable();
        GetCurrentRoomPlayers();

        if (PhotonNetwork.IsMasterClient)
            startGameButton.SetActive(true);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < currentPlayers.Count; i++)
        {
            Destroy(currentPlayers[i].gameObject);
        }

        currentPlayers.Clear();


        startGameButton.SetActive(false);
    }

    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected) return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null) return;


        foreach(KeyValuePair<int, Photon.Realtime.Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    private void AddPlayerListing(Photon.Realtime.Player newPlayer)
    {

        //checks if the player is already in the lobby
        int index = currentPlayers.FindIndex(x => x.Player == newPlayer);
        if(index != -1)
        {
            //update the info if the player already exists
            currentPlayers[index].SetPlayerInfo(newPlayer);
        }
        else
        {

            PlayerListing listing = Instantiate(playerListing, content);
            if (listing != null)
            {
                listing.SetPlayerInfo(newPlayer);
                currentPlayers.Add(listing);
            }

        }
    }

    // public override void OnPlayerEnteredRoom(Player newPlayer)
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)               // Removed override which caused compilation error
    {
        AddPlayerListing(newPlayer);
    }

    

    // public override void OnPlayerLeftRoom(Player otherPlayer)
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)               // Removed override which caused compilation error
    {
        //finds the player that left
        int index = currentPlayers.FindIndex(x => x.Player == otherPlayer);

        //the player that left is found
        if (index != -1)
        {
            Destroy(currentPlayers[index].gameObject);
            currentPlayers.RemoveAt(index);
        }


        ////if master client leaves, check if this client becomes master
        if (PhotonNetwork.IsMasterClient)
            startGameButton.SetActive(true);
    }

    public void OnClick_LeaveRoom()
    {
        PhotonNetwork.LeaveRoom(true);
        print("left room");
        CanvasManager.Instance.SwitchCanvases();
    }

    public void OnClick_StartGame()
    {
        if (!PhotonNetwork.IsConnected) return;

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }   
    }
}
