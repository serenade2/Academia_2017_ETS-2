using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ThirdPerson controller for the Hacker
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class HackerCharacterController : MonoBehaviour
{
    public float speed = 10f;

    private CharacterController ctrl;

	// Use this for initialization
	void Start ()
	{
	    this.ctrl = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        Vector3 input = new Vector3(Input.GetAxis("Horizontal1"), 0f, Input.GetAxis("Vertical1"));
	    ctrl.SimpleMove(input*speed*Time.deltaTime);
	}
}
