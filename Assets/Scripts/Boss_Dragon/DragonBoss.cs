using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBoss : MonoBehaviour, IDamageable
{
    public float damage = 1;
    public float knockbackForce = 5f;
    public float detectionRadius = 5f;
    public LayerMask playerLayer;
    public float knockbackDuration = 0.2f;
    public float maxHealth = 100f;
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
    public Boss_Dragon_Attack attackScript;
    public Boss_Dragon_Breath breathScript;
    public AudioManager audioManager;
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
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
        PolygonCollider2D polygonCollider = GetComponent<PolygonCollider2D>();
        if (polygonCollider != null)
        {
            originalPath = polygonCollider.points;
            initialLocalPosition = polygonCollider.transform.localPosition;
        }
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackScript = GetComponentInChildren<Boss_Dragon_Attack>();
        breathScript = GetComponentInChildren<Boss_Dragon_Breath>();
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
        breathScript.StopAttack();
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
        int randomAttack = Random.Range(0, 2); // 0 or 1

        if (randomAttack == 0)
        {
            if (audioManager != null)
            {
                audioManager.PlaySFX(audioManager.Dragon_Fire);
            }
            animator.SetTrigger("AttackBreath");
        }
        else
        {
            if (audioManager != null)
            {
                audioManager.PlaySFX(audioManager.Dragon_Stomp);
            }
            animator.SetTrigger("AttackEarthQuake");
        }

        StartCoroutine(DisableAttackCollider());
    }

    public void AdvanceCollider()
    {
        if (attackScript != null)
        {
            attackScript.AdvanceCollider();
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
