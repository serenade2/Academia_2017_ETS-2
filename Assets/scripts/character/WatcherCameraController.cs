using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WatcherCameraController : MonoBehaviour
{
    public GameObject level;
    //public Vector3 offset = Vector3.zero;

    public float radius = 1f;
    public float translatSpeed = 1;
    private Vector3 lookAtOffset;

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
    }

    // Update is called once per frame
    void Update()
    {

        //transform.RotateAround(level.transform.position, Vector3.up, -Input.GetAxis("Horizontal1") * 90 * Time.deltaTime);
        //transform.RotateAround(level.transform.position, transform.right, Input.GetAxis("Vertical1") * 90 * Time.deltaTime);
        //transform.Translate((transform.position - level.transform.position) * Input.GetAxis("Zoom") * translatSpeed * Time.deltaTime, Space.World);
        //transform.LookAt(level.transform.position);

        transform.Rotate(Vector3.up, Input.GetAxis("RHorizontal1") * 90 * Time.deltaTime, Space.World);
        transform.Rotate(transform.right, -Input.GetAxis("RVertical1") * 90 * Time.deltaTime, Space.World);
        transform.Translate(transform.forward * Input.GetAxis("Vertical1") * translatSpeed * Time.deltaTime, Space.World);
        transform.Translate(transform.right * Input.GetAxis("Horizontal1") * translatSpeed * Time.deltaTime, Space.World);
    }
}
