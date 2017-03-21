using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// ThirdPerson controller for the Hacker
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class HackerCharacterController : NetworkBehaviour, IRewindable
{
    public GameObject hackerCameraPrefab;
    public float speed = 10f;
    private bool canMove = true;
    private CharacterController ctrl;
    private NetworkAnimator hackerNetworkAnimator;

    // Use this for initialization
    void Start()
    {
        this.ctrl = GetComponent<CharacterController>();
        hackerNetworkAnimator = GetComponentInChildren<NetworkAnimator>();

        if (hasAuthority)
        {
            GameObject.Instantiate(hackerCameraPrefab);
        }

    }

    // Update is called once per frame
    void Update()
    {
        // check if a new animator has been assigned.
        if (hackerNetworkAnimator.transform.gameObject.activeSelf == false)
        {
            hackerNetworkAnimator = GetComponentInChildren<NetworkAnimator>();
        }

        if (!hasAuthority)
        {
            return;
        }
        
	    if (canMove)
	    {
            Vector3 input = new Vector3(Input.GetAxis("Horizontal1"), 0f, Input.GetAxis("Vertical1"));
            hackerNetworkAnimator.animator.SetFloat("Speed", input.sqrMagnitude);
            //align the model to the same direction the character is moving.
            //hackerNetworkAnimator.transform.forward = this.transform.forward; 
            ctrl.SimpleMove(input * speed);
        }
    }

    public void Rewind(bool isRewinding)
    {
        if (isRewinding)
        {
            canMove = false;
        }
        else
        {
            canMove = true;
        }
    }

    public void Pause(bool isPaused)
    {

    }

    public void FastForward(bool fastForward)
    {

    }
}
