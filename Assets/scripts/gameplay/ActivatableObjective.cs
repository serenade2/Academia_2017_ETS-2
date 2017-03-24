using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActivatableObjective : NetworkBehaviour {

    public float fadeTime = 3f;
    private GameObject objective;
    

    [ClientRpc]
    public void RpcActivate()
    {
        this.gameObject.SetActive(false);
    }
    
}

