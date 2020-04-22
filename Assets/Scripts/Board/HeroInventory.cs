using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroInventory : MonoBehaviour
{

    // Keeps track of item quantity
    private Dictionary<ItemType, int> Inventory = new Dictionary<ItemType, int>();

    public HeroInventory()
    {
        Inventory = new Dictionary<ItemType, int>();
        
        Inventory[ItemType.Helm] = 0;
        Inventory[ItemType.Wineskin] = 0;
        Inventory[ItemType.Bow] = 0;
        Inventory[ItemType.Telescope] = 0;
        Inventory[ItemType.Falcon] = 0;
        Inventory[ItemType.MedicinalHerb] = 0;
        Inventory[ItemType.Witchbrew] = 0;
        Inventory[ItemType.Shield] = 0;
    }

    public void addItem(ItemType item)
    {
        Inventory[item] += 1;
        Debug.Log("Added Item"+ item);
    }

    public void removeItem(ItemType item)
    {
        if(Inventory[item] > 0)
        {
            Inventory[item] -= 1;
        }
        else
        {
            Debug.Log("You have no "+item.ToString()+ " to remove.");
        }
    }

    public bool containsItem(ItemType item)
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

    public Dictionary<ItemType, int> getInventory()
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
