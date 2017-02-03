/**
The following code have been inpired by the basic networking tutorial provided by Unity3d.
 source : https://unity3d.com/learn/tutorials/topics/multiplayer-networking
¸*/

using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;
    void OnCollisionEnter(Collision collision)
    {
        GameObject hitGameObject = collision.gameObject;
        var health = hitGameObject.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(bulletDamage);
        }

        Destroy(gameObject);
    }
}