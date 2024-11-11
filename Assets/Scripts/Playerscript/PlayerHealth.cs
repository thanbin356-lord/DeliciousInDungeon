using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private DefeatSceneManager defeatSceneManager;
    [SerializeField] private GameOver gameOverManager;
    private List<string> gameOverScenes = new List<string> { "SampleScene", "Scene2", "BossScene" };
    public float maxHealth = 10f;
    public float knockbackDuration = 0.2f;
    public float knockbackForce = 5f;
    private PlayerController playerController;
    private float currentHealth;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isKnockedBack = false;
    private bool IsAttacked = false;

    public SFXPlayerManager sFXPlayerManager;
    private void Awake()
    {
        sFXPlayerManager = SFXPlayerManager.Instance;
        sFXPlayerManager = GameObject.FindGameObjectWithTag("AudioSFXPlayer").GetComponent<SFXPlayerManager>();
    }
    public float Health
    {
        set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            if (currentHealth > 0)
            {
                animator.SetTrigger("IsAttacked");
                if (sFXPlayerManager != null)
                {
                    sFXPlayerManager.PlaySFX(sFXPlayerManager.Player_GetHit);
                }
                IsAttacked = true;
                playerController.DisableInput();
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
        playerController = GetComponent<PlayerController>();
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    private void FixedUpdate()
    {
        if (IsAttacked && !animator.GetCurrentAnimatorStateInfo(27).IsName("IsAttacked"))
        {
            IsAttacked = false;
            playerController.EnabledInput();
            Debug.Log("Input Enabled");
        }
    }

    public void OnHit(float damage, Vector2 knockback)
    {
        Health -= damage;
        StartCoroutine(Knockback(knockback));
    }

    private IEnumerator Knockback(Vector2 knockback)
    {
        isKnockedBack = true;
        rb.AddForce(knockback * knockbackForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        rb.velocity = Vector2.zero;
        isKnockedBack = false;
    }

    public void Defeated()
    {
        if (sFXPlayerManager != null)
        {
            sFXPlayerManager.PlaySFX(sFXPlayerManager.Player_Death);
        }
        animator.SetTrigger("Defeated");
        gameObject.layer = LayerMask.NameToLayer("Default");
        playerController.DisableInput();
        if (gameOverScenes.Contains(SceneManager.GetActiveScene().name))
        {
            gameOverManager.ShowGameOverScreen();
        }
        else
        {
            defeatSceneManager.LoadDefeatScene();
        }
    }

    public void Heal(float amount)
    {
        Health = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void OnHit(float damage)
    {
        Health -= damage;
    }
}
