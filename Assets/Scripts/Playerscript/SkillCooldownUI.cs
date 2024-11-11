using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooldownUI : MonoBehaviour
{
    public Image cooldownFillImage;
    public float cooldownDuration;

    private float cooldownStartTime;
    private bool isOnCooldown = false;

    void LateUpdate()
    {
        if (isOnCooldown)
        {
            float timeElapsed = Time.time - cooldownStartTime;
            float fillAmount = Mathf.Clamp01(1 - (timeElapsed / cooldownDuration));

            cooldownFillImage.fillAmount = fillAmount;

            if (timeElapsed >= cooldownDuration - 0.01f)
            {
                isOnCooldown = false;
                cooldownFillImage.fillAmount = 0;
            }
        }
    }

    public void StartCooldown(float duration)
    {
        if (!isOnCooldown)
        {
            cooldownDuration = duration;
            cooldownStartTime = Time.time;
            isOnCooldown = true;
            cooldownFillImage.fillAmount = 1f;
        }
    }
}
