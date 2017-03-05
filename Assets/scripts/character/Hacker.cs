using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class Hacker : NetworkBehaviour
{
    [Tooltip("The list that contains the Hackable AI")]
    public List<GameObject> AiList;
    [Tooltip("The radius that allow the hacker to take control of AI")]
    public float HackingRadius;
    public float DecoyLifeTime;
    public int _currentIndex;
    private HackerCursor hackerCursor;

    // Use this for initialization
    public override void OnStartAuthority()
	{
        hackerCursor = GameObject.Find("Cursor").GetComponent<HackerCursor>();

        if (hackerCursor != null)
        {
            //hackerCursor.Target = this.gameObject;
            hackerCursor.Target = null;
        }
        
        if (!hasAuthority) return;
		AiList = new List<GameObject>();
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
        //if(AiList.Count == 0 || _currentIndex == -1)
        //{
        //    _currentIndex = 0; // force the index to be minus one a null equivalent
        //}

        if (!hasAuthority)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button4)) // LB button pressed
	    {
            // disable the previous cursor
            UpdateHackerCursor(false);
            PreviousTarget();
            // enable the cursor on the next ai
            UpdateHackerCursor(true);
	    }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button5)) // RB Button pressed
	    {
            UpdateHackerCursor(false);
            NextTarget();
            UpdateHackerCursor(true);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button0))// A button pressed
        {
            if (AiList.Count > 0)
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
            if (AiList.Contains(ai))
            {
                return;
            }
            if (hackerCursor.Target == null)
            {
                hackerCursor.Target = ai;
            }

            AiList.Add(ai);
            //Debug.Log(ai.gameObject.name + " has been added");
            //Debug.Log("total elements in " + AiLinkedList.Count);
        }
    }
   
    public void RemoveAi(GameObject ai)
    {
        if (!hasAuthority) return;

        //before removing the ai find at which position it was
        //AiLinkedList.
        if (!AiList.Contains(ai))
        {
            return;
        }
        if (GetCurrentAi().name.Equals(ai.name))
        {
            hackerCursor.SetTarget(null);
        }
        AiList.Remove(ai);

        //update the current index
        PreviousTarget();

        //Debug.Log(ai.gameObject.name + " has been removed!");
        //Debug.Log("total elements out " + AiLinkedList.Count);
    }

    public void NextTarget()
    {
        if (AiList.Count > 0)
        {
            print("Nb elements" + AiList.Count);

            if(_currentIndex >= AiList.Count)
            {
                _currentIndex = 0;
            }
            else
            {
                print("Before index increment:" + _currentIndex);
                _currentIndex++;
                print("After incrementation : " + _currentIndex);
            }
        }
    }

    public void PreviousTarget()
    {
        _currentIndex--;

        if(_currentIndex < 0)
        {
            if(AiList.Count > 0)
            {
                _currentIndex = AiList.Count - 1;
            }
            else
            {
                _currentIndex = 0;
            }
        }
    }

    public GameObject GetCurrentAi()
    {
        if(AiList.Count != 0)
        {
            return AiList.ElementAt(_currentIndex);
        }
        else
        {
            return null;
        }
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
        RemoveAi(ai);
        RpcDestroyAI(ai);
        Destroy(ai);
    }

    [ClientRpc]
    public void RpcDestroyAI(GameObject ai)
    {
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

    public void UpdateHackerCursor(bool visibleCondition)
    {
        GameObject currentAi = GetCurrentAi();

        if (currentAi == null)
            return;
        hackerCursor.SetTarget(currentAi);
        hackerCursor.IsVisible = visibleCondition;
    }
}
