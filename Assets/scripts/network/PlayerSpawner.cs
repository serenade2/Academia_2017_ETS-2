using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSpawner : NetworkBehaviour {

    public GameObject hackerScientist;
    public GameObject hackerEngineer;
    public GameObject hackerGuard;

    public GameObject watcherPrefab;
    public GameObject objectiveManagerPrefab;

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
            GameObject[] player1spawns = GameObject.FindGameObjectsWithTag("Player1Spawn");

            GameObject character = RandomSelectCharacter();

            player = GameObject.Instantiate(character, player1spawns[Random.Range(0,player1spawns.Length)].transform.position, character.transform.rotation);
            NetworkServer.SpawnWithClientAuthority(player, gameObject);
			GameObject.Find("RewindManager(Clone)").GetComponent<RewindManager>().AddRewindable(player.GetComponent<Rewindable>());

            // spawn the objective manager which needs client authority
            GameObject objectiveManager = GameObject.Instantiate(objectiveManagerPrefab);
            NetworkServer.SpawnWithClientAuthority(objectiveManager, gameObject);
        }
        else
        {
			player = GameObject.Instantiate(watcherPrefab, GameObject.Find("player2Spawn").transform.position, watcherPrefab.transform.rotation);
			NetworkServer.Spawn(player);
        }
    }

    public GameObject RandomSelectCharacter()
    {

        List<GameObject> choices = new List<GameObject> {hackerEngineer, hackerGuard, hackerScientist};

        return choices[Random.Range(0, choices.Count)];


    }
}
