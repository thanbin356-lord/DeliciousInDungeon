using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUi : Singleton<HealthBarUi>
{
    public PlayerHealth playerHealth;
    public Image healthBarFill;

    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth not assigned to HealthBarUI!");
            return;
        }

        healthBarFill.fillAmount = 1f; // Start with full health
    }

    void Update()
    {
        if (playerHealth != null)
        {
            // Calculate the fill amount (0 to 1) based on current/max health
            healthBarFill.fillAmount = playerHealth.Health / playerHealth.maxHealth;
        }
    }
}
