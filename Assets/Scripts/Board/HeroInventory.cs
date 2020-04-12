using System.Collections;
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
        Debug.Log("Added Item");
    }

    public void removeItem(Item item)
    {
        itemList.Remove(item);
    }

    public bool containsItem(Item item)
    {
        return itemList.Contains(item);
    }

    public bool containsItem(string item)
    {
        /*
        foreach (Item i in itemList)
        {
            if (i.type.TryParse))
            {
                return true;
            }
        }
        */
        return false;
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
