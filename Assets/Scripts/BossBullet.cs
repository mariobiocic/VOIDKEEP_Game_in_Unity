using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 14f;
    public float angleSpread = 50f;  // max otklon od smjera prema playeru

    [Header("Direction Bias")]
    [Range(0f, 1f)]
    public float playerBias = 0.75f; // 0 = potpuno random, 1 = uvijek prema playeru

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Vector2 launchDir;
    private bool launched = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sr.enabled = false;
    }

    
    public void Launch(Vector2 toPlayer, Vector2 bossFacing)
    {
        if (sr != null) sr.enabled = true;

        
        Vector2 baseDir = Vector2.Lerp(bossFacing, toPlayer, playerBias).normalized;

        
        float randomAngle = Random.Range(-angleSpread, angleSpread);
        launchDir = (Quaternion.Euler(0, 0, randomAngle) * baseDir).normalized;

        float angle = Mathf.Atan2(launchDir.y, launchDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        launched = true;
    }

    void FixedUpdate()
    {
        if (!launched) return;
        rb.linearVelocity = launchDir * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>()?.TakeDamage(1);
            Destroy(gameObject);
        }

        if (other.CompareTag("Obsticle"))
            Destroy(gameObject);
    }
}