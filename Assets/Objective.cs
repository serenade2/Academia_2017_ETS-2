using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Objective : NetworkBehaviour {
	private bool isUsed = false;
	private int userCount = 0;
	public int maxUser = 3;
	public int[] restrictions = { 0, 0, 0 };
    public Transform usingPos;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int[] GetRestrictions()
    {
        return restrictions;
    }

    public bool GetIsUsed()
    {
        return isUsed;
    }
    public void SetIsUsed(bool isUsed)
    {
        this.isUsed = isUsed;
    }

    public bool AddUser()
    {
        if (userCount < maxUser)
        {
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


}
