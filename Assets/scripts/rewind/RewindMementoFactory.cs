using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindMementoFactory{

	public IRewindMemento MakeRewindMemento(int memento, GameObject rewindable){
        switch (memento)
        {
            case 0: return new RewindMementoCharacter(rewindable);
            case 1: return new RewindMementoAI(rewindable);
            default: return null;
        }
    }
}
