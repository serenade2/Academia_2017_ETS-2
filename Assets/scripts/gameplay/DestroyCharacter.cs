using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DestroyCharacter : NetworkBehaviour
{
	// Update is called once per frame
	void Update () {
		
	}

    [ClientRpc]
    public void RpcDestroy()
    {
        Destroy(this.gameObject);
    }

    public void Destroy()
    {
        RewindManager rewindManager = GameObject.Find("RewindManager(Clone)").GetComponent<RewindManager>();
        
        Rewindable self = GetComponent<Rewindable>();
        
        rewindManager.RemoveRewindable(self);

        RpcDestroy();
    }


}
