using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkCamera : NetworkBehaviour {
    public bool serverOnly;

	// Use this for initialization
	void Start () {
        if (isServer)
        {

        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
