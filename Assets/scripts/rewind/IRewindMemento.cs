using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRewindMemento
{
    void RestoreFromMemento(GameObject rewindable);
}
