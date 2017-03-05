using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class CameraManager : NetworkBehaviour
{
    public Camera ServerCamera;
    public Camera ClientCamera;

	// Use this for initialization
	public override void OnStartServer()
    {
        ServerCamera.enabled = true;
	}
    public override void OnStartClient()
    {
        ClientCamera.enabled = true;
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
