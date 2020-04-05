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
        Text willpowerText = GameObject.Find("WillpowerText").GetComponent<UnityEngine.UI.Text>();
        Text strengthText = GameObject.Find("StrengthText").GetComponent<UnityEngine.UI.Text>();
        Text goldText = GameObject.Find("GoldText").GetComponent<UnityEngine.UI.Text>();

        GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        farmerText.text = " Farmers: " + gameManager.GetSelfHero().getNumFarmers();
        willpowerText.text = " Willpower: " + gameManager.GetSelfHero().getWillpower();
        strengthText.text = " Strength: " + gameManager.GetSelfHero().getStrength();
        goldText.text = " Gold: " + gameManager.GetSelfHero().getGold();
    }

    public void Initialize()
    {

    }
}
