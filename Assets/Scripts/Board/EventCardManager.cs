using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCardManager : MonoBehaviour
{
    private EventCard[] eventCards = new EventCard[34];

    string[] cardQuotes = new string[]
    {
        "The dwarf merchant Garz makes an offer.",
        "A biting wind blows across the coast from the sea.",
        "Wisdom from the Tree of Songs.",
        "Poisonous vapors from the mountains are tormenting the heroes.",
        "A farm girl sings a beautiful song that wafts across the northern woods. But it fails to stir the hearts of all the heroes.",
        "The dwarf merchant Garz invites one of the heroes to have a drink.",
        "Sulfurous mist surround the heroes.",
        "Trading ships reach the coast of Andor.",
        "Dark clouds cover the sun, filling all the good people of Andor with a strange foreboding.",
        "Jugglers from the north display their art.",
        "The creatures gather their strength.",
        "The hereos replenish their water supplies at the river.",
        "The lovely sound of a horn echoes across the land.",
        "A fragment of a very old sculpture has been found. Not all of the heroes are able to appreciate that kind of handiwork.",
        "Rampaging creatures despoil the well in the south of Andor.",
        "Royal falcons fly high above the land, keeping watch.",
        "Heavy weather moves across the land.",
        "A wild gor storms forth.",
        "An exhausting day.",
        "A farmer falls ill.",
        "A mysterious terror lurks in the southern woods.",
        "Rampaging creatures despoil the well at the foot of the mountains.",
        "The king's blacksmiths are laboring tirelessly.",
        "A storm moves across the countryside and weighs upon the mood of the heroes.",
        "Keeper Melkart's generosity.",
        "The minstrels sing a ballad about the deeds of the heroes, strengthening their determination.",
        "The creatures are possessed with blind fury.",
        "A beautifully clear, starry night gives the heroes confidence.",
        "The keepers of the Tree of Songs offer a gift.",
        "A drink in the tavern.",
        "Hot rain from the south lashes the land.",
        "A sleepless night awaits the heroes.",
        "Their adventure is wearing down the heroes.",
        "The dwarf merchant Garz meets one of the heroes and offers him a trade."
    };

    string[] cardDescriptions = new string[]
    {
        "Each hero may now purchase any article from the equipment board (except the witch's brew) in exchange for 3 willpower points.",
        "Each hero standing on a space with a number between 0 and 20 now loses 3 willpower points.",
        "A hero who enters the Tree of Songs space or is already standing there gets 1 strength point. If more than one hero is standing there, the one with the highest rank gets the strength point. Now place this card on space 57 until a hero has gotten the strength point. Then remove it from the game.",
        "Each hero standing on a space with a number between 37 and 70 now loses 3 willpower points.",
        "The wizard and the archer each immediately get 3 willpower points.",
        "The hero with the lowest rank gets to decide if he wants to roll one of his hero dice. If he rolls 1,2,3, or 4, he loses the rolled number of willpower points. If he rolls 5 or 6, he wins that number of willpower points.",
        "The hero with the lowest rank rolls one of his hero dice. The group loses the rolled number of willpower points.",
        "A hero who enters space 9 or is already standing there can buy 2 strength points there for just 2 gold. Place this card on space 9 until a hero has made the purchase.Then remove it from the game.",
        "On this day, no hero is allowed to use a 10th hour. Place this card above the overtime area of the time track. At the end of the day, it is removed from the game.",
        "Each hero can now purchase 3 willpower points in exchange for 1 gold.",
        "On this day, each creatures has 1 extra strength point. Place this card next to the creature display. At the end of the day, it is removed from the game.",
        "Each hero who is now standing on a space bordering the river gets a wineskin.",
        "Each hero who has fewer than 10 willpower points can immediately raise his total to 10.",
        "The dwarf and the warrior immediately get 3 willpower points each.",
        "The well token on space 35 is removed from the game.",
        "The hero with the highest rank is allowed to take a look at the top card on the event card deck. Then he gets to decide whether to remove the card from the game or to place it back on the deck.",
        "Each hero with more than 12 willpower points immediately reduces his point total to 12.",
        "The gor on the space with the lowest number now moves one space in the direction of the arrow. The group can prevent that by paying willpower points: 2 heroes = 2 willpower points,  3 heroes = 4 willpower points,  4 heroes = 6 willpower points.",
        "On this day, the 9th and 10th hours will each cost 3 willpower points instead of 2. Place this card above the overtime area of the time track. At the end of the day, it is removed from the game.",
        "One farmer token on the game board that has not yet been taken to the castle must be removed from the game. The group can prevent that by paying gold and/or willpower points: 2 heroes = 2 gold/willpower points, 3 heroes = 3 gold/willpower points, 4 heroes = 4 gold/willpower points.",
        "A hero who enters space 22, 23, 24, or 25 or is already standing there will immediately lose 4 willpower points. If more than one hero is standing there, the one with the highest rank loses the points. Place this card next to space 24 until it is triggered. Then it is removed from the game.",
        "The well token on space 45 is removed from the game.",
        "Up to two heroes with 6 or fewer strength points can each add 1 strength point to what they already have. You can decide as a group which hereos those will be.",
        "Any hero who is not on a forest space, in the mine (space 71), in the tavern (space 72), or in the castle (space 0) loses 2 willpower points.",
        "Any hero with fewer than 6 willpower points rolls a hero dies and gets the rolled number of willpower points.",
        "On this day, the 8th hour costs no willpower points. Place this card above the overtime area of the time track. At the end of the day, it is removed from the game.",
        "The creature standing on the space with the highest number of will now move one space along the arrow. The group can prevent that by paying gold and/or willpower points: 2 heroes = 2 gold/willpower points, 3 heroes = 3 gold/willpower points, 4 heroes = 4 gold/willpower points.",
        "Every hero whose time marker is presently in the sunrise box gets 2 willpower points.",
        "Now place a shield on space 57. A hero who enters space 57 or is already standing there can collect the shield and place it on the large storage space on his hero board. If more than one hero is standing there, the hero with the lowest rank gets the shield.",
        "Place a wineskin on the tavern space (72). A hero who enters space 72 or is already standing there can collect the wineskin and place it on the small storage space on his hero board. If more than one hero is standing there, the hero with the lowest rank gets the wineskin.",
        "Any hero who is not on a forest space, in the mine (space 71), in the tavern (space 72), or in the castle (space 0) loses 2 willpower points.",
        "Every hero whose time marker is presently in the sunrise box loses 2 willpower points.",
        "One of the heroes immediately loses 1 strength point. You can decide as a group which hero that will be. If no hero has more than 1 strength point, nothing happens.",
        "One of the heroes can now purchase 10 willpower points in exchange for 2 strength points. You can decide as a group which hero that will be."
    };

    public void Initialize()
    {
        for(int i = 0; i < 34; i++)
        {
            eventCards[i].setId(i + 1);
            eventCards[i].setQuote(cardQuotes[i]);
            eventCards[i].setDescription(cardDescriptions[i]);
        }
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
