/**
The following code have been inpired by the basic networking tutorial provided by Unity3d.
 source : https://unity3d.com/learn/tutorials/topics/multiplayer-networking
¸*/

using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public float HorizontalSensitivity;
    public float VerticalSensitivity;
    public Color LocalPlayerColor;
    public GameObject BulletPrefab;
    public Transform BulletSpawn;
    public float BulletVelocity;
    /// <summary>
    /// called on clients for player objects for the local client (only)
    /// </summary>
    public override void OnStartLocalPlayer()
    {
        //Add all initalization that are exclusive to the client (local machine) here. (Cameras and Inputs)

        GetComponent<MeshRenderer>().material.color = LocalPlayerColor;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // check if the current player that is processed is the same player as the local machine.
        if (!isLocalPlayer)
        {
            return;
        }
        if ( Input.GetKeyDown(KeyCode.Space) )
        {
            CmdFire();
        }
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * HorizontalSensitivity;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * VerticalSensitivity;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
    }

    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        GameObject bullet = (GameObject)Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * BulletVelocity;

        // Spawn the bullet prefab on the server so that the other clients can see the bullet instantiation.
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }
}
