using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WatcherCameraController : MonoBehaviour
{
    public GameObject level;
    //public Vector3 offset = Vector3.zero;

    public float radius = 1f;
    public float moveSpeed = 1;
    public float lookAngleIncrement = 90;
    public enum CameraMode {Rotation, Fps, Drone};
    public CameraMode cameraMode = CameraMode.Fps;
    private SoundManager soundManager;
    // Use this for initialization
    void Start()
    {
        if (level == null)
        {
            GameObject room = GameObject.FindGameObjectWithTag("Room");
            if (room != null)
            {
                this.level = room;
            }

        }

        transform.position = level.transform.position + new Vector3(0, radius, 0);
        transform.LookAt(level.transform.position);
        soundManager = GameObject.FindObjectOfType<SoundManager>();
        soundManager.GameStartPlay();
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraMode == CameraMode.Rotation)
        {
            transform.RotateAround(level.transform.position, Vector3.up, -Input.GetAxis("Horizontal1") * 90 * Time.deltaTime);
            transform.RotateAround(level.transform.position, transform.right, Input.GetAxis("Vertical1") * 90 * Time.deltaTime);
            transform.Translate(transform.forward * Input.GetAxis("Zoom") * moveSpeed * Time.deltaTime, Space.World);
            transform.LookAt(level.transform.position);
        }
        else if(cameraMode == CameraMode.Drone)
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
