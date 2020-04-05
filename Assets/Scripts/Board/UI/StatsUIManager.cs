using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatsUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Text farmerText = GameObject.Find("FarmersText").GetComponent<UnityEngine.UI.Text>();
        Text willPower = GameObject.Find("FarmersText").GetComponent<UnityEngine.UI.Text>();
        Text farmerText = GameObject.Find("FarmersText").GetComponent<UnityEngine.UI.Text>();
        Text farmerText = GameObject.Find("FarmersText").GetComponent<UnityEngine.UI.Text>();
        //UIText.text = gameObject.GetComponent<Text>().text;
        //UIText = GameObject.FindGameObjectWithTag("Text").GetComponent<UnityEngine.UI.Text>();
        farmerText.text = "Farmer: ";
        //farmerText = "Farmers: ";
    }

    public void Initialize()
    {

    }
}
