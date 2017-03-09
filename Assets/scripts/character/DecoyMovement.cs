using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class DecoyMovement : NetworkBehaviour
{
    public Vector3 TargetPosition;
    public Quaternion TargetQuaternion;
    public float LifeTime;
    public float WalkSpeed;
    public Vector3 _walkDirection;
    public bool _isComplete;
    //private bool _isDead;
    // Use this for initialization
    public void Start()
    {
        _isComplete = false;
        //_isDead = false;
    }

    public void InitialiseDecoy(Vector3 direction, float lifeTime, float walkSpeed)
    {
        this._walkDirection = direction;
        this.LifeTime = lifeTime;
        this.WalkSpeed = walkSpeed;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    //if (!_isComplete) return;
        //this.transform.position += _walkDirection * WalkSpeed * Time.deltaTime;
	    //if (_isComplete)
	    //{
            this.transform.Translate(_walkDirection * this.WalkSpeed * Time.deltaTime);
        //}
	    //if (_isDead)
	    //{
	    //    Destroy(this.gameObject);
	    //}
	}

    public void TakeApparency(GameObject hackerGameObject)
    {
        //if (!hasAuthority) return;
        MeshRenderer targetMeshRenderer = hackerGameObject.GetComponent<MeshRenderer>();
        MeshFilter targetMeshFilter = hackerGameObject.GetComponent<MeshFilter>();

        MeshRenderer currentMeshRenderer = GetComponent<MeshRenderer>();
        MeshFilter currentMeshFilter = GetComponent<MeshFilter>();

        // take the apparency of the Hacker
        currentMeshRenderer.materials = targetMeshRenderer.materials;
        currentMeshFilter.mesh = targetMeshFilter.mesh;
        this.transform.localScale = hackerGameObject.transform.localScale;
        //this._isComplete = true; //TODO for some reason this doesn't work...
    }

    public void SetLifeTime(float lifeTime)
    {
        this.LifeTime = lifeTime;
    }

    public void SetWalkSpeed(float walkSpeed)
    {
        this.WalkSpeed = walkSpeed;
    }

    public void SetWalkDirection(Vector3 walkDirection)
    {
        this._walkDirection = walkDirection;
    }

    public void SetIsComplete(bool isComplete)
    {
        this._isComplete = isComplete;
    }

    public void EliminateDecoy(float lifeTime)
    {
        try
        {
            Destroy(this.gameObject, lifeTime);
        }
        catch (System.Exception ex)
        {
            print(ex.Message);
        }
    }
}
