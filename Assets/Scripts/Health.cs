using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField, Range(1, 100)]
    private int maxHealth;

    private int health;

    private void Start()
    {
        health = maxHealth;
    }

    public void ReduceHealth(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
