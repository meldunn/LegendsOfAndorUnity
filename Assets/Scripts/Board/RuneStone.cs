using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneStone
{
    int ID;
    string Color;
    public RuneStone(int ID_)
    {
        Debug.Log("Rune Stone" + ID+ " created.");
        ID = ID_;

        if(ID == 1 || ID == 2) Color = "Yellow";
        else if(ID == 3 || ID == 4) Color = "Green";
        else if(ID == 5) Color = "Blue";
        else Debug.Log("Error instantiating Rune Stone. ID " + ID_ +" invalid.");

        // Debug.Log("Color: " + Color);
    }

}
