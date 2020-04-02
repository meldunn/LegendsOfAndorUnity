using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public int amount;

    public enum ItemType
    {
        Wineskin,
        Telescope,
        Witchbrew,
        Helm,
        Shield,
        Bow,
        Falcon

    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
