using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListing : MonoBehaviour
{
    [SerializeField]
    private Text nickName;

    public Player Player { get; private set; }

    public void SetPlayerInfo(Player player)
    {
        this.Player = player;
        // nickName.text = player.NickName;         // Commenting out due to compilation error
    }
}
