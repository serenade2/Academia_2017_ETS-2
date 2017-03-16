using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingZone : MonoBehaviour
{
    // delegate function for the  OnStateChange event
    private void OnStateChange()
    {
        Debug.Log("player wins");
        Debug.Log("current state: " + GameStateManager.Instance.gameState);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Hacker") return;

        GameStateManager.Instance.OnStateChange += OnStateChange;
        GameStateManager.Instance.SetGameState(GameState.End);
    }

    void OnDestroy()
    {
        // remove the delegate reference to avoid memory leaks
        if (GameStateManager.isActive)
        {
            GameStateManager.Instance.OnStateChange -= OnStateChange;
        }
    }
}
