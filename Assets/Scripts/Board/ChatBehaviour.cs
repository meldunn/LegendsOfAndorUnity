using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatBehaviour
{

    [SerializeField]
    GameObject ChatUI = null;
    [SerializeField]
    TMP_Text ChatMessages = null;
    [SerializeField]
    TMP_InputField InputMessages = null;

    public List<string> ChatHistory = new List<string>();
    public string currentMessage = string.Empty;

     public void sendMessage()
    {
        if (true)
        {
            //networkView.RPC()
        }

    }
    
}
