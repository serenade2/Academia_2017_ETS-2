using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectiveManager : NetworkBehaviour
{
    public ActivatableDoor[] doors;
    protected ObjectiveManager() { }        // make it protected so no one can instantiate it themselves
    private static ObjectiveManager instance = null;

    private InteractionManager[] objectives;

    public static bool isActive
    {
        get { return instance != null; }
    }


    public static ObjectiveManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ObjectiveManager>();

                if (instance == null)
                {
                    GameObject gameObject = new GameObject("ObjectiveManager");
                    instance = gameObject.AddComponent<ObjectiveManager>();
                }
            }

            return instance;
        }
    }


	// Use this for initialization
	void Start ()
	{
	    objectives = GameObject.FindObjectsOfType<InteractionManager>();
	}

    [Command]
    public void CmdCompleteObjective(GameObject door)
    {
        door.GetComponent<ActivatableDoor>().CmdActivate();
    }

}
