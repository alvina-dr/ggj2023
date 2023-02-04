using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class HealthManager : MonoBehaviour
{

    public int maxHealth = 5;

    public int currentHealth = 0;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
    }

    public void GetDamage(int damage)
    {

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}