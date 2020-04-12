using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Placed outside of Item in order to access enum Type in other classes
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
    public Type type;

    public Type GetType(Item i)
    {
        return i.type;
    }
}
