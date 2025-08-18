using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    public float moveSpeed = 2f;
    public float dashSpeed = 5f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float collisionOffset = 0.01f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;
    public SwordAttack1 swordAttack1;
    public SwordAttack2 swordAttack2;
    public LayerMask PlayerLayer;
    public LayerMask EnemyLayer;
    Vector2 movementInput;
    public GameObject slashEffectPrefab;
    public GameObject slashEffect;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] private PlayerInput playerInput;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    private Sprite initialSprite;
    public SFXPlayerManager sFXPlayerManager;
    bool canMove = true;
    bool isDashing = false;
    bool isDashCooldown = false;

    protected override void Awake()
    {
        base.Awake();
        initialSprite = spriteRenderer.sprite;
        sFXPlayerManager = SFXPlayerManager.Instance;
        sFXPlayerManager = GameObject.FindGameObjectWithTag("AudioSFXPlayer").GetComponent<SFXPlayerManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (!isDashing)
            {
                // nếu movement không = 0 chạy thử move
                Vector2 desiredPosition = rb.position + movementInput * moveSpeed * Time.fixedDeltaTime;
                Vector2 smoothMovement = Vector2.Lerp(rb.position, desiredPosition, 0.8f); // Add smoothing
                bool success = TryMove(smoothMovement);
                animator.SetBool("isMoving", success);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }
        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private bool TryMove(Vector2 desiredPosition)
    {
        if (!isDashing && desiredPosition != rb.position)
        {
            // tạo tia cast tới vị trí mong muốn để tránh nv va phải collider và có thể xảy ra lỗi
            int count = rb.Cast((desiredPosition - rb.position).normalized, movementFilter, castCollisions, collisionOffset);
            if (count == 0)
            {
                rb.MovePosition(desiredPosition);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnAttack()
    {
        animator.SetTrigger("swordAttack");
    }
    void OnSlash()
    {
        Debug.Log("SlashAttack is called");
        animator.SetTrigger("SlashAttack");
    }
    public void TriggerSlashEffect()
    {
        if (slashEffect == null)
        {
            slashEffect = Instantiate(slashEffectPrefab, transform.position, Quaternion.identity);
        }
        // Lấy component và trigger
        SlashsEffect slashController = slashEffect.GetComponent<SlashsEffect>();
        if (slashController != null)
        {
            slashController.TriggerSlashEffect(spriteRenderer.flipX);
        }
    }

    void OnDash()
    {
        if (!isDashing && !isDashCooldown)
        {

            StartCoroutine(Dash());
        }
    }

    IEnumerator Dash()
    {
        if (sFXPlayerManager != null)
        {
            sFXPlayerManager.PlaySFX(sFXPlayerManager.Player_Dash);
        }
        isDashing = true;
        animator.SetBool("isDashing", true);
        canMove = false;
        float startTime = Time.time;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerLayer"), LayerMask.NameToLayer("EnemyLayer"), true);


        while (Time.time < startTime + dashDuration)
        {
            rb.linearVelocity = movementInput.normalized * dashSpeed;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerLayer"), LayerMask.NameToLayer("EnemyLayer"), false);
        isDashing = false;
        canMove = true;
        animator.SetBool("isDashing", false);

        isDashCooldown = true;
        yield return new WaitForSeconds(dashCooldown);
        isDashCooldown = false;
    }
    //Hitbox1
    public void StopAttack()
    {
        swordAttack.StopAttack();
    }
    public void SwordAttack()
    {
        LockMovement();
        if (spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
            if (sFXPlayerManager != null)
            {
                sFXPlayerManager.PlaySFX(sFXPlayerManager.Player_Attack1);
            }
        }
        else
        {
            swordAttack.AttackRight();
            if (sFXPlayerManager != null)
            {
                sFXPlayerManager.PlaySFX(sFXPlayerManager.Player_Attack1);
            }
        }
    }

    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
        if (slashEffect != null)
        {
            slashEffect.SetActive(false); // Deactivate the slash effect
        }
    }

    //Hitbox2
    public void SwordAttack1()
    {
        LockMovement();

        if (spriteRenderer.flipX == true)
        {
            swordAttack1.AttackLeft1();
            if (sFXPlayerManager != null)
            {
                sFXPlayerManager.PlaySFX(sFXPlayerManager.Player_Attack2);
            }
        }
        else
        {
            swordAttack1.AttackRight1();
            if (sFXPlayerManager != null)
            {
                sFXPlayerManager.PlaySFX(sFXPlayerManager.Player_Attack2);
            }
        }
    }

    public void EndSwordAttack1()
    {
        UnlockMovement();
        swordAttack1.StopAttack1();
        swordAttack.StopAttack();
    }

    //Hitbox3
    public void SwordAttack2()
    {
        LockMovement();

        if (spriteRenderer.flipX == true)
        {
            swordAttack2.AttackLeft2();
            if (sFXPlayerManager != null)
            {
                sFXPlayerManager.PlaySFX(sFXPlayerManager.Player_Attack3);
            }
        }
        else
        {
            swordAttack2.AttackRight2();
            if (sFXPlayerManager != null)
            {
                sFXPlayerManager.PlaySFX(sFXPlayerManager.Player_Attack3);
            }
        }
    }

    public void EndSwordAttack2()
    {
        UnlockMovement();
        swordAttack2.StopAttack2();
        swordAttack1.StopAttack1();
        swordAttack.StopAttack();
    }
    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }
    public void DisableInput()
    {
        playerInput.actions.FindActionMap("Player").Disable();
    }
    public void EnabledInput()
    {
        playerInput.actions.FindActionMap("Player").Enable();
    }
    public void ResetSprite()
    {
        spriteRenderer.sprite = initialSprite;
    }
}