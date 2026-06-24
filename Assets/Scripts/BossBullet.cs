using UnityEngine;

public class BossBullet : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 18f;
    public float angleSpread = 40f;  

    [Header("Direction Bias")]
    [Range(0f, 1f)]
    public float playerBias = 0.45f; 

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

    // Poziva BossAI — prima smjer prema playeru i facing smjer bossa
    public void Launch(Vector2 toPlayer, Vector2 bossFacing)
    {
        if (sr != null) sr.enabled = true;

        // Miješaj smjer prema playeru s facing smjerom bossa
        Vector2 baseDir = Vector2.Lerp(bossFacing, toPlayer, playerBias).normalized;

        // Random otklon
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

        if (other.CompareTag("Obsticle") || other.CompareTag("Katana"))
            Destroy(gameObject);
    }
}