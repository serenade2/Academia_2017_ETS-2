using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int MaxHealth;
    private int currentHealth;
    public RectTransform HealthBarForeground;
    // Use this for initialization;
    void Start ()
    {
        currentHealth = MaxHealth;
    }
    
    // Update is called once per frame
    void Update ()
    {
        
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        HealthBarForeground.sizeDelta = new Vector2(currentHealth, HealthBarForeground.sizeDelta.y);

        if (currentHealth > 0)
        {
            return;
        }

        currentHealth = 0;
       

        Debug.Log(string.Format("{0} is dead!", gameObject.name));
    }
}
