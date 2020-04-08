using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;


//Autor: Vitaly

public class PlayerSelector : MonoBehaviourPun
{
    [SerializeField]
    private GameObject readyToken;
    [SerializeField]
    private Button next;
    [SerializeField]
    private Button prev;
    [SerializeField]
    private Image hero;
    [SerializeField]
    private Text nickName;

    [Header("HeroSprites")]
    [SerializeField]
    private Sprite[] heroSprites;
    private int currentHero;


    public bool isReady { get; private set; }



    public void OnEnable()
    {
        if (photonView.IsMine)
        {
            next.gameObject.SetActive(true);
            prev.gameObject.SetActive(true);
            photonView.RPC("InstantiateSelector", RpcTarget.All, PhotonNetwork.LocalPlayer.ActorNumber - 1);
        }
    }


    //adjusts the tranforms
    [PunRPC]
    void InstantiateSelector(int id)
    {
        currentHero = 0;
        hero.sprite = heroSprites[currentHero];
        nickName.text = photonView.Owner.NickName;

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
        if(currentHero == 4)
        {
            currentHero = 0;
        }
        hero.sprite = heroSprites[currentHero];
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
    }

    public void OnClick_Ready()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("ReadyUp", RpcTarget.All);
        }
    }

    [PunRPC]
    void ReadyUp()
    {
        bool isEnabled = readyToken.activeSelf;
        readyToken.SetActive(!isEnabled);
    }

}
