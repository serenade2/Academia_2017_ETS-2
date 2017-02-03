/**
The following code have been inpired by the basic networking tutorial provided by Unity3d.
 source : https://unity3d.com/learn/tutorials/topics/multiplayer-networking
¸*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour
{
    public GameObject EnemyPrefab;
    public int NumberOfEnemies;
    public float PositionMinRange;
    public float PositionMaxRange;
    public float RotationMinRange;
    public float RotationMaxRange;

    public override void OnStartServer()
    {
        for (var i = 0; i < NumberOfEnemies; ++i)
        {
            Vector3 spawnPosition = 
                new Vector3(Random.Range(PositionMinRange,PositionMaxRange), 0.0f, Random.Range(PositionMinRange,PositionMaxRange));

            Quaternion spawnRotation = Quaternion.Euler(0.0f, Random.Range(RotationMinRange, RotationMaxRange), 0.0f);

            GameObject enemy = (GameObject) Instantiate(EnemyPrefab, spawnPosition, spawnRotation);

            // spawn the enemy on the server
            NetworkServer.Spawn(enemy);
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
