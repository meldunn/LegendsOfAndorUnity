using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NetworkSettings")]
public class NetworkSettings : ScriptableObject
{
    [SerializeField]
    private string gameVersion = "0.0.0";
    public string GameVersion { get { return gameVersion; } }
    [SerializeField]
    private string nickName = "vitaly";
    public string NickName { get { return nickName; } }

}
