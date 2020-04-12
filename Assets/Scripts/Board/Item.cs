using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item: MonoBehaviour
{
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
    public Type type;

    public Type GetType(Item i)
    {
        return i.type;
    }
}
