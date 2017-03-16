using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SplashScreenInputHandler : MonoBehaviour {

    [Tooltip("Index of the next Scene to load after the Splash Screen")]
    public int nextSceneIndex;

    [HideInInspector]
    public bool willAutoskip = true;

    [HideInInspector]
    public float timeBeforeAutoSkip = 10f;

    [Tooltip("The screen is skippable by the user if true, else the user must wait for the TimeBeforeAutoSkip")]
    [HideInInspector]
    public bool isSkippable = true;

    [HideInInspector]
    public float timeBeforeSkippable = 0f;

    private bool canNowSkip = false;  // use internally to know when it can be skipped

    // Update is called once per frame
    void Update()
    {
        if (canNowSkip && Input.anyKeyDown)
        {
            GameStateManager.Instance.SetGameState(GameState.MainMenu);
        }

    }

    void Awake()
    {
        GameStateManager.Instance.OnStateChange += OnStateChangeDelegate;
    }

    void Start()
    {
        if (isSkippable)
        {
            StartCoroutine(EnableSkip());
        }

        if (willAutoskip)
        {
            StartCoroutine(AutoSkip());
        }
    }

    IEnumerator AutoSkip()
    {
        yield return new WaitForSeconds(timeBeforeAutoSkip);
        GameStateManager.Instance.SetGameState(GameState.MainMenu);

    }

    IEnumerator EnableSkip()
    {
        yield return new WaitForSeconds(timeBeforeSkippable);
        canNowSkip = true;

    }

    // Delegate function for GameStateManager onStateChange
    private void OnStateChangeDelegate()
    {
        SceneManager.LoadScene(nextSceneIndex);
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(SplashScreenInputHandler))]
public class MyScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var myScript = target as SplashScreenInputHandler;

        EditorGUILayout.LabelField("Must have at least of the following options !", EditorStyles.boldLabel);

        GUIContent autoSkipToggle = new GUIContent("Will AutoSkip", "The next scene will load itself automatically after a certain time if True");
        myScript.willAutoskip = EditorGUILayout.Toggle(autoSkipToggle, myScript.willAutoskip);

        if (myScript.willAutoskip)
        {
            GUIContent field = new GUIContent("Time Before AutoSkip", "Seconds before the next scene is automatically loaded");
            myScript.timeBeforeAutoSkip = EditorGUILayout.FloatField(field, myScript.timeBeforeAutoSkip);
        }
        else if (!myScript.isSkippable)
        {
            myScript.willAutoskip = true;
        }

        GUIContent skippableToggle = new GUIContent("Is Skippable", "The screen is skippable by the user if true, else the user must wait for the TimeBeforeAutoSkip");
        myScript.isSkippable = EditorGUILayout.Toggle(skippableToggle, myScript.isSkippable);

        if (myScript.isSkippable)
        {
            GUIContent field = new GUIContent("Time Before Skippable", "Seconds before the user can skip the splash screen with a button press");
            myScript.timeBeforeSkippable = EditorGUILayout.FloatField(field, myScript.timeBeforeSkippable);
        }
        else if (!myScript.willAutoskip)
        {
            myScript.isSkippable = true;
        }

    }
}

#endif
