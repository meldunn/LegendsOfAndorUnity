using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


//Autor: Vitaly

public class PlayerSelector : MonoBehaviourPun
{
    [SerializeField]
    public GameObject readyToken { get; private set; }
    [SerializeField]
    public Button next { get; private set; }
    [SerializeField]
    public Button prev { get; private set; }
    [SerializeField]
    public Image hero { get; private set; }

    [Header("HeroSprites")]
    [SerializeField]
    private Sprite[] heroSprites;

    [HideInInspector]
    public Photon.Realtime.Player selectorOwner;






    public bool isReady { get; private set; }

    private void Start()
    {
       
    }
}
