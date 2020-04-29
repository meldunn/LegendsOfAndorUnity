using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Placed outside of Item in order to access enum Type in other classes
public enum ItemType
{
    Wineskin,
    Telescope,
    Witchbrew,
    Helm,
    Shield,
    Bow,
    Falcon,
    MedicinalHerb,
    StrengthPoints
}
public class Item: MonoBehaviour
{
    /*
    public enum Type
    {
        Wineskin,
        Telescope,
        Witchbrew,
        Helm,
        Shield,
        Bow,
        Falcon
    }
    */
    public ItemType type;

    public ItemType GetItemType()
    {
        return type;
    }
}
