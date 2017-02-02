using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindMementoCharacter : IRewindMemento{
    private Vector3 position;

    public RewindMementoCharacter(GameObject rewindable)
    {
        position = rewindable.transform.position;
    }

    public void RestoreFromMemento(GameObject rewindable){
        rewindable.transform.position = position;
    }
}
