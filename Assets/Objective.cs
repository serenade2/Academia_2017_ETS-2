using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Objective : NetworkBehaviour {
	public bool isUsed = false;
	private int userCount = 0;
	public int maxUser = 3;
    public bool guardAccess = true;
    public bool scientistAccess = true;
    public bool engineerAccess = true;
    public Transform usingPos;
    public List<AIMovement> users = new List<AIMovement>();
    private GameObject currentUser;
    public int workingTime = 3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool GetGuardAccess()
    {
        return guardAccess;
    }
    public bool GetScientistAccess()
    {
        return scientistAccess;
    }
    public bool GetEngineerAccess()
    {
        return engineerAccess;
    }

    public bool GetIsUsed()
    {
        return isUsed;
    }

    public void SetIsUsed(bool isUsed)
    {
        this.isUsed = isUsed;
    }
     
    public bool AddUser(GameObject user)
    {
        if (userCount < maxUser)
        {
            users.Add(user.GetComponent<AIMovement>());
            userCount++;
            return true;
        }
        else
        {
            return false;
        }
    }

    public Transform GetUsingPos()
    {
        return usingPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("NPC") || other.CompareTag("Hacker")) && !isUsed)
        {
            isUsed = true;
            currentUser = other.gameObject;
            currentUser.GetComponent<AIMovement>().StartWorking(workingTime);
            foreach (AIMovement user in users)
            {
                user.ObjectiveTaken(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentUser == other.gameObject)
        {
            currentUser.GetComponent<AIMovement>().StopWorking();
            currentUser = null;
            isUsed = false;
        }
    }


}
