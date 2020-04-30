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
        Debug.Log("LEGEND CARD C");
        //A1
        //Skral stronghold - 50 + dice roll is the region num
        //Farmer placed on 28
        Waypoint waypoint28 = GameObject.Find("Waypoint (28)").GetComponent<Waypoint>();
        waypoint28.dropOneFarmer();
        //creatureManager.SpawnTowerSkral();

        headerText.text = "LEGEND CARD C";
        narratorCardText.text = "";
        infoPanel.SetActive(true);
        infoPanel.transform.SetPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);


        //C2
        //Gors placed on 27, 31
        creatureManager.Spawn(CreatureType.Gor, 27);
        creatureManager.Spawn(CreatureType.Gor, 31);
        //Skral placed on 29
        creatureManager.Spawn(CreatureType.Skral, 29);
        //Prince Thorald placed on 72
        heroManager.InitializeHero(HeroType.PrinceThorald, 72);
    }

    public void activateLegendCard_C(int regionNumber)
    {
        Debug.Log("LEGEND CARD C");
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
        Debug.Log("LEGEND CARD G");
        //Remove Prince Thorald
        //Wardraks placed on 26, 27 (10 Strength, 7 Willpower, 2 Black dice)
        creatureManager.Spawn(CreatureType.Wardrak, 26);
        creatureManager.Spawn(CreatureType.Wardrak, 27);
        headerText.text = "LEGEND CARD G";
        narratorCardText.text = "G – Prince Thorald joins up with a scouting patrol with the intention of leaving for just a few days. But he is not to be seen again for quite a long time. Prince Thorald is removed from the game. Black shadows are moving in the moonlight. The rumors were right – the wardraks are coming! Wardraks have been placed on 26 and 27. If one of the spaces is already occupied by a creature, the new creature is moved along the arrow to the adjacent space. A wardrak has 10 strength and 7 willpower points, and uses 2 black dice in battle. Identical dice values are added up. If the wardrak has fewer than 7 willpower points, it only has 1 black die available. These creatures are especially dangerous, because they move twice each sunrise, 1 space each time(see sunrise box). For every wardrak defeated, the heroes get a reward of 6 gold or 6 willpower points, or any combination of the two adding up to 6. Defeated wardraks are placed on space 80. \nTip: To prepare for a collective battle against this powerful creature, calculate your collective strength points beforehand and keep track!";
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
