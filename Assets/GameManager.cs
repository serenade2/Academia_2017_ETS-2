using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {
    public NetworkManager networkManager;
    public int[] playerRole = new int[2];

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int[] getPlayerRole()
    {
        return playerRole;
    }

    [Command]
    public void CmdSetPlayer(int role) {
        //playerRole[networkManager.] = role; 
    }


    public void OnPlayerConnected()
    {
        print("hola");
    }

}
