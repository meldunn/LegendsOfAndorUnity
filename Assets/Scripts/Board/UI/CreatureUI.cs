using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// User interface attached to the creature prefabs
public class CreatureUI : MonoBehaviour
{
    // Battle icon hovering above the creature
    StartBattleIcon StartBattleIcon;

    // The Creature linked to this icon
    Creature MyCreature;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize reference to child components
        StartBattleIcon[] StartBattleIconChildren = GetComponentsInChildren<StartBattleIcon>();
        StartBattleIcon = StartBattleIconChildren[0];                                             // We only expect one
        Creature[] CreatureChildren = GetComponentsInChildren<Creature>();
        MyCreature = CreatureChildren[0];                                                         // We only expect one

        // Initialize the start battle icon as not visible
        StartBattleIcon.Initialize(MyCreature);
        StartBattleIcon.Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseUp()
    {
        // Toggle the start battle icon
        StartBattleIcon.Toggle();
    }
}
