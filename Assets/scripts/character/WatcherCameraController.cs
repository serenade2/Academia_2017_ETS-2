using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WatcherCameraController : MonoBehaviour
{
    //public Vector3 offset = Vector3.zero;

    public float radius = 1f;
    public float moveSpeed = 1;
    public float lookAngleIncrement = 90;
    public enum CameraMode {Fps, Drone};
    public CameraMode cameraMode = CameraMode.Fps;
    private SoundManager soundManager;
    private Rigidbody rb;
    // Use this for initialization
    void Start()
    {

        soundManager = GameObject.FindObjectOfType<SoundManager>();
        soundManager.GameStartPlay();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraMode == CameraMode.Drone)
        {
            transform.Rotate(Vector3.up, Input.GetAxis("RHorizontal1") * lookAngleIncrement * Time.deltaTime, Space.World);
            transform.Rotate(transform.right, -Input.GetAxis("RVertical1") * lookAngleIncrement * Time.deltaTime, Space.World);
            transform.Translate(new Vector3(transform.forward.x, 0, transform.forward.z).normalized * Input.GetAxis("Vertical1") * moveSpeed * Time.deltaTime, Space.World);
            transform.Translate(transform.right * Input.GetAxis("Horizontal1") * moveSpeed * Time.deltaTime, Space.World);
            transform.Translate(Vector3.up * -Input.GetAxis("Zoom") * moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Rotate(Vector3.up, Input.GetAxis("RHorizontal1") * lookAngleIncrement * Time.deltaTime, Space.World);
            transform.Rotate(transform.right, -Input.GetAxis("RVertical1") * lookAngleIncrement * Time.deltaTime, Space.World);
            transform.Translate(transform.forward * Input.GetAxis("Vertical1") * moveSpeed * Time.deltaTime, Space.World);
            transform.Translate(transform.right * Input.GetAxis("Horizontal1") * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
