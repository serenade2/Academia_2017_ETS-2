﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TagCharacter : NetworkBehaviour {
    public GameObject model;
    private Material material; //The GameObject material
    private Color startColor;
    private bool isTagged = false;

    // Use this for initialization
    void Start()
    {

        material = model.GetComponent<Renderer>().material;
        startColor = model.GetComponent<Renderer>().material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Tag(Color color)
    {
        Debug.Log("tag");
        material.SetColor("_Color", color);

        RpcChangeState(color);
        Debug.Log("tag");
        isTagged = true;
        Debug.Log("tagFin");
    }

    public void UnTag()
    {
        Debug.Log("untag");
        material.SetColor("_Color", startColor);
        RpcChangeState(startColor);
        isTagged = false;
        Debug.Log("untagFin");
    }

    public bool GetIsTagged()
    {
        return isTagged;
    }

    [ClientRpc]
    public void RpcChangeState(Color color)
    {
        Debug.Log(color);
        material.SetColor("_Color", color);
    }
}
