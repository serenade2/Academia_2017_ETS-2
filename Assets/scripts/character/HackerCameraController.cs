using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Behavior of the Hacker's camera
/// </summary>
public class HackerCameraController : MonoBehaviour
{
    public string targetTag;    // tag of the object the camera will follow
    public Vector3 cameraOffset;

    private Transform playerTransform;
    
	void Start() {

	    playerTransform = GameObject.FindGameObjectWithTag(targetTag).transform;
	}
    
    void LateUpdate()
    {
        this.transform.position = playerTransform.position + cameraOffset;
        
    }
}
