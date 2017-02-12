using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DestroyCharacter : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    [Command]
    public void CmdDestroy()
    {
        RpcDestroy();
    }

    [ClientRpc]
    public void RpcDestroy()
    {
        Destroy(gameObject);
    }
}
