﻿// From Alexander Zotov's Unity 2D Board Game Tutorial: https://www.youtube.com/watch?v=W8ielU8iURI

// DO NOT USE THIS FILE DIRECTLY; IT WILL BE REMOVED

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private Sprite[] diceSides;
    private SpriteRenderer rend;
    private int whosTurn = 1;
    private bool coroutineAllowed = true;

    private int currentDiceRoll;

    private GameManager GameManager;

    // Use this for initialization
    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("Board/Dice/");
        rend.sprite = diceSides[5];
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    private void OnMouseDown()
    {
        if (!GameControl.gameOver && coroutineAllowed)
            StartCoroutine("RollTheDice");
    }

    private IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        int randomDiceSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(1, 7);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }

        GameControl.diceSideThrown = randomDiceSide;

        currentDiceRoll = randomDiceSide;

        Debug.Log("Set dice side to "+ GameControl.diceSideThrown);

        //if (whosTurn == 1)
        //{
        //    GameControl.MovePlayer(1);
        //}
        //else if (whosTurn == -1)
        //{
        //    GameControl.MovePlayer(2);
        //}
        //whosTurn *= -1;
        
        coroutineAllowed = true;
    }

}
