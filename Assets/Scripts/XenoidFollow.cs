using UnityEngine;

public class XenoidFollow : MonoBehaviour
{
    [Header("Vision")]
    [SerializeField] private float viewDistance = 8f;
    [SerializeField] private LayerMask visionMask;
    [SerializeField] private Transform eyePoint;

    [Header("Movement")]
    [SerializeField] private Transform walkPoint;
    [SerializeField] private float walkSpeed = 1.8f;
    [SerializeField] private float runSpeed = 5.2f;
    [SerializeField] private float attackRange = 1.5f;

    [Header("References")]
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;

    private GameObject player;
    private EnemyHealth enemyHealth;

    private bool lineOfSight = false;
    private bool hasSpotted = false;

    private bool isInSpottedIdle = false;
    private float spottedTimer = 0f;

    [Header("Attack")]
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackCooldown = 1.2f;
    private float lastAttackTime = -999f;

    private float lostPlayerTimer = 0f;
    [SerializeField] private float lostPlayerDelay = 1f;

    private bool damageTriggered = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyHealth = GetComponent<EnemyHealth>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (enemyHealth != null && enemyHealth.IsDead)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsWalking", false);
            return;
        }

        if (enemyHealth != null && enemyHealth.IsTakingDamage && !damageTriggered)
        {
            EnterSpottedIdle();
            damageTriggered = true;
        }

        if (enemyHealth != null && !enemyHealth.IsTakingDamage)
        {
            damageTriggered = false;
        }

        CheckVision();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void CheckVision()
    {
        if (player == null || eyePoint == null) return;

        Vector2 dir = (player.transform.position - eyePoint.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(
            eyePoint.position,
            dir,
            viewDistance,
            visionMask
        );

        bool rayHitPlayer = hit.collider != null && hit.collider.CompareTag("Player");

        
        if (hasSpotted)
        {
            lineOfSight = true;
        }
        else
        {
            lineOfSight = rayHitPlayer;
        }

        Debug.DrawRay(eyePoint.position, dir * viewDistance, lineOfSight ? Color.green : Color.red);

        
        if (lineOfSight && !hasSpotted)
        {
            animator.SetTrigger("Spotted");
            animator.SetBool("IsWalking", false);
            hasSpotted = true;
        }

        // Reset lost timer kad ga vidi
        if (lineOfSight)
        {
            lostPlayerTimer = 0f;
        }
    }


    void HandleMovement()
    {
        if (HandleSpottedIdle())
            return;

        if (!hasSpotted && walkPoint != null)
        {
            HandlePatrol();
            return;
        }

        if (!lineOfSight)
        {
            HandleLostPlayer();
            return;
        }

        float distance = Vector2.Distance(rb.position, player.transform.position);

        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            HandleAttack();
            return;
        }

        HandleChase();
    }

    bool HandleSpottedIdle()
    {
        if (!isInSpottedIdle)
            return false;

        animator.SetBool("IsRunning", false);
        animator.SetBool("IsWalking", false);

        spottedTimer -= Time.deltaTime;
        if (spottedTimer <= 0f)
            isInSpottedIdle = false;

        return true;
    }

    void HandlePatrol()
    {
        if (hasSpotted)
            return;

        animator.SetBool("IsRunning", false);
        animator.SetBool("IsWalking", true);

        Vector2 newPos = Vector2.MoveTowards(rb.position, walkPoint.position, walkSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Vector2.Distance(rb.position, walkPoint.position) < 0.1f)
            animator.SetBool("IsWalking", false);
    }

    void HandleLostPlayer()
    {
        lostPlayerTimer += Time.deltaTime;

        if (lostPlayerTimer >= lostPlayerDelay)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsWalking", false);
            hasSpotted = false;
        }

    }

    void HandleAttack()
    {
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsWalking", false);
        animator.SetTrigger("Attack");

        player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);

        lastAttackTime = Time.time;

        EnterSpottedIdle();
    }

    void HandleChase()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", true);

        Vector2 chasePos = Vector2.MoveTowards(rb.position, player.transform.position, runSpeed * Time.fixedDeltaTime);
        rb.MovePosition(chasePos);
    }

    public void EnterSpottedIdle()
    {
        animator.SetTrigger("ReturnToSpottedIdle");
        isInSpottedIdle = true;
        spottedTimer = 1f;
    }
}