using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSubNetwork
{
    string MyPhrase = "Default";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPhrase(string Text)
    {
        MyPhrase = Text;
    }

    public string GetPhrase()
    {
        return MyPhrase;
    }
}
