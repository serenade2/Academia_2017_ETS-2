﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActivatableDoor : NetworkBehaviour {

    public float fadeTime = 3f;

    private GameObject door;
    private Renderer doorRenderer;
    private Color originalColor;
    private Color fadeTo;


    public void Start()
    {
        door = this.transform.GetChild(0).gameObject;
        doorRenderer = door.GetComponent<Renderer>();
        originalColor = doorRenderer.material.color;
        fadeTo = originalColor;
        fadeTo.a = 0f;
    }

    [ClientRpc]
    public void RpcActivate()
    {
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        for (float t = 0f; t < fadeTime; t += Time.deltaTime)
        {
            float fadeProgress = t / fadeTime;
            doorRenderer.material.color = Color.Lerp(originalColor, fadeTo, fadeProgress);

            yield return null; // continue the loop on the next frame
        }
        

        door.SetActive(false);

    }


    [Command]
    public void CmdActivate()
    {
        RpcActivate();
    }
}
