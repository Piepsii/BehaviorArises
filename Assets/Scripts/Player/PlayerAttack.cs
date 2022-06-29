using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField, Range(0, 50)]
    private int damage;
    private ParticleSystem pSystem;
    private bool hasShot;

    void Start()
    {
        pSystem = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            pSystem.Play();
            hasShot = true;
        }
        else
        {
            hasShot = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") && hasShot)
        {
            var health = other.GetComponent<Health>();
            health.ReduceHealth(damage);
        }
    }
}
