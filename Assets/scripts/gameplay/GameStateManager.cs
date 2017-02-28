using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Holds the different game states
/// </summary>
public enum GameState
{
    None,
    Intro,
    MainMenu,
    Game,
    End
}

/// <summary>
/// delegate which will be given by the caller of the SetGameState method
/// </summary>
public delegate void OnStateChangeHandler();

/// <summary>
/// Class that manages the game states and fires an event when transitioning
/// </summary>
public class GameStateManager : MonoBehaviour
{
    protected GameStateManager() { }        // make it protected so no one can instantiate it themselves
    private static GameStateManager instance = null;
    public event OnStateChangeHandler OnStateChange;

    public GameState gameState { get; private set; }
    public static bool isActive
    {
        get { return instance != null; }
    }


    public static GameStateManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<GameStateManager>();

                if (instance == null)
                {
                    GameObject gameObject  = new GameObject("GameStateManager");
                    DontDestroyOnLoad(gameObject);
                    instance = gameObject.AddComponent<GameStateManager>();
                }
            }

            return instance;
        }
    }

    public void SetGameState(GameState state)
    {
        this.gameState = state;
        if (OnStateChange != null)
        {
            OnStateChange();
        }
       
    }
}
