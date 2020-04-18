using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Well : MonoBehaviourPun
{
    private bool Full = true;

    public bool IsFull()
    {
        return Full;
    }

    // Replicates for all clients
    public void ReplenishWell()
    {

    }

    // Replicates for all clients
    public void EmptyWell()
    {

    }
}
