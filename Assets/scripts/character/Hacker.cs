using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Hacker : NetworkBehaviour
{
    [Tooltip("The list that contains the Hackable AI")]
    public LinkedList<GameObject> AiLinkedList;
    [Tooltip("The radius that allow the hacker to take control of AI")]
    public float HackingRadius;

    public float DecoyLifeTime;
    private int _currentIndex;
	// Use this for initialization
	public override void OnStartAuthority()
	{
	    if (!hasAuthority) return;
		AiLinkedList = new LinkedList<GameObject>();
	    if (HackingRadius > 0)
	    {
	        CharacterController car = GetComponent<CharacterController>();
	        car.radius = HackingRadius;
	    }
	    _currentIndex = 0;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    if (!hasAuthority)
	    {
	        return;
	    }

	    if (Input.GetKeyDown(KeyCode.Joystick1Button4))
	    {
	        Debug.Log("LB2 Pressed");
            NextTarget();
	    }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button5))
	    {
	        Debug.Log("RB2 pressed");
            PreviousTarget();    
	    }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            Debug.Log("A button Pressed");
            TakeOver(GetCurrentAi());
        }
	}
   
    public void AddAi(GameObject ai)
    {
        if (!hasAuthority) return;
        // check if the gameObject we add is an AICharacter
        if (ai.GetComponent<AICharacter>() != null)
        {
            AiLinkedList.AddLast(ai);
            Debug.Log(ai.gameObject.name + " has been added");
        }
    }
   
    public void RemoveAi(GameObject ai)
    {
        if (!hasAuthority) return;
        AiLinkedList.Remove(ai);
        Debug.Log(ai.gameObject.name + " has been removed!");
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
        return AiLinkedList.Count > 0 ? AiLinkedList.ElementAt(_currentIndex) : null;
    }

    public void TakeOver(GameObject targetGameObject)
    {
        if (targetGameObject == null)
        {
            return;
        }
            
        // retreive the direction the ai was walking before getting possesed
        Vector3 targetWalkingDirection = targetGameObject.transform.forward;
        Transform targetTransform = targetGameObject.transform;
        // Destroy the hacked target
        Destroy(targetGameObject);
        // move the hacker at the same position of the ai
        this.transform.position = targetGameObject.transform.position;
        this.transform.rotation = targetGameObject.transform.rotation;
    }
}
