using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RuneStoneDice : MonoBehaviour
{
    private Sprite[] diceSides;
    private SpriteRenderer rend;
    private bool coroutineAllowed = true;

    private int currentDiceRoll;

    private RuneStoneMenu RuneStoneMenu;

    // Use this for initialization
    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("Board/Dice/");
        rend.sprite = diceSides[5];
    }

    private void OnMouseDown()
    {
        if(RuneStoneMenu == null)
        {
            RuneStoneMenu = GameObject.Find("RuneStoneMenu").GetComponent<RuneStoneMenu>();
        }

        if (!GameControl.gameOver && coroutineAllowed)
            StartCoroutine("RollTheDice");
    }

    private IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        int randomDiceSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = UnityEngine.Random.Range(1, 7);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }

        // check which dice was rolled
        bool OnesPosition = String.Equals(gameObject.name, "RuneOnesDice");
        Debug.Log("Change Rune Ones Die to " + randomDiceSide);
        if(RuneStoneMenu != null)
        {
            RuneStoneMenu.FinishedRoll(OnesPosition, randomDiceSide);
        }

        coroutineAllowed = true;
    }
}
