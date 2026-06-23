using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3750;
    private SpriteRenderer sr;
    private Color originalColor;
    private int currentHealth;
    private bool isDead = false;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    [Header("Health Bar")]
    public Slider healthBar;

    [Header("Animator")]
    public Animator animator;

    public bool IsDead => isDead;

    void Start()
    {
        currentHealth = maxHealth;

        sr = GetComponent<SpriteRenderer>();
        if (sr == null) sr = GetComponentInChildren<SpriteRenderer>();
        originalColor = sr.color;

        if (healthBar != null)
        {
            healthBar.minValue = 0;
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
            healthBar.value = currentHealth;

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

        if (healthBar != null)
            healthBar.gameObject.SetActive(false);

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