using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TagCharacter : NetworkBehaviour {

    private Material material; //The GameObject material
    private Color startColor;
    [SyncVar]
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
        if (!hasAuthority)
            print(isTagged);
    }

    [Command]
    public void CmdTag(Color color)
    {
        RpcChangeState(color);
        isTagged = true;
    }

    [Command]
    public void CmdUnTag()
    {
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
        material.SetColor("_Color", color);;
    }
}
