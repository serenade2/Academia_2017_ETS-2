using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class AIMovement : NetworkBehaviour, IRewindable
{
    public GameObject[] objectives;
	public LinkedList<int> objectiveHistory = new LinkedList<int>();
    private UnityEngine.AI.NavMeshAgent agent;
    private bool hasChangedPath = false; //Verify if path has changed for a new one
    private IEnumerator working;
    private bool isWorking = false;
	private int currentObjectiveIndex;
    private bool isRewinding;

    // Use this for initialization
    public void Start()
    {

        objectives = GameObject.FindGameObjectsWithTag("PathNode");

        //Set agent
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = true;

        ChangeDestination();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //When destination is reached
        if (agent.remainingDistance <= 0 && hasChangedPath == false && !agent.pathPending && !isRewinding)
        {
            hasChangedPath = true;

            if(objectiveHistory.Count > 0)
            {
                objectiveHistory.RemoveFirst();
                if (objectiveHistory.Count > 0)
                    ChangeDestination(objectiveHistory.First.Value);
                else
                    ChangeDestination();
            }
            else
            {
                ChangeDestination();
            }
        }

        /*if (agent.desiredVelocity.magnitude > 0)
            transform.rotation = Quaternion.LookRotation(agent.desiredVelocity, transform.up);*/
            
    }

    public void ChangeDestination()
    {
        hasChangedPath = false;
        currentObjectiveIndex = Random.Range(1, objectives.Length);
        agent.SetDestination(objectives[currentObjectiveIndex].transform.position);
    }

    public void ChangeDestination(int objectiveIndex)
    {
        hasChangedPath = false;
        currentObjectiveIndex = objectiveIndex;
        agent.SetDestination(objectives[currentObjectiveIndex].transform.position);
    }

    public void Rewind(bool isRewinding)
    {
        if (isRewinding)
        {
            isRewinding = true;
            agent.Stop();
            hasChangedPath = false;
        }
        else
        {
            isRewinding = false;
            agent.Resume();
        }
    }

    public void Pause(bool isPaused)
    {
        if (isPaused)
        {
            agent.Stop();
        }
        else
        {
            agent.Resume();
        }
    }

    public void FastForward(bool isFastForwarding)
    {

    }

    public int GetCurrentObjectiveIndex()
    {
        return currentObjectiveIndex;
    }

    public LinkedList<int> GetObjectiveHistory()
    {
        return objectiveHistory;
    }
}
