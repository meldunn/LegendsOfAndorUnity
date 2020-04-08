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

    [Header("HeroSprites")]
    [SerializeField]
    private Sprite[] heroSprites;





    public bool isReady { get; private set; }


  
    public void Initialize()
    {
        //check if the ownership was transfered
        if (photonView.IsMine)
        {
            next.gameObject.SetActive(true);
            prev.gameObject.SetActive(true);
        }
    }
}
