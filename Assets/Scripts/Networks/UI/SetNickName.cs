using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SetNickName : MonoBehaviour
{

    [SerializeField]
    private Text playerName;
    

    // Start is called before the first frame update
    public void OnNameConfirmationClick()
    {
        if(!string.IsNullOrEmpty(playerName.text) && !string.IsNullOrWhiteSpace(playerName.text))
        {
            PhotonNetwork.NickName = playerName.text;
            CanvasManager.Instance.SwitchToCreateRoom();
        }
    }
}
