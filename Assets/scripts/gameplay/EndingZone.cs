using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingZone : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hacker")
        {
            GameStateManager.Instance.SetGameState(GameState.End);
        }
    }
}
