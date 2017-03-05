﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CharacterMovement : NetworkBehaviour, IRewindable
{
    public GameObject hackerCameraPrefab;
    public float speed = 10f;

    private bool canMove = true;
    private CharacterController ctrl;


    // Use this for initialization
    void Start()
    {
        this.ctrl = GetComponent<CharacterController>();

        if (hasAuthority)
        {
            GameObject.Instantiate(hackerCameraPrefab);
        }

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
            Vector3 input = new Vector3(Input.GetAxis("Horizontal1"), 0f, Input.GetAxis("Vertical1"));
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
