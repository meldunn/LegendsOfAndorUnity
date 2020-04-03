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

    [SerializeField]
    private string app_id = "d4408bb2-cc46-441f-a12b-33f9f0571f95";
    public string AppID { get { return app_id; } }

}
