// From Alexander Zotov's Unity 2D Board Game Tutorial: https://www.youtube.com/watch?v=W8ielU8iURI

// DO NOT USE; WILL BE REMOVED -- USE GameManager INSTEAD

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameControl : MonoBehaviour
{
    private static GameObject whoWinsText, player1MoveText, player2MoveText;

    private static GameObject player1, player2;

    public static int diceSideThrown = 0;
    public static int player1StartWaypoint = 0;
    public static int player2StartWaypoint = 0;

    public static bool gameOver = false;

    public static NarratorLetter curLetter = NarratorLetter.A;

    // Use this for initialization
    void Start()
    {

        whoWinsText = GameObject.Find("WhoWinsText");
        player1MoveText = GameObject.Find("Player1MoveText");
        player2MoveText = GameObject.Find("Player2MoveText");

        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");

        player1.GetComponent<FollowThePath>().moveAllowed = false;
        player2.GetComponent<FollowThePath>().moveAllowed = false;

        whoWinsText.gameObject.SetActive(false);
        player1MoveText.gameObject.SetActive(true);
        player2MoveText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Debug.Log("Player 1 WP index: " + player1.GetComponent<FollowThePath>().waypointIndex);
        Debug.Log("Player 1 Start WP index: " + player1StartWaypoint);
        Debug.Log("Player 2 WP index: " + player2.GetComponent<FollowThePath>().waypointIndex);
        Debug.Log("Player 2 Start WP index: " + player2StartWaypoint);
        Debug.Log("diceSideThrown: "+ diceSideThrown);
        Debug.Log(player1.GetComponent<FollowThePath>().waypointIndex > player1StartWaypoint + diceSideThrown);
        Debug.Log(player2.GetComponent<FollowThePath>().waypointIndex > player2StartWaypoint + diceSideThrown);
        */

        if (player1.GetComponent<FollowThePath>().waypointIndex >
            player1StartWaypoint + diceSideThrown)
        {
            player1.GetComponent<FollowThePath>().moveAllowed = false;
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(true);
            player1StartWaypoint = player1.GetComponent<FollowThePath>().waypointIndex - 1;
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex >
            player2StartWaypoint + diceSideThrown)
        {
            player2.GetComponent<FollowThePath>().moveAllowed = false;
            player2MoveText.gameObject.SetActive(false);
            player1MoveText.gameObject.SetActive(true);
            player2StartWaypoint = player2.GetComponent<FollowThePath>().waypointIndex - 1;
        }

        if (player1.GetComponent<FollowThePath>().waypointIndex ==
            player1.GetComponent<FollowThePath>().waypoints.Length)
        {
            whoWinsText.gameObject.SetActive(true);

            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(false);
            // whoWinsText.GetComponent<Text>().text = "Player 1 Wins";
            gameOver = true;
        }

        if (player2.GetComponent<FollowThePath>().waypointIndex ==
            player2.GetComponent<FollowThePath>().waypoints.Length)
        {
            whoWinsText.gameObject.SetActive(true);
            player1MoveText.gameObject.SetActive(false);
            player2MoveText.gameObject.SetActive(false);
            // whoWinsText.GetComponent<Text>().text = "Player 2 Wins";
            gameOver = true;
        }

        if (curLetter == NarratorLetter.A)
        {
            //Move to location
        } 
        else if (curLetter == NarratorLetter.B)
        {

        } 
        else if (curLetter == NarratorLetter.C)
        {

        }
        else if (curLetter == NarratorLetter.D)
        {

        }
        else if (curLetter == NarratorLetter.E)
        {

        }
        else if (curLetter == NarratorLetter.F)
        {

        }
        else if (curLetter == NarratorLetter.G)
        {

        }
        else if (curLetter == NarratorLetter.H)
        {

        }
        else if (curLetter == NarratorLetter.I)
        {

        }
        else if (curLetter == NarratorLetter.J)
        {

        }
        else if (curLetter == NarratorLetter.K)
        {

        }
        else if (curLetter == NarratorLetter.L)
        {

        }
        else if (curLetter == NarratorLetter.M)
        {

        }
        else if (curLetter == NarratorLetter.N)
        {

        }
        else if (curLetter == NarratorLetter.Z)
        {
            gameOver = true;
        }

    }

    public static void MovePlayer(int playerToMove)
    {
        switch (playerToMove)
        {
            case 1:
                player1.GetComponent<FollowThePath>().moveAllowed = true;
                break;

            case 2:
                player2.GetComponent<FollowThePath>().moveAllowed = true;
                break;
        }
    }

    public static void moveNarrator()
    {
        curLetter = (NarratorLetter)(curLetter + 1);
    }
}
