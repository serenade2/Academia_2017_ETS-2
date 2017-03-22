using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class AIMovement : NetworkBehaviour, IRewindable
{
    public enum AIType { GUARD, SCIENTIST, ENGINEER };
    public AIType aiType = AIType.GUARD;
    public int objectivePerAI;
    [SerializeField]
    private GameObject[] objectives;
	private LinkedList<int> objectiveHistory = new LinkedList<int>();
    private UnityEngine.AI.NavMeshAgent agent;
    private float agentVelocity;
    private bool hasChangedPath = false; //Verify if path has changed for a new one
	private int currentObjectiveIndex;
    private bool isRewinding;
    private bool isWorking = false;
    private int nbObjectives = 0;
    private NetworkAnimator networkAnimator;
    private IEnumerator workingCoroutine;

    // Use this for initialization
    public void Start()
    {
        if (!hasAuthority)
            return;
        InitializeAi();
    }

    [Command]
    void CmdInitializeAi()
    {
        InitializeAi();
    }
    void InitializeAi()
    {

        objectives = new GameObject[objectivePerAI];

        //Get this AI objectives to patrol
        GameObject[] allObjectives = GameObject.FindGameObjectsWithTag("Objective");
        shuffleGameObjectArray(allObjectives);
        foreach (GameObject objective in allObjectives)
        {
            if (nbObjectives < objectivePerAI)
            {
                Objective objectiveScript = objective.GetComponent<Objective>();
                if (aiType == AIType.GUARD)
                {
                    if (objectiveScript.GetGuardAccess())
                    {
                        if (objectiveScript.AddUser(gameObject))
                        {
                            objectives[nbObjectives] = objective;
                            nbObjectives++;
                        }
                    }
                }
                else if (aiType == AIType.SCIENTIST)
                {
                    if (objectiveScript.GetScientistAccess())
                    {
                        if (objectiveScript.AddUser(gameObject))
                        {
                            objectives[nbObjectives] = objective;
                            nbObjectives++;
                        }
                    }
                }
                else
                {
                    if (objectiveScript.GetEngineerAccess())
                    {
                        if (objectiveScript.AddUser(gameObject))
                        {
                            objectives[nbObjectives] = objective;
                            nbObjectives++;
                        }
                    }
                }
            }
        }

        //Set agent
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = true;

        networkAnimator = GetComponent<NetworkAnimator>();
        ChangeDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority)
            return;

        agentVelocity = agent.desiredVelocity.magnitude;
        UpdateAnimation(agentVelocity);
        //print("DesiredVelocity SqrMagnitude:" + agent.desiredVelocity.sqrMagnitude);
        //When destination is reached
        /*if (agent.remainingDistance <= 0 && hasChangedPath == false && !agent.pathPending && !isRewinding)
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
        }*/
            
    }

    public void ChangeDestination()
    {
        hasChangedPath = false;
        currentObjectiveIndex = Random.Range(0, nbObjectives);
        agent.SetDestination(objectives[currentObjectiveIndex].GetComponent<Objective>().GetUsingPos().position);
        if (objectives[currentObjectiveIndex].GetComponent<Objective>().GetIsUsed())
            ChangeDestination();
    }

    public void ChangeDestination(int objectiveIndex)
    {
        hasChangedPath = false;
        currentObjectiveIndex = objectiveIndex;
        agent.SetDestination(objectives[currentObjectiveIndex].GetComponent<Objective>().GetUsingPos().position);
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
            StopCoroutine(workingCoroutine);
        }
        else
        {
            agent.Resume();
            StartCoroutine(workingCoroutine);
        }
    }

    public void FastForward(bool isFastForwarding)
    {

    }

    public void shuffleGameObjectArray(GameObject[] array)
    {
        List<GameObject> tempList = new List<GameObject>(); 
        foreach(GameObject obj in array)
        {
            tempList.Add(obj);
        }
        for(int i = 0; i < array.Length; i++)
        {
            int tempIndex = Random.Range(0, tempList.Count-1);
            array[i] = tempList[tempIndex];
            tempList.RemoveAt(tempIndex);
        }
    }

    public int GetCurrentObjectiveIndex()
    {
        return currentObjectiveIndex;
    }

    public LinkedList<int> GetObjectiveHistory()
    {
        return objectiveHistory;
    }

    public void UpdateAnimation(float velocity)
    {
        networkAnimator.animator.SetFloat("Speed", velocity);
    }

    public void ObjectiveTaken(GameObject objective)
    {
        if(objective == objectives[currentObjectiveIndex] && !isWorking)
        {
            ChangeDestination();
        }
    }

    public void StartWorking(int delay)
    {
        isWorking = true;
        workingCoroutine = Working(delay);
        StartCoroutine(workingCoroutine);
    }

    public void StopWorking()
    {
        isWorking = false;
        StopCoroutine(workingCoroutine);
    }

    IEnumerator Working(int delay)
    {
        for(int i = 0; i <= delay; i++)
        {
            yield return new WaitForSeconds(1f);
        }
        isWorking = false;
        ChangeDestination();
    }

}
