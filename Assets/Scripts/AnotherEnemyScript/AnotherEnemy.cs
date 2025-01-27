using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnotherEnemy : MonoBehaviour, IDamageable
{
    public float damage = 1;
    public float knockbackForce = 5f; // Lực knockback
    public float moveSpeed = 1f; 
    public float detectionRadius = 5f; // Radar quái
    public LayerMask playerLayer; 
    public float knockbackDuration = 0.2f; // Thời gian knockback
    public float health = 3;
    public float attackRange = 1f; 
    public float attackCooldown = 1f; 
    private float lastAttackTime = 0f;
    public LayerMask obstructionLayer;
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isKnockedBack = false;
    public AnotherEnemy_Attack attackScript;
    private PatrolBehavior patrolBehavior;


    public float Health
    {
        set
        {
            health = value;
            if (health > 0)
            {
                animator.SetTrigger("IsAttacked");
            }
            if (health <= 0)
            {
                Defeated();
            }
        }
        get
        {
            return health;
        }
    }

    public bool Targetable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackScript = GetComponentInChildren<AnotherEnemy_Attack>();
        patrolBehavior = GetComponent<PatrolBehavior>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (!isKnockedBack)
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
            patrolBehavior.enabled = false;
        }
        else
        {
            player = null;
            patrolBehavior.enabled = true;
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

        // đây là los hay nói cách khác chính là chắn tầm nhìn
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
            patrolBehavior.enabled = false;
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

        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }

    public void Defeated()
    {
        animator.SetTrigger("Defeated");
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
        }
        rb.isKinematic = true;
        enabled = false;
    }

    public void RemoveEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
