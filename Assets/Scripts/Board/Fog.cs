using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    private FogType FogType;
    public int waypointnum; //number on the board
    public  GameObject FogFrontCard;
    public FogBack FogBackCard;
    


    public Fog(int waypointnum, FogBack FogBackCard)
    {
        this.waypointnum = waypointnum;
        this.FogBackCard = FogBackCard;
    }

    public void Visibility(GameObject card, bool visible)
    {
        if (visible == true)
        {
            card.SetActive(true);
        }
        else
        {
            card.SetActive(false);
        }
    }

    //public void SetWPNum(int WPNum)
    //{
    //    this.waypointnum = WPNum;
    //}

    public int GetWPNum()
    {
        return waypointnum;
    }

    public void SetFrontCard(GameObject FogFrontCard)
    {
        this.FogFrontCard = FogFrontCard;
    }

    public FogBack GetFogBackCard()
    {
        return this.FogBackCard;
    }

    public FogType GetFogType()
    {
        return this.FogType;
    }
    //public void SetBackCard(GameObject FogBackCard)
    //{
    //    this.FogBackCard = FogBackCard;
    //}

    //public void SetBackCard(FogBack FogBackCard)
    //{
    //    this.FogBackCard = FogBackCard;
    //}
}
