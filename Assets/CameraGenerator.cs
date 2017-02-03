using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGenerator : MonoBehaviour {
    public RoomCameras[] roomCamerasArray;
    public Screens screensScript;

	// Use this for initialization
	void Start () {
        GameObject[] screens = screensScript.GetScreens();
        int i = 0;
        foreach (RoomCameras roomCameras in roomCamerasArray)
        {
            foreach (Camera camera in roomCameras.GetCameras())
            {
                RenderTexture rt = new RenderTexture(1024, 1024, 16, RenderTextureFormat.ARGB32);
                rt.Create();
                camera.enabled = true;
                camera.targetTexture = rt;
                GameObject screen = screens[i];
                screens[i].GetComponent<Renderer>().material.mainTexture = rt;
                i++;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
