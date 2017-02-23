using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Hacker : NetworkBehaviour
{
    [Tooltip("The list that contains the Hackable AI")]
    public LinkedList<GameObject> AiLinkedList;

    private int _currentIndex;
	// Use this for initialization
	public override void OnStartLocalPlayer()
    {
		AiLinkedList = new LinkedList<GameObject>();
	    _currentIndex = 0;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (Input.GetKeyDown(KeyCode.Joystick1Button4))
	    {
	        Debug.Log("LB2 Pressed");
	    }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button5))
	    {
	        Debug.Log("RB2 pressed");    
	    }
	}

    public void AddAi(GameObject ai)
    {
        // check if the gameObject we add is an AICharacter
        if (ai.GetComponent<AICharacter>() != null)
        {
            AiLinkedList.AddLast(ai);
        }
    }
    public void RemoveAi(GameObject ai)
    {
        AiLinkedList.Remove(ai);
    }

    public void NextTarget()
    {
        if (_currentIndex < AiLinkedList.Count)
        {
            _currentIndex++;
        }
    }

    public void PreviousTarget()
    {
        if (_currentIndex > AiLinkedList.Count)
        {
            _currentIndex--;
        } 
    }

    public GameObject GetCurrentAi()
    {
        return AiLinkedList.ElementAt(_currentIndex);
    }
}
