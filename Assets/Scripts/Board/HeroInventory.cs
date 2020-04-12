using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInventory : MonoBehaviour
{

    // Keeps track of item quantity
    public Dictionary<Item, int> Inventory;

    public HeroInventory()
    {
        Inventory = new Dictionary<Item, int>();
    }

    public void addItem(Item item)
    {
        Inventory[item] += 1;
        Debug.Log("Added Item");
    }

    // Called from Hero in BuyFromMerchant
    public void addItemByType(Type ItemType)
    {
        foreach(KeyValuePair<Item, int> entry in Inventory)
        {
            if(string.Compare(entry.Key.GetType().ToString(), ItemType.ToString())== 0)
            {
                Inventory[entry.Key] += 1;
            }
        }
        Debug.Log(Inventory);
    }

    public void removeItem(Item item)
    {
        if(Inventory[item] > 0)
        {
            Inventory[item] -= 1;
        }
        else
        {
            Debug.Log("You have no "+item.GetType().ToString()+ " to remove.");
        }
    }

    public bool containsItem(Item item)
    {
        if(Inventory[item] > 0) return true;

        return false;
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

    public Dictionary<Item, int> getInventory()
    {
        return Inventory;
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
