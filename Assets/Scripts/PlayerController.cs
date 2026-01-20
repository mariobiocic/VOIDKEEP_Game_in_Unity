using UnityEngine;
using UnityEngine.Audio;

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

    private BoxCollider2D flipZoneCollider;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        flipZoneCollider = GetComponent<BoxCollider2D>();

        if (flipZoneCollider == null)
        {
            Debug.LogWarning("Dodaj BoxCollider2D na player za flip zonu!");
        }
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
        // Unos kretanja
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;

        // Odabir brzine
        currentMoveSpeed = (Input.GetKey(KeyCode.LeftShift) && movement.magnitude > 0f) ? runSpeed : walkSpeed;

        // Postavljanje brzine u animator
        float speedValue = rb.linearVelocity.magnitude;
        animator.SetFloat("Speed", speedValue);
        animator.SetBool("HasGun", shotgun != null && shotgun.activeSelf);

        if (flipZoneCollider != null && crosshair != null)
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;

            Bounds bounds = flipZoneCollider.bounds;

            // Dead zona oko centra
            float deadZone = 0.2f; // prilagodi po potrebi

            if (mouseWorldPos.x < bounds.center.x - deadZone)
            {
                SetFlip(true);
            }
            else if (mouseWorldPos.x > bounds.center.x + deadZone)
            {
                SetFlip(false);
            }
        }
        else if (movement.x != 0)
        {
            SetFlip(movement.x < 0);
        }
    }

    private void SetFlip(bool flip)
    {
        Vector3 localScale = spriteRenderer.transform.localScale;
        float baseScaleX = Mathf.Abs(localScale.x);

        localScale.x = flip ? -baseScaleX : baseScaleX;
        spriteRenderer.transform.localScale = localScale;
    }


}
