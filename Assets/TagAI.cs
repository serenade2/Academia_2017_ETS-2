using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagAI : MonoBehaviour {
    public GameObject character;
    public TrailRenderer trail;
    private Material material;
    private Color startColor;
    private bool isTagged = false;
    

	// Use this for initialization
	void Start () {
		material = character.GetComponent<Renderer>().material;
        startColor = character.GetComponent<Renderer>().material.color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Tag(Color color)
    {
        material.SetColor("_Color", color);
        isTagged = true;
        trail.enabled = true;
    }

    public void UnTag()
    {
        material.SetColor("_Color", startColor);
        isTagged = false;
        trail.enabled = false;
    }

    public bool GetIsTagged()
    {
        return isTagged;
    }
}
