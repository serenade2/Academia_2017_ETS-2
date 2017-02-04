using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class AIMovement : NetworkBehaviour, IRewindable {
    public GameObject[] objectives;
    private UnityEngine.AI.NavMeshAgent agent;
    private bool hasChangedPath = false; //Verify if path has changed for a new one
    private IEnumerator working;
    private bool isWorking = false;

    // Use this for initialization
    public override void OnStartServer()
    {
        if (!isServer)
        {
            return;
        }
        //Assign new coroutine
        working = Working();

        //Set agent
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.SetDestination(objectives[Random.Range(1, objectives.Length)].transform.position);

        //NetworkServer.Spawn(gameObject);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!isServer)
        {
            return;
        }
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
        if (!isServer)
        {
            return;
        }
        hasChangedPath = false;
        agent.ResetPath();
        agent.SetDestination(objectives[Random.Range(0, objectives.Length)].transform.position);
    }

    public void Rewind(bool isRewinding)
    {
        if (!isServer)
        {
            return;
        }
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
        if (!isServer)
        {
            return;
        }
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
        if (!isServer)
        {
            return;
        }
    }

    IEnumerator Working()
    {
        isWorking = true;
        for (int i = 0; i < Random.Range(3,5); i++)
        {
            yield return new WaitForSeconds(1f);
        }
        ChangeDestination();
        isWorking = false;
    }
}
