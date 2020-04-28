using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour, Subject
{
    // List of Observers (Observer design pattern)
    private List<Observer> Observers = new List<Observer>();

    // List of chat messages
    private List<ChatMessage> Messages = new List<ChatMessage>();

    // Start is called before the first frame update
    void Start()
    {
        // Not used to ensure Managers are started in the right order.
        // GameManager calls Initialize instead.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {

    }

    // Creates a new message from the specified hero
    public void SendMessage(Hero Sender, string Text)
    {
        ChatMessage NewMessage = new ChatMessage(Sender, Text);
        Messages.Add(NewMessage);

        Notify("CHAT");
    }

    // Creates a new message from the system
    public void SendSystemMessage(string Text)
    {
        string ItalicText = "<i>" + Text + "</i>";
        ChatMessage NewMessage = new ChatMessage(null, ItalicText);
        Messages.Add(NewMessage);

        Notify("CHAT");
    }

    public List<ChatMessage> GetMessages()
    {
        return Messages;
    }

    // Used in Observer design pattern
    public void Attach(Observer o)
    {
        Observers.Add(o);
    }

    // Used in Observer design pattern
    public void Detach(Observer o)
    {
        Observers.Remove(o);
    }

    // Used in Observer design pattern
    public void Notify(string Category)
    {
        // Iterate through a copy of the observer list in case observers detach themselves during notify
        var ObserversCopy = new List<Observer>(Observers);

        foreach (Observer o in ObserversCopy)
        {
            o.UpdateData(Category);
        }
    }
}
