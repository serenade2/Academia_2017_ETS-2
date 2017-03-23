using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectiveManager : NetworkBehaviour
{
    protected ObjectiveManager() { }        // make it protected so no one can instantiate it themselves
    private static ObjectiveManager instance = null;

    [Tooltip("Hacker needs to activate this number of NPCObjectives to win")]
    public int numberOfObjectivesToWin = 3;

    public ActivatableObjective activatableObjectivePrefab;

    private List<Objective> NPCObjectives;
    private List<ActivatableObjective> hackerObjectives = new List<ActivatableObjective>();


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
	public override void OnStartClient ()
	{


        if(isServer)
            return;

	    NPCObjectives = GameObject.FindObjectsOfType<Objective>().ToList();
        SpawnHackerObjectives();
	}


    void SpawnHackerObjectives()
    {
        for (int i = 0; i < numberOfObjectivesToWin; i++)
        {
            int randomIndex = Random.Range(0, NPCObjectives.Count);
            Objective obj = NPCObjectives[randomIndex];
            NPCObjectives.RemoveAt(randomIndex);

            // instantiate a hacker objective at the NPC objective station
            ActivatableObjective hackerObj = GameObject.Instantiate(activatableObjectivePrefab, obj.transform.position, Quaternion.identity, obj.transform);

            hackerObjectives.Add(hackerObj);
        }
    }


    [Command]
    public void CmdCompleteObjective(GameObject door)
    {
        door.GetComponent<ActivatableObjective>().CmdActivate();
    }

}
