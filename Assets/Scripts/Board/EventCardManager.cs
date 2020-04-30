using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class EventCardManager : MonoBehaviourPun
{
    private EventCard[] eventCards = new EventCard[9];
    private System.Random random = new System.Random();
    private List<int> usedCards = new List<int>();
    private int counter = 0;

    [SerializeField]
    GameObject infoPanel;
    [SerializeField]
    Text quoteText;
    [SerializeField]
    Text descriptionText;

    string[] cardQuotes = new string[]
    {
        "A biting wind blows across the coast from the sea.",
        "Poisonous vapors from the mountains are tormenting the heroes.",
        "A farm girl sings a beautiful song that wafts across the northern woods. But it fails to stir the hearts of all the heroes.",
        "The lovely sound of a horn echoes across the land.",
        "A fragment of a very old sculpture has been found. Not all of the heroes are able to appreciate that kind of handiwork.",
        "Heavy weather moves across the land.",
        "A beautifully clear, starry night gives the heroes confidence.",
        "Hot rain from the south lashes the land.",
        "A sleepless night awaits the heroes.",
    };

    string[] cardDescriptions = new string[]
    {
        "Each hero standing on a space with a number between 0 and 20 now loses 3 willpower points.",
        "Each hero standing on a space with a number between 37 and 70 now loses 3 willpower points.",
        "The wizard and the archer each immediately get 3 willpower points.",
        "Each hero who has fewer than 10 willpower points can immediately raise his total to 10.",
        "The dwarf and the warrior immediately get 3 willpower points each.",
        "Each hero with more than 12 willpower points immediately reduces his point total to 12.",
        "Every hero whose time marker is presently in the sunrise box gets 2 willpower points.",
        "Any hero who is not on a forest space, in the mine (space 71), in the tavern (space 72), or in the castle (space 0) loses 2 willpower points.",
        "Every hero whose time marker is presently in the sunrise box loses 2 willpower points.",
    };

    public void Initialize()
    {
        for (int i = 0; i < 9; i++)
        {
            eventCards[i] = new EventCard(i + 1, cardQuotes[i], cardDescriptions[i]);
        }
    }

    public void triggerRandom()
    {
        //if all cards are used, reset
        if (counter == 8)
            usedCards.Clear();
        //Get random index, check if card has already been called
        int index = -1;
        do
        {
            index = random.Next(9);
        } while (usedCards.Contains(index));

        if (PhotonNetwork.IsConnected && photonView.IsMine)
        {
            photonView.RPC("triggerRandomRPC", RpcTarget.All, index);
        }
        else
        {
            triggerRandomRPC(index);
        }
    }

    [PunRPC]
    private void triggerRandomRPC(int index)
    {
        //Update the text fields
        quoteText.text = eventCards[index].getQuote();
        descriptionText.text = eventCards[index].getDescription();
        //Display the panel
        infoPanel.SetActive(true);
        infoPanel.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);

        eventCards[index].trigger();
        usedCards.Add(index);
        counter++;
    }

    public void hide()
    {
        infoPanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
