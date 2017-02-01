using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RewindMovement : MonoBehaviour {
    public float walkingSpeed = 2f;
    public NavMeshAgent agent;
    public Transform[] pathNodes;
    private bool hasChangedPath = false;

	// Use this for initialization
	void Start () {
        agent.SetDestination(pathNodes[Random.Range(1, 5)].position);
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (agent.remainingDistance == 0 && hasChangedPath == false)
        {
            hasChangedPath = true;
            Invoke("MoveAgain",Random.Range(1,5));
        }
	}

    public void CanMove(bool canMove)
    {
        if (canMove)
        {
            agent.Resume();
        }
        else
        {
            agent.Stop();
        }
    }

    public void MoveAgain()
    {
        hasChangedPath = false;
        agent.ResetPath();
        agent.SetDestination(pathNodes[Random.Range(0, pathNodes.Length)].position);
    }
}
