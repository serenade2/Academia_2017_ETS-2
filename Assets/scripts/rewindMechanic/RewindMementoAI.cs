using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class RewindMementoAI : IRewindMemento
{
    Vector3 position;
    GameObject currentObjective;
    Vector3 destination;
    NavMeshAgent navMesh;

    public RewindMementoAI(GameObject rewindable)
    {
        /*navMesh = rewindable.GetComponent<NavMeshAgent>();
        position = rewindable.transform.position;
        destination = navMesh.destination;*/
        position = rewindable.transform.position;
        currentObjective = rewindable.GetComponent<AIMovement>().GetCurrentObjective();
    }

    public void RestoreFromMemento(GameObject rewindable)
    {
        rewindable.transform.position = position;
        rewindable.GetComponent<AIMovement>().ChangeDestination(currentObjective);
        //navMesh.SetDestination(destination);
    }
}
