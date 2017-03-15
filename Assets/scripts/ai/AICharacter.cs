using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class AICharacter : NetworkBehaviour
{
    public string HackerTag;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider col)
    {
        if (!col.gameObject.name.Equals("HackingCollisionRadius"))
        {
            return;
        }

        TakeOver hacker = col.GetComponentInParent<TakeOver>();
        hacker.AddAi(this.gameObject);
    }

    public void OnTriggerExit(Collider col)
    { 
        if (!col.gameObject.name.Equals("HackingCollisionRadius"))
        {
            return;
        }

        TakeOver hacker = col.GetComponentInParent<TakeOver>();

        hacker.RemoveAi(this.gameObject);
    }
}
