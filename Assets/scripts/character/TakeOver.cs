﻿using System;
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
    public GameObject DecoyPrefab;
    public GameObject HackedGuardPrefab;
    public GameObject HackedScientistPrefab;
    public GameObject HackedEngineerPrefab;
    public GameObject HackerDefaultPrefab;
    public GameObject HackedGuardNewPrefab;

    private HackerCursor hackerCursor;
    private GameObject _hackedGuard;
    private GameObject _hackedScientist;
    private GameObject _hackedEngineer;
    private GameObject _hackedGuardNew;
    private GameObject _hacker;
    //private NetworkAnimator _hackerAnimator;
    //private NetworkAnimator _hackerGuardAnimator;
    //private NetworkAnimator _hackerEngineerAnimator;
    //private NetworkAnimator _hackerScientistAnimator;
    private float _animationSpeed;
    // Use this for initialization
    public override void OnStartAuthority()
	{
        GameObject cursor = GameObject.Instantiate(CursorPrefab);
        //_animationSpeed = GetComponent<HackerCharacterController>().speed;

        if (HackedGuardPrefab != null && HackedScientistPrefab != null && HackedEngineerPrefab != null )
        {
            InitializeHackerModels();
            //InitializeHackerAnimators();
            DisableModels();
        }
        else
        {
            Debug.LogError("HackerGuardPrefab, HackedScientistPrefab or HackedEngineerPrefab is Missing!\n Please attach all the prefabs to the hacker in the editor");
        }
        

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

    public override void OnStartServer()
    {
        InitializeHackerModels();
        DisableModels();
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
            //UpdateHackerCursor(false);
            PreviousTarget();
            // enable the cursor on the next ai
            UpdateHackerCursor(true);
	    }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button5)) // RB Button pressed
	    {
            //UpdateHackerCursor(false);
            NextTarget();
            UpdateHackerCursor(true);
        }
        else if (Input.GetKeyDown(KeyCode.Joystick1Button0) || Input.GetKeyDown(KeyCode.A))// A button pressed
        {
            if (AiList.Count > 0)
            {
                StealIdentity(GetCurrentAi());
            }
        }

	    //UpdateHackedAnimations();
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
                hackerCursor.EnableCursor();
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
        SpawnDecoy(this.gameObject, this.transform.position, this.transform.rotation, DecoyLifeTime, 1.0f);
        CmdSpawnDecoy(this.gameObject, this.transform.position, this.transform.rotation, DecoyLifeTime, 1.0f);

        UpdatedHackedPrefabs(targetGameObject);
        CmdUpdateHackedPrefab(targetGameObject);

        //// Update on the client the mesh
        //UpdateHackerMesh(targetGameObject);
        //// update on the server the mesh
        //CmdUpdateHackerMesh(targetGameObject);

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
        //this.transform.localScale = targetTransform.localScale;
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
        //GameObject spawnedDecoy = GameObject.Instantiate(decoy);
        Vector3 decoyDirection = decoy.transform.forward;
        GameObject spawnedDecoy = GameObject.Instantiate(DecoyPrefab, targetPosition, targetQuaternion);
        
        spawnedDecoy.name = "DECOY_" + decoy.name;
        DecoyMovement decoyBehaviour = spawnedDecoy.GetComponent<DecoyMovement>();
        
        decoyBehaviour.InitialiseDecoy(decoyDirection, lifeTime, walkSpeed);
        //decoyBehaviour.TakeApparency(this.gameObject);
        decoyBehaviour.SetIsComplete(true);
        decoyBehaviour.EliminateDecoy(lifeTime); //TODO try removing the coupling
        //TODO play the desintegrate animation
    }

    [Command]
    public void CmdSpawnDecoy(GameObject decoy, Vector3 targetPosition, Quaternion targetQuaternion, float lifeTime, float walkSpeed)
    {
        SpawnDecoy(decoy, targetPosition, targetQuaternion, lifeTime, walkSpeed);
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

    //[Command]
    //public void CmdUpdateHackerMesh(GameObject targetGameObject)
    //{
    //    UpdateHackerMesh(targetGameObject);
    //}

    //public void UpdateHackerMesh(GameObject targetGameObject)
    //{
    //    MeshRenderer targetMeshRenderer = targetGameObject.GetComponent<MeshRenderer>();
    //    MeshFilter targetMeshFilter = targetGameObject.GetComponent<MeshFilter>();

    //    MeshRenderer currentMeshRenderer = GetComponent<MeshRenderer>();
    //    MeshFilter currentMeshFilter = GetComponent<MeshFilter>();

    //    // take the apparency of the AI 
    //    currentMeshRenderer.materials = targetMeshRenderer.materials;
    //    currentMeshFilter.mesh = targetMeshFilter.mesh;
    //}

    public void UpdateHackerCursor(bool visibleCondition)
    {
        GameObject currentAi = GetCurrentAi();
       
        if (currentAi == null)
            return;
        
        hackerCursor.SetTarget(currentAi);
        hackerCursor.IsVisible = visibleCondition;
    }

    private void InitializeHackerModels()
    {
        _hacker = GameObject.Instantiate(HackerDefaultPrefab, this.transform.position, this.transform.rotation);
        _hackedGuard = GameObject.Instantiate(HackedGuardPrefab, this.transform.position, this.transform.rotation);
        _hackedScientist = GameObject.Instantiate(HackedScientistPrefab, this.transform.position, this.transform.rotation);
        _hackedEngineer = GameObject.Instantiate(HackedEngineerPrefab, this.transform.position, this.transform.rotation);
        _hackedGuardNew = GameObject.Instantiate(HackedGuardNewPrefab, this.transform.position, this.transform.rotation);

        _hacker.transform.parent = this.gameObject.transform;
        //_hacker.transform.localScale = this.gameObject.transform.lossyScale;

        _hackedGuard.transform.parent = this.gameObject.transform;
        //_hackedGuard.transform.localScale = this.gameObject.transform.lossyScale;

        _hackedScientist.transform.parent = this.gameObject.transform;
        //_hackedScientist.transform.localScale = this.gameObject.transform.lossyScale;

        _hackedEngineer.transform.parent = this.gameObject.transform;
        //_hackedEngineer.transform.localScale = this.gameObject.transform.lossyScale;

        _hackedGuardNew.transform.parent = this.gameObject.transform;
    }

    //private void InitializeHackerAnimators()
    //{
    //    _hackerAnimator = _hacker.GetComponent<NetworkAnimator>();
    //    _hackerGuardAnimator = _hackedGuard.GetComponent<NetworkAnimator>();
    //    _hackerScientistAnimator = _hackedScientist.GetComponent<NetworkAnimator>();
    //    _hackerEngineerAnimator = _hackedEngineer.GetComponent<NetworkAnimator>();
    //}

    private void DisableModels()
    {
        // disable the models at the game start 
        _hackedGuard.SetActive(false);
        _hackedScientist.SetActive(false);
        _hackedEngineer.SetActive(false);
        _hackedGuardNew.SetActive(false);
    }

    public void UpdatedHackedPrefabs(GameObject targetGameObject)
    {
        //TODO Remove the renderer component on the hacker and create a hacker default mesh and assign it on a gameObject
        //this.GetComponent<MeshRenderer>().enabled = false; 
        //Activate the HackedGuardPrefab instead!
        if (targetGameObject.name.ToLower().Contains("new"))
        {
            _hacker.SetActive(false);
            _hackedGuard.SetActive(false);
            _hackedScientist.SetActive(false);
            _hackedEngineer.SetActive(false);
            _hackedGuardNew.SetActive(true);
        }
        else if (targetGameObject.name.ToLower().Contains("guard"))
        {
            _hacker.SetActive(false);
            _hackedGuard.SetActive(true);
            _hackedScientist.SetActive(false);
            _hackedEngineer.SetActive(false);
            _hackedGuardNew.SetActive(false);
        }
        else if (targetGameObject.name.ToLower().Contains("scientist"))
        {
            _hacker.SetActive(false);
            _hackedGuard.SetActive(false);
            _hackedScientist.SetActive(true);
            _hackedEngineer.SetActive(false);
            _hackedGuardNew.SetActive(false);
        }
        else if (targetGameObject.name.ToLower().Contains("engineer"))
        {
            _hacker.SetActive(false);
            _hackedGuard.SetActive(false);
            _hackedScientist.SetActive(false);
            _hackedEngineer.SetActive(true);
            _hackedGuardNew.SetActive(false);
        }
        else
        {
            Debug.LogError("No Ai has the following keyphrase in their name, : guard, scientist, engineer");
        }
    }

    [Command]
    public void CmdUpdateHackedPrefab(GameObject targetGameObject)
    {
        UpdatedHackedPrefabs(targetGameObject);    
    }

    //private void UpdateHackedAnimations()
    //{
    //    float h = Input.GetAxis("Horizontal1");              // the direction the model is facing
    //    float v = Input.GetAxis("Vertical1");                // the speed at wich the model is moving
       
    //    if (_hackedGuard.activeSelf)
    //    {
    //        _hackerGuardAnimator.animator.SetFloat("Speed", v);
    //        _hackerGuardAnimator.animator.SetFloat("Speed", h);
    //    }
    //    else if (_hackedScientist.activeSelf)
    //    {
    //        _hackerScientistAnimator.animator.SetFloat("Speed", v);
    //        _hackerScientistAnimator.animator.SetFloat("Speed", h);
    //    }
    //    else if (_hackedEngineer.activeSelf)
    //    {
    //        _hackerEngineerAnimator.animator.SetFloat("Speed", v);
    //        _hackerEngineerAnimator.animator.SetFloat("Direction", h);
    //    }
    //    else
    //    {
    //        _hackerAnimator.animator.SetFloat("Speed", v);
    //        _hackerAnimator.animator.SetFloat("Direction", h);
    //    }
    //} 
}
