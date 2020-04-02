using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyStrokes : MonoBehaviour
{
    /*
     * ############# From Unity KeyStrokes API: https://docs.unity3d.com/ScriptReference/Event-keyCode.html
     */
    public GameObject WellPanel;
    // Start is called before the first frame update
    void Start()
    {
        WellPanel = GameObject.Find("WellsPanel");
    }

    // Update is called once per frame
    void Update()
    {
    //    WellPanel = GameObject.Find("WellsPanel");
    //    if(Input.GetKeyDown("w"))
    //    {
    //        Debug.Log("Show the wells");
    //        if(WellPanel != null)
    //        {
    //            Debug.Log("Found well panel");
    //            bool isActive = WellPanel.activeSelf;
    //            WellPanel.SetActive(!isActive);
    //        }
    //    }
    }

}
