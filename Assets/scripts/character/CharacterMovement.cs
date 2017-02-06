using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterMovement : NetworkBehaviour, IRewindable
{
    private bool canMove = true;
    private Rigidbody rb;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority)
        {
            return;
        }
        if (canMove)
        {
            rb.velocity = new Vector3(Input.GetAxis("Horizontal1"), rb.velocity.y, Input.GetAxis("Vertical1"));
        }
    }

    public void Rewind(bool isRewinding)
    {
        if (isRewinding)
        {
            canMove = false;
            rb.isKinematic = true;
        }
        else
        {
            canMove = true;
            rb.isKinematic = false;
        }
    }

    public void Pause(bool isPaused)
    {

    }

    public void FastForward(bool fastForward)
    {

    }
}
