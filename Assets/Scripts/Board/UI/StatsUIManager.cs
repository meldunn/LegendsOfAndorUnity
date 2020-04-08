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

    String oldFarmerText;
    String oldWillpowerText;
    String oldStrengthText;
    String oldGoldText;

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
        if (!string.Equals(oldFarmerText, farmerText.text))
        {
            farmerText.text = " Farmers: " + gameManager.GetSelfHero().getNumFarmers();
            oldFarmerText = farmerText.text;
        }
        if (!string.Equals(oldWillpowerText, willpowerText.text))
        {
            willpowerText.text = " Willpower: " + gameManager.GetSelfHero().getWillpower();
            oldWillpowerText = willpowerText.text;
        }
        if (!string.Equals(oldStrengthText, strengthText.text))
        {
            strengthText.text = " Strength: " + gameManager.GetSelfHero().getStrength();
            oldStrengthText = strengthText.text;
        }
        if (!string.Equals(oldGoldText, goldText.text))
        {
            goldText.text = " Gold: " + gameManager.GetSelfHero().getGold();
            oldGoldText = goldText.text;
        }
    }

    public void Initialize()
    {

    }
}
