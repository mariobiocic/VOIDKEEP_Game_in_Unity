using UnityEngine;
using UnityEngine.Audio;

public class XenoidFollow : MonoBehaviour
{
    [Header("Vision")]
    [SerializeField] private float viewDistance = 8f;
    [SerializeField] private LayerMask visionMask;
    [SerializeField] private Transform eyePoint;

    [Header("Movement")]
    [SerializeField] private Transform[] walkPoint;
    private int currentPoint = 0;

    [SerializeField] private float patrolWaitTime = 1f;
    private float patrolWaitCounter = 0f;
    private bool isWaitingAtPoint = false;



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

    [SerializeField] private float viewAngle = 90f;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool isPlayingSpotted = false; // za spotted prije chase stanja



    [Header("Attack")]
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackCooldown = 1.2f;
    private float lastAttackTime = -999f;

    private float lostPlayerTimer = 0f;
    [SerializeField] private float lostPlayerDelay = 1f;

    private bool damageTriggered = false;

    [Header("Sounds")]
    private AudioSource audioSourceAttack;
    public AudioClip[] attackSounds;

    public AudioSource audiosorceChase;
    public AudioClip chaseSound;



    private int lastAttackIndex = -1;

    private Collider2D playerCollider;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerCollider = player?.GetComponent<Collider2D>();
        enemyHealth = GetComponent<EnemyHealth>();
        rb = GetComponent<Rigidbody2D>();

        audioSourceAttack = GetComponent<AudioSource>();
        if (audioSourceAttack == null)
            audioSourceAttack = gameObject.AddComponent<AudioSource>();

        audioSourceAttack.playOnAwake = false;
        audioSourceAttack.loop = false;

      

    }

    void Update()
    {
        if (PlayerHealth.GameIsOver)
        {
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsWalking", false);
            rb.linearVelocity = Vector2.zero;
            return;
        }

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

        
        Vector2 playerPos = playerCollider != null
            ? playerCollider.ClosestPoint(eyePoint.position)
            : (Vector2)player.transform.position;

        Vector2 toPlayer = (playerPos - (Vector2)eyePoint.position).normalized;
        float distToPlayer = Vector2.Distance(eyePoint.position, playerPos);

        Vector2 forward = spriteRenderer.transform.localScale.x > 0 ? Vector2.left : Vector2.right;
        float angle = Vector2.Angle(forward, toPlayer);

        if (angle > viewAngle * 0.5f)
        {
            if (!hasSpotted)
            {
                lineOfSight = false;
                return;
            }
        }

        RaycastHit2D hit = Physics2D.Raycast(
            eyePoint.position,
            toPlayer,
            viewDistance,
            visionMask
        );

        bool rayHitPlayer = hit.collider != null && hit.collider.CompareTag("Player");

        if (hasSpotted)
            lineOfSight = true;
        else
            lineOfSight = rayHitPlayer;

        Debug.DrawRay(eyePoint.position, toPlayer * viewDistance, lineOfSight ? Color.green : Color.red);

        Vector2 leftBoundary = Quaternion.Euler(0, 0, viewAngle * 0.5f) * forward;
        Vector2 rightBoundary = Quaternion.Euler(0, 0, -viewAngle * 0.5f) * forward;
        Debug.DrawRay(eyePoint.position, leftBoundary * viewDistance, Color.yellow);
        Debug.DrawRay(eyePoint.position, rightBoundary * viewDistance, Color.yellow);

        if (lineOfSight && !hasSpotted)
        {
            animator.SetTrigger("Spotted");
            animator.SetBool("IsWalking", false);
            hasSpotted = true;
            isPlayingSpotted = true;
        }

        if (lineOfSight)
            lostPlayerTimer = 0f;
    }


    void HandleMovement()
    {
        if (isPlayingSpotted)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (HandleSpottedIdle())
            return;

        if (!hasSpotted && walkPoint != null && walkPoint.Length > 0)
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
        if (hasSpotted || walkPoint == null || walkPoint.Length == 0)
        {
            animator.SetBool("IsWalking", false);
            return;
        }

        Transform target = walkPoint[currentPoint];

        // WAIT LOGIKA
        if (isWaitingAtPoint)
        {
            animator.SetBool("IsWalking", false);
            patrolWaitCounter -= Time.fixedDeltaTime;

            if (patrolWaitCounter <= 0f)
            {
                isWaitingAtPoint = false;

                // uvijek kružno
                currentPoint = (currentPoint + 1) % walkPoint.Length;
            }

            return;
        }

        animator.SetBool("IsRunning", false);
        animator.SetBool("IsWalking", true);

        float direction = target.position.x - transform.position.x;

        if (Mathf.Abs(direction) > 0.05f)
        {
            SetFlip(direction < 0);
        }

        Vector2 newPos = Vector2.MoveTowards(
             rb.position,
             target.position,
             walkSpeed * Time.fixedDeltaTime
         ); 


        rb.MovePosition(newPos); 

        //Vector2 dir = (target.position - transform.position).normalized;
        //rb.linearVelocity = new Vector2(dir.x * walkSpeed, rb.linearVelocity.y); - ne valja

        if (Vector2.Distance(rb.position, target.position) < 0.05f)
        {
            isWaitingAtPoint = true;
            patrolWaitCounter = patrolWaitTime;
        }
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

        animator.ResetTrigger("Attack");
        animator.SetTrigger("Attack");

        if (attackSounds.Length > 0)
        {
            int index;
            do
            {
                index = Random.Range(0, attackSounds.Length);
            } while (attackSounds.Length > 1 && index == lastAttackIndex);

            audioSourceAttack.pitch = Random.Range(0.8f, 1.2f);
            audioSourceAttack.volume = Random.Range(0.8f, 1f);
            audioSourceAttack.PlayOneShot(attackSounds[index]);

            lastAttackIndex = index;
        }


        if (player != null) {
            player.GetComponent<PlayerHealth>()?.TakeDamage(attackDamage); 
        }
            

        lastAttackTime = Time.time;
    }

    void HandleChase()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", true);

        float direction = player.transform.position.x - transform.position.x;
        if (Mathf.Abs(direction) > 0.05f)
            SetFlip(direction < 0);

        Vector2 chasePos = Vector2.MoveTowards(rb.position, player.transform.position, runSpeed * Time.fixedDeltaTime);
        rb.MovePosition(chasePos);
    }

    public void EnterSpottedIdle()
    {
        animator.SetTrigger("ReturnToSpottedIdle");
        isInSpottedIdle = true;
        spottedTimer = 1f;
    }

    private void SetFlip(bool flip)
    {
        Vector3 scale = spriteRenderer.transform.localScale;
        float baseX = Mathf.Abs(scale.x);
        scale.x = flip ? baseX : -baseX;
        spriteRenderer.transform.localScale = scale;
    }

    public void EndSpottedAnimation()
    {
        isPlayingSpotted = false;
        lineOfSight = true;
    }

    public void PlayChaseSound()
    {
        audiosorceChase.PlayOneShot(chaseSound);
    }


}