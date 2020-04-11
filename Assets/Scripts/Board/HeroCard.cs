using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCard : MonoBehaviour
{
    GameObject goldIcon;
    // Start is called before the first frame update
    void Start()
    {
        goldIcon = GameObject.Find("GoldIcon");
        Instantiate(goldIcon, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
