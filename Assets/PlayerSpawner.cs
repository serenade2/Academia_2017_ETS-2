using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSpawner : NetworkBehaviour {
    public GameObject player1Prefab;
    public GameObject player2Prefab;

    private GameObject player;

    // Use this for initialization
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        if (!isLocalPlayer)
        {
            return;
        }
        CmdSpawnPlayer(isServer, gameObject);
    }

    [Command]
    void CmdSpawnPlayer(bool isPlayer2, GameObject playerAuthority)
    {
        if (!isPlayer2)
        {
            player = GameObject.Instantiate(player1Prefab, GameObject.Find("player1Spawn").transform.position, player1Prefab.transform.rotation);
        }
        else
        {
            player = GameObject.Instantiate(player2Prefab, GameObject.Find("player2Spawn").transform.position, player1Prefab.transform.rotation);
        }

        NetworkServer.SpawnWithClientAuthority(player, gameObject);
        GameObject.Find("RewindManager").GetComponent<RewindManager>().AddRewindable(player.GetComponent<Rewindable>());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
