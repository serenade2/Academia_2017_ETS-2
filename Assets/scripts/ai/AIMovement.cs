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
    private int nbObjectives = 0;
    private NetworkAnimator networkAnimator;
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
                    if (objectiveScript.GetRestrictions()[0] == 1)
                    {
                        if (objectiveScript.AddUser())
                        {
                            objectives[nbObjectives] = objective;
                            nbObjectives++;
                        }
                    }
                }
                else if (aiType == AIType.SCIENTIST)
                {
                    if (objectiveScript.GetRestrictions()[1] == 1)
                    {
                        if (objectiveScript.AddUser())
                        {
                            objectives[nbObjectives] = objective;
                            nbObjectives++;
                        }
                    }
                }
                else
                {
                    if (objectiveScript.GetRestrictions()[2] == 1)
                    {
                        if (objectiveScript.AddUser())
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
            
    }

    public void ChangeDestination()
    {
        hasChangedPath = false;
        currentObjectiveIndex = Random.Range(0, nbObjectives);
        agent.SetDestination(objectives[currentObjectiveIndex].GetComponent<Objective>().GetUsingPos().position);
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
        }
        else
        {
            agent.Resume();
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
}
