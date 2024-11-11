using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Demon_Slime_Attack3 : MonoBehaviour
{
    public Collider2D enemyCollider;
    public float damage = 1;
    public float knockbackForce = 10f;
    public UnityEvent onAdvanceCollider;
    Vector2 rightAttackOffset;
    Vector2 rightAttackOffsetscale;
    [SerializeField] private List<Collider2D> attackColliders; // Danh sách collider
    private int currentColliderIndex = 0;

    private void Start()
    {
        rightAttackOffset = transform.localPosition;
        rightAttackOffsetscale = transform.localScale;
    }

    public void AttackRight()
    {
        for (int i = 0; i < attackColliders.Count; i++)
        {
            attackColliders[i].enabled = (i == currentColliderIndex);
        }
        transform.localPosition = rightAttackOffset;
        transform.localScale = rightAttackOffsetscale;
    }

    public void AttackLeft()
    {
        for (int i = 0; i < attackColliders.Count; i++)
        {
            attackColliders[i].enabled = (i == currentColliderIndex);
        }
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);
        transform.localScale = new Vector3(rightAttackOffsetscale.x * -1, rightAttackOffsetscale.y);
    }

    public void StopAttack()
    {
    }
    public void AdvanceCollider()
    {
        onAdvanceCollider.Invoke();
        if (attackColliders.Count > 0)
        {
            currentColliderIndex = (currentColliderIndex + 1) % attackColliders.Count;
        }
        else
        {
            Debug.LogWarning("Không có collider nào trong attackColliders.");
        }
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
        if (damageable != null)
        {
            Debug.Log("Attack hit a damageable object: " + other.name);
        }
        else
        {
            Debug.Log("Triggered object is not damageable: " + other.name);
        }
    }
}
