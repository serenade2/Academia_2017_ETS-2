﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour {
    public GameObject reticle;
    public Color color;
    public LayerMask layer;
    private List<TagAI> tags = new List<TagAI>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Input.GetAxis("Horizontal2") * Time.deltaTime * 3, 0, Input.GetAxis("Vertical2") * Time.deltaTime * 3, Space.World);
        RaycastHit hit;
        if (Physics.Raycast(transform.position - transform.TransformDirection(Vector3.up*0.1f), transform.TransformDirection(Vector3.back), out hit, 50f, layer))
        {

            if (Input.GetKeyDown(KeyCode.Joystick2Button0))
            {
                if (hit.collider.GetComponent<TagAI>().GetIsTagged())
                {
                    tags.Remove(hit.collider.GetComponent<TagAI>());
                    hit.collider.GetComponent<TagAI>().UnTag();
                }
                else
                {
                    if (tags.Count <= 2)
                    {
                        tags.Add(hit.collider.GetComponent<TagAI>());
                        hit.collider.GetComponent<TagAI>().Tag(color);
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Joystick2Button2))
            {
                Destroy(hit.collider.gameObject.transform.parent.gameObject);
            }

        }
	}
}
