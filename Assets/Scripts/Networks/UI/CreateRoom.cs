using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;


//followed tutorial online https://www.youtube.com/watch?v=IkozWbZo_wc&list=PLkx8oFug638oMagBH2qj1fXOkvBr6nhzt&index=7

public class CreateRoom : MonoBehaviourPunCallbacks
{
    //TODO: need to make sure this is not empty when creating a room
    [SerializeField]
    private Text roomName;


    //3.b create a room with options
    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            print("Log: not connected, on room creation");
            return;
        }

        if (string.IsNullOrWhiteSpace(roomName.text)) return;

        RoomOptions options = new RoomOptions();
        options.MaxPlayers = 4;
        PhotonNetwork.JoinOrCreateRoom(roomName.text, options, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        print("created room successfully.");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("Room creation failed: " + message);
    }
}
