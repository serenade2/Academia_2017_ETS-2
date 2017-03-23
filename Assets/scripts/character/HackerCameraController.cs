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
    private SoundManager soundManager;
    public void Start()
    {
        //Debug.Log("Looking for hacker");
        playerTransform = GameObject.FindGameObjectWithTag(targetTag).transform;
        //Debug.Log("HackerPosition is" + playerTransform.position.ToString());
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        soundManager.GameStartPlay();
    }

    void LateUpdate()
    {
        this.transform.position = playerTransform.position + cameraOffset;
    }
}
