using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldIconManager : MonoBehaviour
{
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(icon, waypoints[0].GetLocation(), Quaternion.identity);



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        WaypointManager manager = GameObject.Find("WaypointManager").GetComponent<WaypointManager>();
        Waypoint[] waypoints = new Waypoint[85];
        waypoints = manager.GetWaypoints();
        prefab = GameObject.Find("GoldIcon");
        Debug.Log(waypoints[0].GetLocation());
        for (int i = 0; i <= 84; i++) { 
            Debug.Log(waypoints[i].GetLocation());
            Instantiate(prefab, waypoints[i].GetLocation(), Quaternion.identity);
        }
    }
}
