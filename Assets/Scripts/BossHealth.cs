using UnityEngine;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 1250;
    private SpriteRenderer sr;
    private Color originalColor;
    private int currentHealth;
    private bool isDead = false;

    public bool IsDead => isDead;

    void Start()
    {
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        StartCoroutine(Flash());
        if (currentHealth <= 0)
            Die();
    }

    IEnumerator Flash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        if (!isDead)
            sr.color = originalColor;
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        sr.color = originalColor;

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Static;
        }
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}