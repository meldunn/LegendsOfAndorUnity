using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//Autor: Vitaly



public class HeroSelectionManager : MonoBehaviourPun
{

    [SerializeField]
    private Transform[] heroSelectorPositions;
    [SerializeField]
    private Transform parentCanvas;
    private bool[] slotStatus = new bool[4]; //true for occupied, false for not occupied

    [SerializeField]
    private GameObject selectorPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //var playerID = PhotonNetwork.PlayerList;
        print("we got here");
        photonView.RPC("InstantiateSelector", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber - 1);

        //print(id);
        //InstantiateFlame();

        //heroSelector.transform.SetParent(parentCanvas);
        //heroSelector.transform.localScale = Vector3.one;

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    for (int i = 0; i < slotStatus.Length; i++)
        //    {
        //        //if not occupied
        //        if (!slotStatus[i])
        //        {

        //            heroSelector.GetComponent<PhotonView>().TransferOwnership
        //            heroSelector.transform.SetParent(parentCanvas);
        //            heroSelector.transform.localScale = Vector3.one;
        //            slotStatus[i] = true; //set as occupied
        //        }
        //    }
        //    PhotonNetwork.LocalPlayer.ActorNumber
        //}


        //player_0.selectorOwner = PhotonNetwork.CurrentRoom.Players[0];


    }

    [PunRPC] 
    void InstantiateSelector(int id)
    {
        print("we got here2");
        GameObject heroSelector = Instantiate(selectorPrefab, heroSelectorPositions[id].position, heroSelectorPositions[id].rotation, heroSelectorPositions[id]);
        heroSelector.GetPhotonView().TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);
    }
}
