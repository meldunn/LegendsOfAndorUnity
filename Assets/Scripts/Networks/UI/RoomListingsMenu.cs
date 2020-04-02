using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


//followed an online tutorial https://www.youtube.com/watch?v=AbGwORylKqo&list=PLkx8oFug638oMagBH2qj1fXOkvBr6nhzt&index=8
public class RoomListingsMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform content;

    //prefab
    [SerializeField]
    private RoomListing roomListing;

    private List<RoomListing> currentRoomListings = new List<RoomListing>();


    //Switching between menus
    //4 joining a room, switch canvases 
    public override void OnJoinedRoom()
    {
        //delete all the room names
        for (int i = 0; i < currentRoomListings.Count; i++)
        {
            Destroy(currentRoomListings[i].gameObject);
        }

        currentRoomListings.Clear();

        CanvasManager.Instance.SwitchCanvases();
    }

    //after joining/creating room, you don't get anymore room updates.
    //2. receive updates about current rooms
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            //removed from rooms list
            if (info.RemovedFromList)
            {
                //finds the index of destroyed room
                int index = currentRoomListings.FindIndex(x => x.RoomInfo.Name == info.Name);

                //if the destroyed room is found
                if (index != -1)
                {
                    print("Removed a room");
                    Destroy(currentRoomListings[index].gameObject);
                    currentRoomListings.RemoveAt(index);
                }
            }
            //added to rooms list
            else
            {
                int index = currentRoomListings.FindIndex(x => x.RoomInfo.Name == info.Name);

                //add a room listing only if it does not yet exist
                if(index == -1)
                {
                    RoomListing listing = Instantiate(roomListing, content);
                    if (listing != null)
                    {
                        print("Added a room");
                        listing.SetRoomInfo(info);
                        currentRoomListings.Add(listing);
                    }
                }
            }
        }
    }
}
