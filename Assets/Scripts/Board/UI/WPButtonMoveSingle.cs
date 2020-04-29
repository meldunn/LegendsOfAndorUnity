using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class WPButtonMoveSingle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseUp()
    {
        WPButtonMoveUI Manager = GameObject.Find("WPButtonMoveUI").GetComponent<WPButtonMoveUI>();
        Manager.ClickToMove(nameToPosInt(this.gameObject.name));
    }


    public int nameToPosInt(string name)
    {
        //convert name to int position

        string newStr;
        newStr = string.Join(string.Empty, Regex.Matches(name, @"\d+").OfType<Match>().Select(m => m.Value)); //extract numbers
        int num = int.Parse(newStr);
        return num;
    }
}
