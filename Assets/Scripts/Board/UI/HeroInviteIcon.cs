using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Icon that can be clicked to invite another hero to a battle
public class HeroInviteIcon : MonoBehaviour
{
    // The parent start battle menu of this icon
    [SerializeField]
    private StartBattleMenu StartBattleMenu = null;

    // The frame around the icon that indicates that the icon was selected
    [SerializeField]
    private GameObject SelectionFrame = null;

    // The type of hero represented by this icon
    [SerializeField]
    private HeroType Type = HeroType.Warrior;       // Default to prevent compiler warnings; this value is overwritten

    // Wether this icon has been selected
    private bool Selected = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        Selected = false;
        SelectionFrame.SetActive(false);
    }

    private void OnMouseUp()
    {
        // On click, toggle this icon's selection
        Selected = !Selected;

        // Add or remove the target hero to the list of heroes to invite to the battle
        if (Selected)
        {
            SelectionFrame.SetActive(true);
            StartBattleMenu.AddInvite(Type);
        }
        else
        {
            SelectionFrame.SetActive(false);
            StartBattleMenu.RemoveInvite(Type);
        }
    }
}
