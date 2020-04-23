using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour//, Observer
{
    // Reference to GameManager
    private GameManager GameManager;

    // References to children components
    GameObject WarriorHeroCard;
    GameObject ArcherHeroCard;
    GameObject DwarfHeroCard;
    GameObject WizardHeroCard;
    


    public void Initialize()
    {
        // Initialize reference to GameManager
       // GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Register as an observer of GameManager
        //GameManager.Attach(this);

        // Initialize references to children components
        //

        /// <summary>
        /// Once the Hero icon is pressed, the HeroCard is activated. 
        ///     - inventory manager checks and activates every item in the Hero's inventory 
        ///     
        ///     we need access to the hero class 
        /// </summary>

    }
        // Start is called before the first frame update
        void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
