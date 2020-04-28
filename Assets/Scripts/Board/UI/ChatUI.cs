using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ChatUI : MonoBehaviourPun, Observer
{
    // Reference to managers
    private GameManager GameManager;
    private HeroManager HeroManager;
    private ChatManager ChatManager;

    // References to children components
    [SerializeField]
    private GameObject ChatMessageViewer = null;

    // Start is called before the first frame update
    void Start()
    {
        // Not used to ensure UI elements are initialized in the right order.
        // UIManager calls Initialize instead.
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize()
    {
        // Initialize reference to managers
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        HeroManager = GameObject.Find("HeroManager").GetComponent<HeroManager>();
        ChatManager = GameObject.Find("ChatManager").GetComponent<ChatManager>();

        // Register as an observer of ChatManager
        ChatManager.Attach(this);

        // Initialize UI
        UpdateChat();
    }

    // Used in Observer design pattern
    public void UpdateData(string Category)
    {
        if (string.Equals(Category, "CHAT"))
        {
            UpdateChat();
        }
    }

    // Updates the chat with the latest 100 messages
    private void UpdateChat()
    {
        List<ChatMessage> Messages = ChatManager.GetMessages();

        // Formatted string that will fill the viewer
        string RefreshedMessages = "";

        // Start at the end and get the last 100 messages
        for (int i = Messages.Count - 1; i >= Math.Max(Messages.Count - 100, 0); i--)
        {
            ChatMessage Message = Messages[i];

            string MessageString = Message.ToString();

            if (string.Equals(RefreshedMessages, "")) RefreshedMessages = MessageString;
            else RefreshedMessages = MessageString + "\n" + RefreshedMessages;
        }

        // Set the messages in the viewer
        SetText(ChatMessageViewer, RefreshedMessages);
    }

    public void SetText(GameObject Display, string Text)
    {
        TextMeshProUGUI InfoText = Display.GetComponent<TextMeshProUGUI>();
        InfoText.SetText(Text);
    }

    public void SendChatMessage(GameObject Input)
    {
        // Get the message string based on the input
        string Message = Input.GetComponent<TMP_InputField>().text;

        // Clear the message from the input
        Input.GetComponent<TMP_InputField>().text = "";

        // Only send a non-blank message
        if (!String.Equals(Message, ""))
        {
            // Get the hero type of the message sender
            HeroType MyHeroType = GameManager.GetSelfHero().GetHeroType();

            // Send the message
            if (PhotonNetwork.IsConnected) photonView.RPC("SendChatRPC", RpcTarget.All, MyHeroType, Message);
            else SendChatRPC(MyHeroType, Message);

        }
    }

    // NETWORKED
    [PunRPC]
    private void SendChatRPC(HeroType Sender, string Message)
    {
        Hero SenderHero = HeroManager.GetHero(Sender);
        ChatManager.SendMessage(SenderHero, Message);
    }
}
