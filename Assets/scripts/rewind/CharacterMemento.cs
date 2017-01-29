using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMemento {
    private Vector3 pos;

    public CharacterMemento(Vector3 pos)
    {
        this.pos = pos;
    }

    public Vector3 GetPos()
    {
        return pos;
    }

    public override string ToString()
    {
        return "pos : " + pos;
    }
}
