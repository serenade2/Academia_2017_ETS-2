using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindMementoFactory{
    public enum Type : int {CHARACTER, AI};  

	public IRewindMemento MakeRewindMemento(Type memento, GameObject rewindable){
        switch (memento)
        {
            case Type.CHARACTER: return new RewindMementoCharacter(rewindable);
            case Type.AI: return new RewindMementoAI(rewindable);
            default: return null;
        }
    }
}
