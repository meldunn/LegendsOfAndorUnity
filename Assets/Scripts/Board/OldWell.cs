using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldWell
{
    private bool Full = false; 

    public OldWell() {
        Full = true;
    }

    public bool IsFull(){
        return Full;
    }
    public void ReplenishWell(){
        Full = true;
    }
    public void EmptyWell(){
        if(!IsFull())
        {
            Debug.Log("Error. You tried to empty an empty well");
        }
        else
        {
            Full = false;
        }
    }
}
