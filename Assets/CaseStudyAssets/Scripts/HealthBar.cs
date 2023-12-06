using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Transform playerTransform;
    public Image healthBar_Foreground;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        healthBar_Foreground.fillAmount = 1.0f;
    }

    private void LateUpdate()
    {
        transform.LookAt(playerTransform.position + Camera.main.transform.forward);
    }

    public void UpdateHealthBar(float currentHealth, float maximumHealth)
    {
        healthBar_Foreground.fillAmount = currentHealth / maximumHealth;
    }
}
