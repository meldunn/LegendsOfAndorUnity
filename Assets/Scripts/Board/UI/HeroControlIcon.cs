using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroControlIcon : MonoBehaviour
{
    // Reference to GameManager
    private GameManager GameManager;

    // The Hero type represented by this icon
    HeroType Type;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize(HeroType Type)
    {
        this.Type = Type;

        // Initialize reference to GameManager
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnMouseUp()
    {
        // On click, change the controlled player
        GameManager.SetSelfPlayer(Type);
    }
}
