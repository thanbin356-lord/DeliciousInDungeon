using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour, IDamageable
{
    public float damage = 1;
    public float knockbackForce = 5f;
    public float moveSpeed = 1f;
    public float detectionRadius = 5f;
    public LayerMask playerLayer;
    public float knockbackDuration = 0.2f;
    public float maxHealth = 20f;
    private float currentHealth;
    public float attackRange = 1f;
    public float attackCooldown = 1f;
    private float lastAttackTime = 0f;
    public LayerMask obstructionLayer;
    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isKnockedBack = false;
    public Slime_Attack attackScript;
    public GameObject phase2demon;
    public GameObject enemyGameObject;
    public AudioManager audioManager;
    private void Awake()
    {

    }


    public float Health
    {
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            if (currentHealth > 0)
            {
                animator.SetTrigger("IsAttacked");
            }
            if (currentHealth <= 0)
            {
                Defeated();
            }
        }
        get
        {
            return currentHealth;
        }
    }

    public bool Targetable { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }


    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackScript = GetComponentInChildren<Slime_Attack>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (currentHealth > 0)
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
            if (direction.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (direction.x < 0)
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
        SpriteRenderer enemySpriteRenderer = enemyGameObject.GetComponent<SpriteRenderer>();
        if (enemySpriteRenderer != null)
        {
            enemySpriteRenderer.enabled = false;
        }

        Collider2D enemyCollider = enemyGameObject.GetComponent<Collider2D>();
        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }
        Vector3 spawnPosition = transform.position;
        GameObject newDemon = Instantiate(phase2demon, spawnPosition, Quaternion.identity);
        newDemon.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
