using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraNavigation : MonoBehaviour
{
    public GameObject level;
    public Vector3 offset = Vector3.zero;

    public float radius = 2.5f;
    public float translatSpeed = 1;
    public float rotateAngle = 0;

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
    }

    // Update is called once per frame
    void Update()
    {

        //Translate with horizontal input
        transform.Translate(Input.GetAxis("Horizontal1") * translatSpeed * Time.deltaTime, 0, 0);

        //Rotation
        rotateAngle = Mathf.Clamp(rotateAngle + Input.GetAxis("Vertical1") * 90 * Time.deltaTime, 0, 90); //Increments the angle
        transform.rotation = Quaternion.AngleAxis(rotateAngle, Vector3.right); //Creates a quaternion from the angle and updades the camera rotation to it

        /*Position the camera by using the sin and the cos of the camera angle(trigonometric circle with radius of 1), multiply it by the wanted radius 
         * and add it the level position*/
        float transformY = Mathf.Sin(rotateAngle * Mathf.Deg2Rad) * radius + level.transform.position.y + offset.y;
        float transformZ = Mathf.Cos(rotateAngle * Mathf.Deg2Rad) * -radius + level.transform.position.z + offset.z;
        transform.position = new Vector3(transform.position.x, transformY, transformZ);
    }
}
