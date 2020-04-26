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
    MedicinalHerb
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

    public void setItemType(ItemType type)
    {
        this.type = type;
    }
}
