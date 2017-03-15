using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class AIMovement : NetworkBehaviour, IRewindable
{
    public GameObject[] objectives;
	public LinkedList<GameObject> objectiveHistory = new LinkedList<GameObject>();
    public bool rewindMode = false;
    private UnityEngine.AI.NavMeshAgent agent;
    private bool hasChangedPath = false; //Verify if path has changed for a new one
    private IEnumerator working;
    private bool isWorking = false;
	private GameObject currentObjective;

    // Use this for initialization
    void Start()
    {
        objectives = GameObject.FindGameObjectsWithTag("PathNode");

        //Assign new coroutine
        working = Working();

        //Set agent
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		currentObjective = objectives [Random.Range (1, objectives.Length)];
        agent.SetDestination(currentObjective.transform.position);
    }

    // Update is called once per frame
    void LateUpdate()
    {

        //When destination is reached
        if (agent.remainingDistance <= 0 && hasChangedPath == false && !agent.pathPending)
        {
            hasChangedPath = true;

            //Starts working timer
            working = Working();
            StartCoroutine(working);
        }
    }

    public void ChangeDestination()
    {
        hasChangedPath = false;
        agent.ResetPath();
        currentObjective = objectives[Random.Range(1, objectives.Length)];
        agent.SetDestination(currentObjective.transform.position);
    }

    public void ChangeDestination(GameObject objective)
    {
        hasChangedPath = false;
        agent.ResetPath();
        currentObjective = objective;
        agent.SetDestination(currentObjective.transform.position);
    }

    public void Rewind(bool isRewinding)
    {
        if (isRewinding)
        {
            agent.Stop();
            StopCoroutine(working);
            isWorking = false;
            hasChangedPath = false;
        }
        else
        {
            agent.Resume();
        }
    }

    public void Pause(bool isPaused)
    {
        if (isPaused)
        {
            agent.Stop();
            StopCoroutine(working);
        }
        else
        {
            agent.Resume();
            if (isWorking)
            {
                StartCoroutine(working);
            }
        }
    }

    public void FastForward(bool isFastForwarding)
    {

    }

    IEnumerator Working()
    {
        isWorking = true;
        for (int i = 0; i < Random.Range(3, 5); i++)
        {
            yield return new WaitForSeconds(1f);
        }
        ChangeDestination();
        isWorking = false;
    }

    public GameObject GetCurrentObjective()
    {
        return currentObjective;
    }
}
