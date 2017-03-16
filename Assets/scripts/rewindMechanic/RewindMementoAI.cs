using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class RewindMementoAI : IRewindMemento
{
    private Vector3 position;
    private int currentObjectiveIndex;
    private AIMovement aiMovement;

    public RewindMementoAI(GameObject rewindable)
    {
        aiMovement = rewindable.GetComponent<AIMovement>();
        position = rewindable.transform.position;
        currentObjectiveIndex = aiMovement.GetCurrentObjectiveIndex();
    }

    public void RestoreFromMemento(GameObject rewindable)
    {
        rewindable.transform.position = position;
        rewindable.GetComponent<AIMovement>().ChangeDestination(currentObjectiveIndex);

        //Add objectives made after rewind
        LinkedList<int> objectiveHistory = aiMovement.GetObjectiveHistory();
        if (objectiveHistory.Count > 0){
            if (objectiveHistory.Last.Value != currentObjectiveIndex && objectiveHistory.First.Value != currentObjectiveIndex)
                    objectiveHistory.AddFirst(currentObjectiveIndex);
        }
        else
        {
            objectiveHistory.AddFirst(currentObjectiveIndex);
        }
    }
}
