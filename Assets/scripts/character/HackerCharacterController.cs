using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// ThirdPerson controller for the Hacker
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class HackerCharacterController : NetworkBehaviour
{
    private bool canMove = true;
    public float speed = 10f;
    private CharacterController ctrl;
    private Rigidbody rb;
	// Use this for initialization
	void Start ()
	{
	    this.ctrl = GetComponent<CharacterController>();
	    //rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (!hasAuthority)
	    {
	        return;
	    }
	    if (canMove)
	    {
            Vector3 input = new Vector3(Input.GetAxis("Horizontal1"), 0f, Input.GetAxis("Vertical1"));
            ctrl.SimpleMove(input * speed * Time.deltaTime);
        }
	}

    public void Rewind(bool isRewinding)
    {
        if (isRewinding)
        {
            canMove = false;
            //rb.isKinematic = true;
        }
        else
        {
            canMove = true;
            //rb.isKinematic = false;
        }
    }

    public void Pause(bool isPaused)
    {

    }

    public void FastForward(bool fastForward)
    {

    }
}
