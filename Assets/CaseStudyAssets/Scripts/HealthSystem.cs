using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private float maximumHealth = 100.0f;
    [SerializeField]
    private float currentHealth = 100.0f;
    [SerializeField]
    private float enemyDamageAmount = 10.0f;

    private void Start()
    {
        UpdateUI();
    }

    public void TakeDamage()
    {
        currentHealth -= enemyDamageAmount;
        UpdateUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void FatalDamage()
    {
        currentHealth = 0;
        UpdateUI();
    }

    private void Die()
    {
        GameObject.FindObjectOfType<Player>().KillPlayer();
    }

    private void UpdateUI()
    {
        GameObject.FindObjectOfType<HealthBar>().UpdateHealthBar(currentHealth, maximumHealth);
    }
}
