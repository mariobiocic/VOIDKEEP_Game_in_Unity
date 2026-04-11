using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement2D : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform crosshair;
    [SerializeField] private GameObject shotgun;

    private Rigidbody2D rb;
    private Vector2 movement;
    private float currentMoveSpeed;
    private PlayerStamina stamina;
    private bool sprintAllowed = true;

    private BoxCollider2D flipZoneCollider;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        flipZoneCollider = GetComponent<BoxCollider2D>();
        stamina = GetComponent<PlayerStamina>();

        if (flipZoneCollider == null)
            Debug.LogWarning("Dodaj BoxCollider2D na player za flip zonu!");
    }

    void Start()
    {
        
    }

    void Update()
    {
        Kretanje();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * currentMoveSpeed;
    }

    void Kretanje()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        OdabirBrzine();

        float speedValue = movement.magnitude * currentMoveSpeed;
        bool isRunning = currentMoveSpeed == runSpeed && movement.magnitude > 0f;

        if (animator != null)
        {
            animator.SetBool("IsRunning", isRunning);
            animator.SetFloat("Speed", speedValue);
            animator.SetBool("HasGun", shotgun != null && shotgun.activeSelf);
        }

        HandleFlip();
    }

    private void HandleFlip()
    {
        if (flipZoneCollider != null && crosshair != null)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Bounds bounds = flipZoneCollider.bounds;
            float deadZone = 0.2f;

            if (mouseWorldPos.x < bounds.center.x - deadZone)
                SetFlip(true);
            else if (mouseWorldPos.x > bounds.center.x + deadZone)
                SetFlip(false);
        }
        else if (movement.x != 0)
        {
            SetFlip(movement.x < 0);
        }
    }

    private void SetFlip(bool flip)
    {
        if (spriteRenderer == null) return;

        Vector3 localScale = spriteRenderer.transform.localScale;
        float baseScaleX = Mathf.Abs(localScale.x);
        localScale.x = flip ? -baseScaleX : baseScaleX;
        spriteRenderer.transform.localScale = localScale;
    }

    private void OdabirBrzine()
    {
        bool wantsToSprint = Input.GetKey(KeyCode.LeftShift);
        bool hasStamina = stamina != null && stamina.CurrentStamina > 0f;

        if (!hasStamina)
            sprintAllowed = false;

        if (stamina != null && stamina.SprintUnlocked)
            sprintAllowed = true;

        if (wantsToSprint && !sprintAllowed)
            currentMoveSpeed = walkSpeed;
        else if (wantsToSprint && hasStamina && movement.magnitude > 0f)
            currentMoveSpeed = runSpeed;
        else
            currentMoveSpeed = walkSpeed;
    }

    void LoadPlayerPosition()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");

            transform.position = new Vector3(x, y, transform.position.z);
            rb.linearVelocity = Vector2.zero;

            Debug.Log($"Player loaded at position: {x}, {y}");
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ne uèitavaj poziciju u GameOver sceni
        if (scene.name != "GameOverScene")
        {
            LoadPlayerPosition();
        }
    }
}