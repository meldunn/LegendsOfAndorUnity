using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : MonoBehaviour
{
    private bool full = false; 

    public Well(){
        full = true;
    }

    public bool isEmpty(){
        return !full;
    }
    public void replenishWell(){
        full = true;
    }
    public void emptyWell(){
        if(isEmpty())
        {
            Debug.Log("Error. You tried to empty an empty well");
        }
        else
        {
            full = false;
        }
    }
}
