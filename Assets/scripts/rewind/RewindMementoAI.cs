using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class RewindMementoAI : IRewindMemento {
    Vector3 position;
    Vector3 destination;

	public RewindMementoAI(GameObject rewindable)
    {
        position = rewindable.transform.position;
        destination = rewindable.GetComponent<NavMeshAgent>().destination;
    }

    public void RestoreFromMemento(GameObject rewindable){
        //rewindable.GetComponent<NavMeshAgent>().Stop();
        rewindable.transform.position = position;
        rewindable.GetComponent<NavMeshAgent>().SetDestination(destination);
    }
}
