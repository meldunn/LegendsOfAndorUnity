using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class LegendCardManager : MonoBehaviourPun
{
    GameManager gameManager;
    CreatureManager creatureManager;
    HeroManager heroManager;
    RuneStoneMenu RuneStoneMenu;

    [SerializeField]
    GameObject infoPanel;
    [SerializeField]
    Text headerText;
    [SerializeField]
    Text narratorCardText;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        heroManager = GameObject.Find("HeroManager").GetComponent<HeroManager>();
        creatureManager = GameObject.Find("CreatureManager").GetComponent<CreatureManager>();
        RuneStoneMenu = GameObject.Find("RuneStoneMenu").GetComponent<RuneStoneMenu>();
        //infoPanel = GameObject.Find("NarratorPopup");
        //headerText = GameObject.Find("HeaderText").GetComponent<Text>();
        //narratorCardText = GameObject.Find("NarratorCardText").GetComponent<Text>();
        //infoPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Activated on Narrator Space D
    public void activateRuneStoneLegendCard()
    {
        if(PhotonNetwork.IsConnected)
        {
            if(PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Rune Stone Menu Activated");
                Vector3 Origin = new Vector3(0,0,0);
                RuneStoneMenu.transform.Translate(Origin - 
                        RuneStoneMenu.transform.position);
            }
        }
        else
        {
            Debug.Log("Rune Stone Menu Activated");
            Vector3 Origin = new Vector3(0,0,0);
            RuneStoneMenu.transform.Translate(Origin - 
                    RuneStoneMenu.transform.position);
        }
    }

    public void activateLegendCard_A()
    {
        //Debug.Log("LEGEND CARD A");
        headerText.text = "LEGEND CARD A";
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //gameManager.getd
        if (gameManager.GetDifficulty() == DifficultyLevel.Easy)
        {
            narratorCardText.text = "A3 – A gloomy mood has fallen upon the people. Rumors are making the rounds that skrals have set up a stronghold in some undisclosed location. The heroes have scattered themselves across the entire land in search of this location. The defense of the castle is in their hands alone. \nThe heroes have been placed on the following regions (if they are being used): dwarf on 7, warrior on 14, archer on 25, wizard on 34.\nGors placed on spaces 8, 20, 21, 26, 48 and one skral on 19. \nMany farmers have asked for help and are seeking shelter behind the high walls of Reitburg Castle. One farmer token on space 24.";
        } 
        else
        {
            narratorCardText.text = "A3 – A gloomy mood has fallen upon the people. Rumors are making the rounds that skrals have set up a stronghold in some undisclosed location. The heroes have scattered themselves across the entire land in search of this location. The defense of the castle is in their hands alone. \nThe heroes have been placed on the following regions (if they are being used): dwarf on 7, warrior on 14, archer on 25, wizard on 34.\nGors placed on spaces 8, 20, 21, 26, 48 and one skral on 19. \nMany farmers have asked for help and are seeking shelter behind the high walls of Reitburg Castle. One farmer token on space 24 and 36.";
        }
        narratorCardText.text = narratorCardText.text + "\n\nA4 – This adventure starts with farmers who can be brought into the castle. The players can move through their heroes to a farmer token and carry it along with their own figure. They can also do that if they just pass through a space with a farmer token. A hero can carry several farmer tokens at one time. If a hero carrying a farmer token moves to a space with a creature or if a creature enters a space with a farmer, the farmer is immediately killed and removed from the game.The heroes can leave a farmer behind on a space at any time. Farmers who have been saved offer a great advantage: For each farmer brought into the castle, one extra creature can get into the castle without loss of the Legend.The farmer token is simply flipped onto its rear side and placed next to the golden shields. At first sunlight, the heroes receive a message: Old King Brandur’s willpower seems to have weakened with the passage of time. But there is said to be an herb growing in the mountain passes that can revive a person’s life.\nTask: The heroes must heal the king with the medicinal herb. To do that, you  must find the witch. Only the she knows the locations where this herb grows. The witch is hiding behind one of the fog tokens. \n\n A5 – When a hero enters a space with the fog token showing the witch’s brew, “The Witch” card is uncovered and read out loud. Note: There are 2 fog tokens that will bring a gor into play.When a hero activates one of those tokens, a gor is placed on that space. Now it’s time to decide when “The Rune Stones” Legend Card comes into play.One player rolls one of the herp dice. Note the little dice pips shown in the Legend track with its arrow pointing to the corresponding letter on the Legend track(matching the result of the rolled die).This card will be triggered when the narrator reaches this letter. Important: From now on, any articles(in addition to strength points) may be purchased from the merchants(spaces 18, 57 and 71) for 2 goals each.See the equipment board for the functions of the articles. “Witch’s brew”, however cannot be purchased from the merchant. Each hero starts with 2 strength points.The group gets 5 gold and 2 wineskins.You all decide together who gets what. The hero with the lowest rank will now begin.";
        infoPanel.SetActive(true);
        infoPanel.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void activateLegendCard_C(int regionNumber)
    {
        //Debug.Log("LEGEND CARD C");
        //C1
        //Skral stronghold - 50 + dice roll is the region num
        //Farmer placed on 28
        Waypoint waypoint28 = GameObject.Find("Waypoint (28)").GetComponent<Waypoint>();
        waypoint28.dropOneFarmer();
        //creatureManager.SpawnTowerSkral();
        creatureManager.Spawn(CreatureType.TowerSkral, regionNumber);
        WaypointManager wpManager = GameObject.Find("WaypointManager").GetComponent<WaypointManager>();
        int strength = wpManager.GetWaypoint(regionNumber).GetCreature().GetStrength();

        headerText.text = "LEGEND CARD C";
        narratorCardText.text = "C1 – The Skral Stronghold has been placed on " + regionNumber + ". The heroes may enter and pass through this space. The skral does not move at sunrise. Other creatures that would move into the space are instead advanced along the arrow to the next space. The skral on the tower has 6 willpower points and " + strength + " strength points. Task: The skral on the tower must be defeated. As soon as he is defeated, the Narrator is advanced to the letter “N” on the Legend track. And there’s more unsettling news: Rumors are circulating about cruel wardraks from the south. They have not been sighted, but more and more farmers are losing their courage, leaving their farmsteads, and seeking safety in the castle. A farmer has been spawned on 28.\n\nC2 – Gors have been placed on 27 and 31, and one skral on 29. But there’s good news from the south too: Prince Thorald, just back from a battle on the edge of the southern forest, is preparing himself to help the heroes. Prince Thorald has been placed on 72. If the prince is standing on the same space as a creature, he counts as 4 extra strength points for the heroes in a battle with the creature. Instead of “fighting” or “moving”, a hero can now also choose the “move prince” action during his move. That will cost him 1 hour on the time track. He can move the prince up to 4 spaces. He can also do that several times during his turn (for example, move the prince up to 8 spaces at a cost of 2 hours). After the “move prince” action, it is the next hero’s turn. Note: Prince Thorald cannot collect any tokens or move any farmers. Prince Thorald accompanies the heroes up to letter “G” on the Legend track.\n Legend goal: The Legend is won when the Narrator reaches the letter “N” on the Legend track, and…\n    ·The castle has been defended, and…\n    ·The medicinal herb is on the castle space, and… \n    ·The skral on the tower has been defeated.";
        infoPanel.SetActive(true);
        infoPanel.transform.SetPositionAndRotation(new Vector3(0,0,0), Quaternion.identity);


        //C2
        //Gors placed on 27, 31
        creatureManager.Spawn(CreatureType.Gor, 27);
        creatureManager.Spawn(CreatureType.Gor, 31);
        //Skral placed on 29
        creatureManager.Spawn(CreatureType.Skral, 29);
        //Prince Thorald placed on 72
        heroManager.InitializeHero(HeroType.PrinceThorald, 72);
    }

    public void activateLegendCard_G()
    {
        //Debug.Log("LEGEND CARD G");
        //Remove Prince Thorald
        //Wardraks placed on 26, 27 (10 Strength, 7 Willpower, 2 Black dice)
        creatureManager.Spawn(CreatureType.Wardrak, 26);
        creatureManager.Spawn(CreatureType.Wardrak, 27);
        headerText.text = "LEGEND CARD G";
        narratorCardText.text = "G – Prince Thorald joins up with a scouting patrol with the intention of leaving for just a few days. But he is not to be seen again for quite a long time. Prince Thorald is removed from the game. Black shadows are moving in the moonlight. The rumors were right – the wardraks are coming! Wardraks have been placed on 26 and 27. If one of the spaces is already occupied by a creature, the new creature is moved along the arrow to the adjacent space. A wardrak has 10 strength and 7 willpower points, and uses 2 black dice in battle. Identical dice values are added up. If the wardrak has fewer than 7 willpower points, it only has 1 black die available. These creatures are especially dangerous, because they move twice each sunrise, 1 space each time(see sunrise box). For every wardrak defeated, the heroes get a reward of 6 gold or 6 willpower points, or any combination of the two adding up to 6. Defeated wardraks are placed on space 80. \nTip: To prepare for a collective battle against this powerful creature, calculate your collective strength points beforehand and keep track!";
        heroManager.destroyThorald();
        //HeroManager.Destroy()
        infoPanel.SetActive(true);
    }

    public void activateLegendCard_N()
    {
        Debug.Log("LEGEND CARD N");
        //End game (win/lose)
    }

    public void closePopup()
    {
        infoPanel.SetActive(false);
    }
}
