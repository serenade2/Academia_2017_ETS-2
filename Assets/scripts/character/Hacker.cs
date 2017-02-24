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
	        SphereCollider childSphereCollider = GetComponentInChildren<SphereCollider>();
            childSphereCollider.radius = HackingRadius;
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
	        //Debug.Log("LB2 Pressed");
            NextTarget();
	    }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button5))
	    {
	        //Debug.Log("RB2 pressed");
            PreviousTarget();    
	    }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            //Debug.Log("A button Pressed");
            if (AiLinkedList.Count > 0)
            {
                TakeOver(GetCurrentAi());
            }
        }

        //Debug.Log(("total elements ") + AiLinkedList.Count);
	}
   
    public void AddAi(GameObject ai)
    {
        if (!hasAuthority) return;
        // check if the gameObject we add is an AICharacter
        if (ai.GetComponent<AICharacter>() != null)
        {
            if (AiLinkedList.Contains(ai))
            {
                return;
            }

            AiLinkedList.AddLast(ai);
            //Debug.Log(ai.gameObject.name + " has been added");
            //Debug.Log("total elements in " + AiLinkedList.Count);
        }
    }
   
    public void RemoveAi(GameObject ai)
    {
        if (!hasAuthority) return;
        AiLinkedList.Remove(ai);
        //Debug.Log(ai.gameObject.name + " has been removed!");
        //Debug.Log("total elements out " + AiLinkedList.Count);
    }

    public void NextTarget()
    {
        if (AiLinkedList.Count > 0 && _currentIndex < AiLinkedList.Count)
        {
            _currentIndex++;
        }
    }

    public void PreviousTarget()
    {
        if (_currentIndex > 0)
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
            
        // retrieve the direction the ai was walking before getting possessed
        Vector3 targetWalkingDirection = targetGameObject.transform.forward;
        Transform targetTransform = targetGameObject.transform;

        // Update on the client the mesh
        UpdateHackerMesh(targetGameObject);
        // update on the server the mesh
        CmdUpdateHackerMesh(targetGameObject);

        // Destroy the hacked target on the server and sync it on all the clients
        CmdDestroyAI(targetGameObject);

        // move the hacker at the same position of the ai
        this.transform.position = targetTransform.position;
        this.transform.rotation = targetTransform.rotation;
        this.transform.localScale = targetTransform.localScale;
    }

    [Command]
    public void CmdDestroyAI(GameObject ai)
    {
       // DestroyCharacter dest = ai.GetComponent<DestroyCharacter>();
       // dest.RpcDestroy();
       RemoveAi(ai);
       Destroy(ai);
    }

    [Command]
    public void CmdUpdateHackerMesh(GameObject targetGameObject)
    {
        MeshRenderer targetMeshRenderer = targetGameObject.GetComponent<MeshRenderer>();
        MeshFilter targetMeshFilter = targetGameObject.GetComponent<MeshFilter>();

        MeshRenderer currentMeshRenderer = GetComponent<MeshRenderer>();
        MeshFilter currentMeshFilter = GetComponent<MeshFilter>();

        // take the apparency of the AI
        currentMeshRenderer.materials = targetMeshRenderer.materials;
        currentMeshFilter.mesh = targetMeshFilter.mesh;
    }

    public void UpdateHackerMesh(GameObject targetGameObject)
    {
        MeshRenderer targetMeshRenderer = targetGameObject.GetComponent<MeshRenderer>();
        MeshFilter targetMeshFilter = targetGameObject.GetComponent<MeshFilter>();

        MeshRenderer currentMeshRenderer = GetComponent<MeshRenderer>();
        MeshFilter currentMeshFilter = GetComponent<MeshFilter>();

        // take the apparency of the AI
        currentMeshRenderer.materials = targetMeshRenderer.materials;
        currentMeshFilter.mesh = targetMeshFilter.mesh;
    }
}
