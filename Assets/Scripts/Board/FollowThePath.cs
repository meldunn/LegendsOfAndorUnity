// From Alexander Zotov's Unity 2D Board Game Tutorial: https://www.youtube.com/watch?v=W8ielU8iURI

// DO NOT USE; WILL BE REMOVED

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowThePath : MonoBehaviour
{
    public Transform[] waypoints;

    [SerializeField]
    private float moveSpeed = 1f;

    [HideInInspector]
    public int waypointIndex = 0;

    public bool moveAllowed = false;

    public int playerNum = 0;

    // Start is called before the first frame update
    private void Start()
    {
        transform.position = waypoints[waypointIndex].transform.position;
        // Debug.Log("Player "+playerNum+" has transform position " + transform.position);
    }

    // Update is called once per frame
    private void Update()
    {
        if (moveAllowed)
            Move();
    }

    private void Move()
    {
        // Debug.Log("Move player "+playerNum+ " waypointIndex "+ waypointIndex);
        if (waypointIndex <= waypoints.Length - 1)
        {
            // Debug.Log("Move player " + playerNum + " moving towards "+ waypoints[waypointIndex].transform.position);
            transform.position = Vector2.MoveTowards(transform.position,
            waypoints[waypointIndex].transform.position,
            moveSpeed * Time.deltaTime);

            // Debug.Log("Player "+playerNum+" transform position = " + transform.position + "| target = " + waypoints[waypointIndex].transform.position + " | equal: "+ (transform.position.x == waypoints[waypointIndex].transform.position.x && transform.position.y == waypoints[waypointIndex].transform.position.y));
            if (transform.position.x == waypoints[waypointIndex].transform.position.x && transform.position.y == waypoints[waypointIndex].transform.position.y)
            {
                waypointIndex += 1;
                // Debug.Log("Player "+playerNum+" now has waypoint index " + waypointIndex);
            }
        }
    }
}
