using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendCardManager : MonoBehaviour
{
    GameManager gameManager;
    CreatureManager creatureManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        creatureManager = GameObject.Find("CreatureManager").GetComponent<CreatureManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void activateLegendCard_C()
    {
        Debug.Log("LEGEND CARD C");
        //C1
        //Skral stronghold - 50 + dice roll is the region num
        //Farmer placed on 28
        Waypoint waypoint28 = GameObject.Find("Waypoint (28)").GetComponent<Waypoint>();
        waypoint28.dropOneFarmer();
        creatureManager.SpawnTowerSkral();

        //C2
        //Gors placed on 27, 31
        creatureManager.Spawn(CreatureType.Gor, 27);
        creatureManager.Spawn(CreatureType.Gor, 31);
        //Skral placed on 29
        creatureManager.Spawn(CreatureType.Skral, 29);
        //Prince Thorald placed on 72
    }

    public void activateLegendCard_G()
    {
        Debug.Log("LEGEND CARD G");
        //Remove Prince Thorald
        //Wardraks placed on 26, 27 (10 Strength, 7 Willpower, 2 Black dice)
        creatureManager.Spawn(CreatureType.Wardrak, 26);
        creatureManager.Spawn(CreatureType.Wardrak, 27);
    }

    public void activateLegendCard_N()
    {
        Debug.Log("LEGEND CARD N");
        //End game (win/lose)
    }
}
