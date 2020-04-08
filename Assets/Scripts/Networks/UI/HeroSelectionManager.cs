using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

//Autor: Vitaly



public class HeroSelectionManager : MonoBehaviourPun
{

    [SerializeField]
    private PlayerSelector player_0;
    [SerializeField]
    private PlayerSelector player_1;
    [SerializeField]
    private PlayerSelector player_2;
    [SerializeField]
    private PlayerSelector player_3;







    // Start is called before the first frame update
    void Start()
    {
        player_0.selectorOwner = PhotonNetwork.CurrentRoom.Players[0];


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
