using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindMementoCharacter : IRewindMemento{
    private Vector3 position;
    private Quaternion rotation;
    private float speed;
    private HackerCharacterController characterController;

    public RewindMementoCharacter(GameObject rewindable)
    {
        position = rewindable.transform.position;
        rotation = rewindable.transform.rotation;
        characterController = rewindable.GetComponent<HackerCharacterController>();
        speed = characterController.GetVelocity();
    }

    public void RestoreFromMemento(GameObject rewindable){
        rewindable.transform.position = position;
        rewindable.transform.rotation = rotation;
        characterController.UpdateAnimation(speed);
    }
}
