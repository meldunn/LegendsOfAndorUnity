using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items
{
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
