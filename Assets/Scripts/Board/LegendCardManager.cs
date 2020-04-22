using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendCardManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

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

        //C2
        //Gors placed on 27, 31
        //Skral placed on 29
        //Prince Thorald placed on 72
    }

    public void activateLegendCard_G()
    {
        Debug.Log("LEGEND CARD G");
        //Remove Prince Thorald
        //Wardraks placed on 26, 27 (10 Strength, 7 Willpower, 2 Black dice)

    }

    public void activateLegendCard_N()
    {
        Debug.Log("LEGEND CARD N");
        //End game (win/lose)
    }
}
