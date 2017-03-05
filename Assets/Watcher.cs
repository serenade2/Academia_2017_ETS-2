﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Watcher : NetworkBehaviour {
	public GameObject rewindManagerPrefab;
	public GameObject reticlePrefab;
	private GameObject rewindManager;
	private GameObject reticle;

	// Use this for initialization
	public override void OnStartServer () {
		base.OnStartServer ();
		rewindManager = Instantiate (rewindManagerPrefab);
		reticle = Instantiate (reticlePrefab, GameObject.Find ("player2Spawn").transform.position, reticlePrefab.transform.rotation);
		NetworkServer.Spawn (rewindManager);
		NetworkServer.Spawn (reticle);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
