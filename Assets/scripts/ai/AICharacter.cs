using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class AICharacter : NetworkBehaviour
{
    public string HackerTag;
    public GameObject Cursor;
    private HackerCursor hackerCursor;

    void Start()
    { 
        if (Cursor != null)
        {
            hackerCursor = Cursor.GetComponent<HackerCursor>();
            hackerCursor.Target = this.gameObject;
        }
    }

 //   // Use this for initialization
 //   void Start () {
		
	//}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider col)
    {
        if (!col.gameObject.CompareTag(HackerTag))
        {
            return;
        }

        Hacker hacker = col.GetComponentInParent<Hacker>();
        hacker.AddAi(this.gameObject);
    }

    public void OnTriggerExit(Collider col)
    {
        if (!col.gameObject.CompareTag(HackerTag))
        {
            return;
        }

        Hacker hacker = col.GetComponentInParent<Hacker>();
        hacker.RemoveAi(this.gameObject);
    }
    //TODO maybe RPC?
    public void DisableCursor()
    {
        hackerCursor.DisableCursor();
    }
    //TODO maybe RPC?
    public void EnableCursor()
    {
        hackerCursor.EnableCursor();
        Debug.Log("Cursor on the " + this.gameObject.name + " should been activated");
    }
}
