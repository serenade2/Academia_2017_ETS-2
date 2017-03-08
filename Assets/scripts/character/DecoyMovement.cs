using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecoyMovement : MonoBehaviour
{
    public Vector3 TargetPosition;
    public Quaternion TargetQuaternion;
    public float LifeTime;
    public float WalkSpeed;
    private Vector3 _walkDirection;
    // Use this for initialization
    void Start ()
    {
      
    }

    public void InitialiseDecoy(Vector3 targetPosition, Quaternion targetQuaternion)
    {
        _walkDirection = TargetPosition - this.transform.position;
        this.transform.rotation = TargetQuaternion;
    }
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.Translate(_walkDirection * WalkSpeed * Time.deltaTime);
    }

    public void TakeApparency(GameObject hackerGameObject)
    {
        MeshRenderer targetMeshRenderer = hackerGameObject.GetComponent<MeshRenderer>();
        MeshFilter targetMeshFilter = hackerGameObject.GetComponent<MeshFilter>();

        MeshRenderer currentMeshRenderer = GetComponent<MeshRenderer>();
        MeshFilter currentMeshFilter = GetComponent<MeshFilter>();

        // take the apparency of the Hacker
        currentMeshRenderer.materials = targetMeshRenderer.materials;
        currentMeshFilter.mesh = targetMeshFilter.mesh;
    }
}
