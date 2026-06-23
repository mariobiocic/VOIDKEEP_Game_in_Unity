using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 6f;
    public float angleSpread = 45f;   

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null) sr.enabled = false;
    }


    public void Launch(Vector2 direction)
    {
        if (sr != null) sr.enabled = true;

        
        float randomAngle = Random.Range(-angleSpread, angleSpread);
        Vector2 spreadDir = Quaternion.Euler(0, 0, randomAngle) * direction;

        
        float angle = Mathf.Atan2(spreadDir.y, spreadDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (rb != null)
            rb.linearVelocity = spreadDir * speed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>()?.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}