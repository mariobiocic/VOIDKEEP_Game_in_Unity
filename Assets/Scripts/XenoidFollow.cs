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

        // Enter spotted idle samo jednom po damage-u
        if (enemyHealth != null && enemyHealth.IsTakingDamage && !damageTriggered)
        {
            EnterSpottedIdle();
            damageTriggered = true;
        }

        // Resetiraj trigger kad se damage završi
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

        lineOfSight = hit.collider != null && hit.collider.CompareTag("Player");

        Debug.DrawRay(eyePoint.position, dir * viewDistance, lineOfSight ? Color.green : Color.red);

        if (lineOfSight && !hasSpotted)
        {
            animator.SetTrigger("Spotted");
            animator.SetBool("IsWalking", false);
            hasSpotted = true;
        }
    }

    void HandleMovement()
    {
        // SPOTTED IDLE
        if (isInSpottedIdle)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsWalking", false);

            spottedTimer -= Time.deltaTime;
            if (spottedTimer <= 0f)
                isInSpottedIdle = false;

            return;
        }

        // WALK / PATROL
        if (!hasSpotted && walkPoint != null)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsWalking", true);

            Vector2 newPos = Vector2.MoveTowards(rb.position, walkPoint.position, walkSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            if (Vector2.Distance(rb.position, walkPoint.position) < 0.1f)
                animator.SetBool("IsWalking", false);

            return;
        }

        // IZGUBIO PLAYERA - vrati se u normalni idle/patrol
        if (!lineOfSight)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsWalking", false);
            hasSpotted = false;
            return;
        }

        float distance = Vector2.Distance(rb.position, player.transform.position);

        // ATTACK
        if (distance <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsWalking", false);
            animator.SetTrigger("Attack");

            player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage);

            lastAttackTime = Time.time;

            // Enter spotted idle kao kod TakeDamage
            EnterSpottedIdle();
            return;
        }

        // RUN (CHASE)
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", true);

        Vector2 chasePos = Vector2.MoveTowards(rb.position, player.transform.position, runSpeed * Time.fixedDeltaTime);
        rb.MovePosition(chasePos);
    }

    // Funkcija za ulazak u spotted idle
    public void EnterSpottedIdle()
    {
        animator.SetTrigger("ReturnToSpottedIdle");
        isInSpottedIdle = true;
        spottedTimer = 1f; // èekanje toèno 1 sekundu
    }

}
