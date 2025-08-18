using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon_Slime_Attack2 : MonoBehaviour
{
    public Collider2D enemyCollider;
    [SerializeField] public Collider2D BossCollider;
    public float damage = 1;
    public float knockbackForce = 10f;
    public float dashForce = 200f;
    public float dashDuration = 0.5f;
    Vector2 rightAttackOffset;
    [SerializeField] private Rigidbody2D rb;
    Vector2 rightAttackOffsetscale;

    private void Start()
    {
        rightAttackOffset = transform.localPosition;
        rightAttackOffsetscale = transform.localScale;
        rb = GetComponentInParent<Rigidbody2D>();
    }
    public void JumpToPlayerRight(Vector2 targetPosition)
    {
        transform.localPosition = rightAttackOffset;
        transform.localScale = rightAttackOffsetscale;
        BossCollider.enabled = false;

        // Tính toán hướng dash
        Vector2 dashDirection = (targetPosition - (Vector2)transform.position).normalized;

        // Chạy Corountine để kiểm soát dash toàn thời gian
        StartCoroutine(DashCoroutine(dashDirection, dashForce, dashDuration));

        // Chạy animation
        Animator animator = GetComponentInParent<Animator>();
        animator.SetTrigger("Attack2");
    }
    public void JumpToPlayerLeft(Vector2 targetPosition)
    {
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
        transform.localScale = new Vector3(rightAttackOffsetscale.x * -1, rightAttackOffsetscale.y);
        BossCollider.enabled = false;

        Vector2 dashDirection = (targetPosition - (Vector2)transform.position).normalized;

        StartCoroutine(DashCoroutine(dashDirection, dashForce, dashDuration));

        Animator animator = GetComponentInParent<Animator>();
        animator.SetTrigger("Attack2");
    }

    private IEnumerator DashCoroutine(Vector2 direction, float force, float duration)
    {
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            rb.linearVelocity = direction * force;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
    }

    public void StopAttack()
    {
        enemyCollider.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();

        if (damageable != null)
        {
            Debug.Log("Attack hit a damageable object!");
            Vector3 parentPosition = gameObject.GetComponentInParent<Transform>().position;
            Vector2 direction = (Vector2)(other.gameObject.transform.position - parentPosition).normalized;
            Vector2 knockback = direction * knockbackForce;
            damageable.OnHit(damage, knockback);
        }
    }
}
