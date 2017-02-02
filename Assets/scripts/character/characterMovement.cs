using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMovement : MonoBehaviour, IRewindable {
    private bool canMove = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (canMove)
            transform.Translate(Input.GetAxis("Horizontal1") * Time.deltaTime, 0, Input.GetAxis("Vertical1") * Time.deltaTime);
	}

    public void Rewind(bool isRewinding)
    {
        if(isRewinding)
            canMove = false;
        else
            canMove = true;
    }

    public void Pause(bool isPaused)
    {

    }

    public void FastForward(bool fastForward)
    {

    }
}
