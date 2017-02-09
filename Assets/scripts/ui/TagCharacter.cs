using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TagCharacter : NetworkBehaviour {

    private Material material; //The GameObject material
    private Color startColor;
    private bool isTagged = false;

    // Use this for initialization
    void Start()
    {
        material = GetComponent<Renderer>().material;
        startColor = GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Tag(Color color)
    {
        material.SetColor("_Color", color);
        RpcChangeState(color);
        isTagged = true;
    }

    public void UnTag()
    {
        material.SetColor("_Color", startColor);
        RpcChangeState(startColor);
        isTagged = false;
    }

    public bool GetIsTagged()
    {
        return isTagged;
    }

    [ClientRpc]
    public void RpcChangeState(Color color)
    {
        material.SetColor("_Color", color);
        print("mango");
    }
}
