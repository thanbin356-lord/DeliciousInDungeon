using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack1 : MonoBehaviour
{
    public Collider2D swordCollider;
    public float damage = 1;
    public float knockbackForce = 10f;
    Vector2 rightAttackOffset;
    Vector2 rightAttackOffsetscale;


    private void Start()
    {
        rightAttackOffsetscale = transform.localScale;
        rightAttackOffset = transform.localPosition;
    }

    public void AttackRight1()
    {
        swordCollider.enabled = true;
        transform.localScale = rightAttackOffsetscale;
        transform.localPosition = rightAttackOffset;
    }

    public void AttackLeft1()
    {
        swordCollider.enabled = true;
        transform.localScale = new Vector3(rightAttackOffsetscale.x * -1, rightAttackOffsetscale.y);
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
    }

    public void StopAttack1()
    {
        swordCollider.enabled = false;
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

        if (other.tag == "ObjectCanbeDestroyed")
        {
            ObjectCanbeDestroyed objectCanbeDestroyed = other.GetComponent<ObjectCanbeDestroyed>();

            if ((objectCanbeDestroyed != null))
            {
                objectCanbeDestroyed.Health -= damage;
            }
        }
    }
}
