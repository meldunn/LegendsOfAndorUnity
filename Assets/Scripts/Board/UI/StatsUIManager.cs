using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StatsUIManager : MonoBehaviour
{
    Text farmerText;
    Text willpowerText;
    Text strengthText;
    Text goldText;

    //Text oldFarmerText = new Text();
    //Text oldWillpowerText = new Text();
    //Text oldStrengthText = new Text();
    //Text oldGoldText = new Text();

    GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        farmerText = GameObject.Find("FarmersText").GetComponent<UnityEngine.UI.Text>();
        willpowerText = GameObject.Find("WillpowerText").GetComponent<UnityEngine.UI.Text>();
        strengthText = GameObject.Find("StrengthText").GetComponent<UnityEngine.UI.Text>();
        goldText = GameObject.Find("GoldText").GetComponent<UnityEngine.UI.Text>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!string.Equals(oldFarmerText.text, farmerText.text))
      
        farmerText.text = " Farmers: " + gameManager.GetSelfHero().getNumFarmers();
        willpowerText.text = " Willpower: " + gameManager.GetSelfHero().getWillpower();
        strengthText.text = " Strength: " + gameManager.GetSelfHero().getStrength();
        goldText.text = " Gold: " + gameManager.GetSelfHero().getGold();
    }

    public void Initialize()
    {

    }
}
