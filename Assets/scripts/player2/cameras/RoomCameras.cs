using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomCameras : MonoBehaviour {
    private Camera[] cameras;

	// Use this for initialization
	void Awake () {
        cameras = GetComponentsInChildren<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public Camera[] GetCameras()
    {
        return cameras;
    }
}
