using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindMovement : MonoBehaviour {
    public float walkingSpeed = 2f;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void LateUpdate () {
	   // rb.velocity = new Vector3(Input.GetAxis("Horizontal") * walkingSpeed,rb.velocity.y,Input.GetAxis("Vertical") * walkingSpeed);
	}
}
