using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSpawner : NetworkBehaviour {
    public GameObject hackerPrefab;
    public GameObject watcherPrefab;

    private GameObject player;

    // Use this for initialization
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        if (!isLocalPlayer)
        {
            return;
        }
        if (isServer)
        {
            //GameObject.Find("player1Camera").SetActive(false);
        }
        else
        {
            //GameObject.Find("player2Camera").SetActive(false);
        }
        CmdSpawnPlayer(isServer);
    }

    [Command]
    void CmdSpawnPlayer(bool isPlayer2)
    {
        if (!isPlayer2)
        {
            player = GameObject.Instantiate(hackerPrefab, GameObject.Find("player1Spawn").transform.position, hackerPrefab.transform.rotation);
            NetworkServer.SpawnWithClientAuthority(player, gameObject);
			GameObject.Find("RewindManager(Clone)").GetComponent<RewindManager>().AddRewindable(player.GetComponent<Rewindable>());
        }
        else
        {
			player = GameObject.Instantiate(watcherPrefab, GameObject.Find("player2Spawn").transform.position, watcherPrefab.transform.rotation);
			NetworkServer.Spawn(player);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
