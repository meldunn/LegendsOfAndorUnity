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

        headerText.text = "LEGEND CARD C";
        narratorCardText.text = "NARRATOR CARD C";
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
        narratorCardText.text = "NARRATOR CARD G";
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
