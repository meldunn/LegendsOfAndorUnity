using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndorPlayer : MonoBehaviour
{
    // Hero controlled by this player
    Hero MyHero = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Sets the hero controlled by this player. Can only be called once per player.
    public void SetHero(Hero Hero)
    {
        if (MyHero != null)
        {
            Debug.LogError("Error: this Players's Hero has already been set.");
            return;
        }

        MyHero = Hero;
    }

    public Hero GetHero()
    {
        return this.MyHero;
    }
}
