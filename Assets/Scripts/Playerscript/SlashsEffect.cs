using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashsEffect : MonoBehaviour
{
    public Animator animator;
    public float cooldownDuration = 5f;
    public float damage = 1;
    public float knockbackForce = 10f;
    public SkillCooldownUI cooldownUI;
    private bool isOnCooldown = false;
    private Collider2D slashCollider;

    private void Start()
    {
        animator = GetComponent<Animator>();
        slashCollider = GetComponent<Collider2D>();
        gameObject.SetActive(false);
    }

    public void TriggerSlashEffect(bool isFacingLeft)
    {
        if (!isOnCooldown)
        {
            isOnCooldown = true;
            gameObject.SetActive(true);

            gameObject.SetActive(true);

            float offsetX = isFacingLeft ? -9.92f : 9.92f;
            Vector3 localOffset = new Vector3(offsetX, 0.8f, 0f);
            transform.localScale = new Vector3(isFacingLeft ? -1.5f : 1.5f, 1.5f, 1.5f);

            if (transform.parent != null)
            {
                transform.position = transform.parent.position + (transform.parent.localScale.x * localOffset);
            }
            else
            {
                transform.localPosition = localOffset;
            }
            if (slashCollider != null)
            {
                slashCollider.enabled = true;
            }

            animator.Play("Skill1", 0, 0f);
            Invoke("ResetCooldown", cooldownDuration);
            cooldownUI.StartCooldown(cooldownDuration);
        }
    }

    private void ResetCooldown()
    {
        isOnCooldown = false;
        gameObject.SetActive(false);

        if (slashCollider != null)
        {
            slashCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            Vector3 parentPosition = transform.parent.position;
            Vector2 direction = (other.transform.position - parentPosition).normalized;
            damageable.OnHit(damage, direction * knockbackForce);
        }
    }
}
