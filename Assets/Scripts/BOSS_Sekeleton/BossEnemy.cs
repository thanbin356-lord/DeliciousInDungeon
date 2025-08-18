using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour, IDamageable
{
    public float damage = 1;
    public float knockbackForce = 5f;
    public float moveSpeed = 1f; 
    public float detectionRadius = 5f;
    public LayerMask playerLayer;
    public float knockbackDuration = 0.2f;
    public float maxHealth = 50f;
    private float currentHealth;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;
    private Vector3 initialLocalPosition;
    private Vector3 initialLocalScale;
    private Vector2[] originalPath;
    public LayerMask obstructionLayer;
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isKnockedBack = false;
    public Boss_Attack attackScript;

     public float Health
    {
        set {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);       
            if (currentHealth > 0) {
                animator.SetTrigger("IsAttacked");              
            }
            if (currentHealth <= 0)
            {
                Defeated();
            }
        }
        get {
            return currentHealth;
        }
    }

    public bool Targetable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }


    void Start()
    {
        currentHealth = maxHealth;
        PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
        if (polygonCollider != null)
        {
            originalPath = polygonCollider.points;
            initialLocalPosition = polygonCollider.transform.localPosition;
        }
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackScript = GetComponentInChildren<Boss_Attack>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (currentHealth>0)
        {
            DetectPlayer();
            if (player != null && CanAttack())
            {
                if (Vector2.Distance(transform.position, player.position) <= attackRange)
                {
                    Attack();
                }
                else
                {
                    MoveTowardsPlayer();
                }
            }
        }
    }

    void DetectPlayer()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerLayer);

        if (colliders.Length > 0)
        {
            player = colliders[0].transform;
        }
        else
        {
            player = null;
        }
    }
    private IEnumerator DisableAttackCollider()
    {
        yield return new WaitForSeconds(1.20f);
        attackScript.StopAttack();
    }

    private bool CanAttack()
    {
        return Time.time - lastAttackTime >= attackCooldown && HasLineOfSight();
    }
    private bool HasLineOfSight()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstructionLayer);

        return hit.collider == null || hit.collider.gameObject == player.gameObject;
    }
    private void Attack()
    {
        lastAttackTime = Time.time;
        if (spriteRenderer.flipX == true)
        {
            attackScript.AttackLeft();
            animator.SetTrigger("Attack");
        }
        else
        {
            attackScript.AttackRight();
            animator.SetTrigger("Attack");
        }

        StartCoroutine(DisableAttackCollider());
    }
    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
            animator.SetTrigger("IsRunning");
            if (direction.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (direction.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            bool facingLeft = (direction.x < 0);
            spriteRenderer.flipX = facingLeft;
            PolygonCollider2D polygonCollider = GetComponent<Collider2D>() as PolygonCollider2D;

            if (polygonCollider != null)
            {
                Vector2[] flippedPath = new Vector2[originalPath.Length];
                for (int i = 0; i < originalPath.Length; i++)
                {
                    flippedPath[i] = originalPath[i];
                    flippedPath[i].x *= facingLeft ? -1 : 1;
                }
                polygonCollider.points = flippedPath;

                Vector2 offset = polygonCollider.offset;
                offset.x = initialLocalPosition.x * (facingLeft ? 0 : 0);
                polygonCollider.offset = offset;
            }
        }
    }


    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;
        StartCoroutine(Knockback(knockback));
    }
    public void OnHit(float damage)
    {
        Health -= damage;
    }


    private IEnumerator Knockback(Vector2 knockback)
    {
        isKnockedBack = true;
        rb.AddForce(knockback);

        yield return new WaitForSeconds(knockbackDuration);

        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }

    public void Defeated()
    {
        animator.SetTrigger("Defeated");
    }

    public void RemoveEnemy()
    {
        Destroy(gameObject);
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
