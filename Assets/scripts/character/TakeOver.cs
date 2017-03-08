using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class TakeOver : NetworkBehaviour
{
    [Tooltip("The list that contains the Hackable AI")]
    public List<GameObject> AiList;
    [Tooltip("The radius that allow the hacker to take control of AI")]
    public float HackingRadius;
    public float DecoyLifeTime = 5.0f;
    public int _currentIndex;
    public GameObject CursorPrefab;
    private HackerCursor hackerCursor;

    // Use this for initialization
    public override void OnStartAuthority()
	{
        GameObject cursor = GameObject.Instantiate(CursorPrefab);
        hackerCursor = cursor.GetComponent<HackerCursor>();

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
                StealIdentity(GetCurrentAi());
            }
        }
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
                hackerCursor.SetTarget(ai);
            }

            AiList.Add(ai);
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
    }

    public void NextTarget()
    {
        if (_currentIndex < AiList.Count)
        {
            _currentIndex ++;

            if(_currentIndex >= AiList.Count)
            {
                _currentIndex = 0;
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

    public void StealIdentity(GameObject targetGameObject)
    {
        if (targetGameObject == null)
        {
            return;
        }
            
        // retrieve the direction the ai was walking before getting possessed
        Vector3 targetWalkingDirection = targetGameObject.transform.forward;
        Transform targetTransform = targetGameObject.transform;

        //TODO add an accessor in the class that use the NavMeshAgent to retreive the original ai speed 
        SpawnDecoy(this.gameObject, targetTransform.position, targetTransform.rotation, DecoyLifeTime, 50.0f);
        CmdSpawnDecoy(this.gameObject, targetTransform.position, targetTransform.rotation, DecoyLifeTime, 50.0f);

        // Update on the client the mesh
        UpdateHackerMesh(targetGameObject);
        // update on the server the mesh
        CmdUpdateHackerMesh(targetGameObject);

        try
        {
            // Destroy the hacked target on the server and sync it on all the clients
            CmdDestroyAI(targetGameObject);
        }
        catch (Exception ex)
        {
            print(ex.Message);
        }

        // move the hacker at the same position of the ai
        this.transform.position = targetTransform.position;
        this.transform.rotation = targetTransform.rotation;
        this.transform.localScale = targetTransform.localScale;
    }


    /// <summary>
    /// Spawn a fake Agent that walk in the direction of the previous agent
    /// before he desintegrate.
    /// </summary>
    /// <param name="decoy">The decoy agent that is created in the scene</param>
    /// <param name="target">The direction at which the decoy will walk towards</param>
    /// <param name="lifeTime">The time elapsed before the decoy dies</param>
    /// <param name="walkSpeed">The speed at which the decoy will move</param>
    public void SpawnDecoy(GameObject decoy, Vector3 targetPosition, Quaternion targetQuaternion, float lifeTime, float walkSpeed)
    {
        GameObject spawnedDecoy = GameObject.Instantiate(decoy);
        
        spawnedDecoy.name = "DECOY_" + decoy.name;
        spawnedDecoy.transform.parent = null;
        DetachDecoyComponents(spawnedDecoy);    

        Vector3 direction = targetPosition - decoy.transform.position;
        spawnedDecoy.transform.rotation = targetQuaternion;
        //TODO make the decoy walk in the same direction the original agent was facing.
        spawnedDecoy.transform.Translate(direction * walkSpeed * Time.deltaTime);
        //TODO play the desintegrate animation

        // then eliminate the decoy!
        try
        {
            Destroy(spawnedDecoy, lifeTime);
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
        }
    }

    [Command]
    public void CmdSpawnDecoy(GameObject decoy, Vector3 targetPosition, Quaternion targetQuaternion, float lifeTime, float walkSpeed)
    {
        SpawnDecoy(decoy, targetPosition, targetQuaternion, lifeTime, walkSpeed);
    }

    private void DetachDecoyComponents(GameObject spawnedDecoy)
    {
        try
        {
            Destroy(spawnedDecoy.GetComponentInChildren<SphereCollider>());
            Destroy(spawnedDecoy.GetComponent<TakeOver>());
            Destroy(spawnedDecoy.GetComponent<HackerCharacterController>());
            spawnedDecoy.GetComponent<CharacterController>().enabled = false;
        }
        catch(System.Exception ex)
        {
            print(ex.Message);
        }
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
        UpdateHackerMesh(targetGameObject);
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
