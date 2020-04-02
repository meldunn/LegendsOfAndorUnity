using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattleIcon : MonoBehaviour
{
    // Reference to UIManager
    private UIManager UIManager;

    // Reference to the start battle menu
    private StartBattleMenu StartBattleMenu;

    // Reference to the creature linked to this icon
    Creature MyCreature;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize reference to UIManager
        UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();

        // Initialize reference to StartBattleMenu
        StartBattleMenu = UIManager.GetStartBattleMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(Creature Creature)
    {
        this.MyCreature = Creature;
    }

    private void OnMouseUp()
    {
        StartBattleMenu.Show(MyCreature, this);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }

    public void Toggle()
    {
        if (this.gameObject.activeSelf == true) Hide();
        else if (this.gameObject.activeSelf == false) Show();
        else Debug.LogError("Error while toggling creature's start battle icon");
    }
}
