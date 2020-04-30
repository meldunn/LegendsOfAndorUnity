using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog 
{
    private FogType FogType;
    public int waypointnum; //number on the board
    public  FogFront FogFrontCard;
    public FogBack FogBackCard;
    public bool IsFogUnused;
    public bool IsFogUncovered;
    


    public Fog(int waypointnum, FogBack FogBackCard, FogFront FogFrontCard, FogType FogType)
    {
        this.waypointnum = waypointnum;
        this.IsFogUnused = true;
        this.IsFogUncovered = false;
        this.FogBackCard = FogBackCard;
        this.FogFrontCard = FogFrontCard;
        this.FogType = FogType;
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



    public int GetWPNum()
    {
        return waypointnum;
    }

    //public void SetFrontCard(GameObject FogFrontCard)
    //{
    //    this.FogFrontCard = FogFrontCard;
    //}

    public FogBack GetFogBackCard()
    {
        return this.FogBackCard;
    }
    public FogFront GetFogFrontCard()
    {
        return this.FogFrontCard;
    }

    public FogType GetFogType()
    {
        return this.FogType;
    }

    public void UncoverBack()
    {
        this.FogBackCard.gameObject.SetActive(false);
        this.IsFogUncovered = true;
        //show front card
        this.FogFrontCard.gameObject.SetActive(true);
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
