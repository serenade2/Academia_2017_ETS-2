using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class RewindMementoAI : IRewindMemento
{
    Vector3 position;
    Vector3 destination;
    NavMeshAgent navMesh;

    public RewindMementoAI(GameObject rewindable)
    {
        navMesh = rewindable.GetComponent<NavMeshAgent>();
        position = rewindable.transform.position;
        destination = navMesh.destination;
    }

    public void RestoreFromMemento(GameObject rewindable)
    {
        rewindable.transform.position = position;
        navMesh.SetDestination(destination);
    }
}
