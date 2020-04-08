using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


//Autor: Vitaly

public class PlayerSelector : MonoBehaviourPun, IPunOwnershipCallbacks
{
    [SerializeField]
    private GameObject readyToken;
    [SerializeField]
    private Button next;
    [SerializeField]
    private Button prev;
    [SerializeField]
    private Image hero;

    [Header("HeroSprites")]
    [SerializeField]
    private Sprite[] heroSprites;





    public bool isReady { get; private set; }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        throw new System.NotImplementedException();
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        //if (targetView.IsMine)
        //{
        //    next.gameObject.SetActive(true);
        //    prev.gameObject.SetActive(true);
        //}
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            next.gameObject.SetActive(true);
            prev.gameObject.SetActive(true);
        }
    }
}
