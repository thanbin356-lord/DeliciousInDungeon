using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Attack : MonoBehaviour
{
    public Collider2D enemyCollider;
    public float damage = 1;
    public float knockbackForce = 10f;
    Vector2 rightAttackOffset;
    Vector2 rightAttackOffsetscale;

    private void Start()
    {
        rightAttackOffset = transform.localPosition;
        rightAttackOffsetscale = transform.localScale;
    }

    public void AttackRight()
    {
        transform.localPosition = rightAttackOffset;
        transform.localScale = rightAttackOffsetscale;
    }

    public void AttackLeft()
    {
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
        transform.localScale = new Vector3(rightAttackOffsetscale.x * -1, rightAttackOffsetscale.y);
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

