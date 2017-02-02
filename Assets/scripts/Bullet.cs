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