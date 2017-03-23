using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DestroyCharacter : NetworkBehaviour
{
    public GameObject VfxGameObject;
    public float VfxLifeTime = 5;
    private ActorSoundManager actorSoundManager;
    void Start()
    {
        actorSoundManager = GetComponent<ActorSoundManager>();
            // hide the 
        VfxGameObject.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
		
	}

    [ClientRpc]
    public void RpcDestroy()
    {
        DisplayDestroyVfx();
        Destroy(this.gameObject);
    }

    public void Destroy()
    {
        RewindManager rewindManager = GameObject.Find("RewindManager(Clone)").GetComponent<RewindManager>();
        
        Rewindable self = GetComponent<Rewindable>();
        
        rewindManager.RemoveRewindable(self);

        RpcDestroy();
    }

    public void DisplayDestroyVfx()
    {

        actorSoundManager.PlayDesintegrateSound();
        VfxGameObject.SetActive(true);
        
        VfxGameObject.transform.parent = null;
        Destroy(VfxGameObject,VfxLifeTime);
    }
}
