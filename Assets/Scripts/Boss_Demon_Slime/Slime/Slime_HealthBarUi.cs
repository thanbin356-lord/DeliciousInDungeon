using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slime_HealthBarUi : MonoBehaviour
{
    public Slime bossHealth;
    public Image healthBarFill;

    void Start()
    {
        if (bossHealth == null)
        {
            Debug.LogError("BossHealth not assigned to HealthBarUI!");
            return;
        }

        healthBarFill.fillAmount = 1f;
    }

    void Update()
    {
        if (bossHealth != null)
        {
            healthBarFill.fillAmount = bossHealth.Health / bossHealth.maxHealth;
        }
    }
}
