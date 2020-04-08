using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCard : MonoBehaviour
{
    private int cardId;
    private string cardQuote;
    private string cardDescription;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setId(int i)
    {
        cardId = i;
    }

    public void setQuote(string s)
    {
        cardQuote = s;
    }

    public void setDescription(string s)
    {
        cardDescription = s;
    }
}
