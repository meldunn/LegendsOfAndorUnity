using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TestNetwork : MonoBehaviourPun
{
    [SerializeField]
    GameObject Input;

    [SerializeField]
    GameObject GoButton;

    [SerializeField]
    GameObject StoreButton;

    [SerializeField]
    GameObject RetrieveButton;

    [SerializeField]
    GameObject Display;

    // Sub object
    TestSubNetwork StringHolder;

    // Start is called before the first frame update
    void Start()
    {
        StringHolder = new TestSubNetwork();      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private string GetInput()
    {
        return Input.GetComponent<InputField>().text;
    }

    [PunRPC]
    private void TestDisplayText(string Text)
    {
        Display.GetComponent<Text>().text = Text;
    }

    [PunRPC]
    private void TestStoreText(string Text)
    {
        StringHolder.SetPhrase(Text);
    }

    private string RetrieveText()
    {
        return StringHolder.GetPhrase();
    }

    // User-triggered action
    public void Go()
    {
        string Input = GetInput();

        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("TestDisplayText", RpcTarget.All, Input);
        }
        else
        {
            TestDisplayText(Input);
        }
    }

    // User-triggered action
    public void Store()
    {
        string Input = GetInput();

        if (PhotonNetwork.IsConnected)
        {
            photonView.RPC("TestStoreText", RpcTarget.All, Input);
        }
        else
        {
            TestStoreText(Input);
        }
    }

    // User-triggered action
    public void Retrieve()
    {
        // if (photonView.IsMine && PhotonNetwork.IsConnected)
        if (PhotonNetwork.IsConnected)
        {
            string Text = RetrieveText();
            photonView.RPC("TestDisplayText", RpcTarget.All, Text);
        }
        else
        {
            TestDisplayText(RetrieveText());
        }
    }
}
