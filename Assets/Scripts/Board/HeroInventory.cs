﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInventory : MonoBehaviour
{

    public List<Item> itemList;

    public HeroInventory()
    {
        itemList = new List<Item>();
    }

    public void addItem(Item item)
    {
        itemList.Add(item);
    }

    public void containsItem(Item item)
    {
        itemList.Contains(item);
    }

    public List<Item> getInventory()
    {
        return itemList;
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
