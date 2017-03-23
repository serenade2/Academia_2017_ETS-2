using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActivatableObjective : NetworkBehaviour {

    public float fadeTime = 3f;
    private GameObject objective;
    

    public void Start()
    {

    }


    [Command]
    public void CmdActivate()
    {
        RpcActivate();
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
            
            // TODO: Fade in the UI feedback to indicate an objective has been taken

            yield return null; // continue the loop on the next frame
        }

    }

}

