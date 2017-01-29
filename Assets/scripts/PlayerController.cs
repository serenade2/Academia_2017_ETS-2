using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float HorizontalSensitivity;
    public float VerticalSensitivity;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * HorizontalSensitivity;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * VerticalSensitivity;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }
}
