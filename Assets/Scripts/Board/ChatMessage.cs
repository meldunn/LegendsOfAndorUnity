using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatMessage
{
    // The message sender
    private Hero Sender;

    // The message contents
    private string Text;

    // Constructor
    public ChatMessage(Hero Sender, string Text)
    {
        this.Sender = Sender;
        this.Text = Text;
    }

    public string ToString()
    {
        string Formatted = "";

        if (Sender != null) Formatted += Sender.GetHeroType() + ": ";
        if (Text != null) Formatted += Text;

        return Formatted;
    }
}
