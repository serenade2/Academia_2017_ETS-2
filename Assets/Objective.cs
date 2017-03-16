using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Objective : NetworkBehaviour {
	private bool isUsed = false;
	private int userCount = 0;
	private int maxUser = 3;
	private int[] restrictions = { 0, 0, 0 };

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetUserCount()
    {
        return userCount;
    }

    public bool GetIsUsed()
    {
        return isUsed;
    }

    public void AddUser()
    {
        userCount++;
    }


}
