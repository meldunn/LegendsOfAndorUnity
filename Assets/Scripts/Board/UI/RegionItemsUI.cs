using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionItemsUI : MonoBehaviour
{

    [SerializeField] GameObject Bow;
    [SerializeField] GameObject Brew;
    [SerializeField] GameObject BlueRune;
    [SerializeField] GameObject Falcon;
    [SerializeField] GameObject GreenRune;
    [SerializeField] GameObject Helm;
    [SerializeField] GameObject Shield;
    [SerializeField] GameObject Telescope;
    [SerializeField] GameObject Wineskin;
    [SerializeField] GameObject YellowRune;
    [SerializeField] GameObject Medicinalherb;

    //reference to waypoint items
    //if waypoint.Items.Itemtype value > 0, then show GameObject ^ 

    public void Initialize()
    {
        Bow.SetActive(false);
        Brew.SetActive(false);
    
    }


    public static void showItems(Dictionary<ItemType, int> Items)
    {
        if (Items[ItemType.Bow] > 0) Bow.SetActive(true);
        if (Items[ItemType.Witchbrew] > 0) Brew.SetActive(true);
        if (Items[ItemType.BlueRuneStone] > 0) BlueRune.SetActive(true);
        if (Items[ItemType.Falcon] > 0) Falcon.SetActive(true);
        if (Items[ItemType.GreenRuneStone] > 0) GreenRune.SetActive(true);
        if (Items[ItemType.Helm] > 0) Helm.SetActive(true);
        if (Items[ItemType.Shield] > 0) Shield.SetActive(true);
        if (Items[ItemType.Telescope] > 0) Telescope.SetActive(true);
        if (Items[ItemType.Wineskin] > 0) Wineskin.SetActive(true);
        if (Items[ItemType.Wineskin] > 0) Wineskin.SetActive(true);
        if (Items[ItemType.YellowRuneStone] > 0) YellowRune.SetActive(true);
        if (Items[ItemType.MedicinalHerb] > 0) Medicinalherb.SetActive(true);

    }



}
