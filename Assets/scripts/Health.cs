/**
The following code have been inpired by the basic networking tutorial provided by Unity3d.
 source : https://unity3d.com/learn/tutorials/topics/multiplayer-networking
¸*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour
{
    private NetworkStartPosition[] spawnPoints;
    public int MaxHealth;
    [SyncVar(hook = "OnChangeHealth")]
    private int currentHealth;
    public RectTransform HealthBarForeground;
    public bool DestroyOnDeath;
    // Use this for initialization;
    void Start ()
    {
        currentHealth = MaxHealth;

        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
    }
    
    // Update is called once per frame
    void Update ()
    {
        
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
        {
            return;
        }
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Debug.Log(string.Format("{0} is dead!", gameObject.name));

            if (DestroyOnDeath)
            {
                Destroy(gameObject);
            }
            else
            {
                currentHealth = MaxHealth;
                //called on the Server, but invoked on the Clients
                RpcRespawn();
            }
          
        }
    }

    public void OnChangeHealth(int currentHealth)
    {
        HealthBarForeground.sizeDelta = new Vector2(currentHealth, HealthBarForeground.sizeDelta.y);
    }

    [ClientRpc]
    void RpcRespawn()
    {
        if (isLocalPlayer)
        {
           // Set the spawn point to origin as a default value
            Vector3 spawnPoint = Vector3.zero;

            if (spawnPoints != null && spawnPoints.Length > 0)
            {
                spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position;
            }

            // Set the player's position to the chosen spawn point
            transform.position = spawnPoint;
        }
    }
}
