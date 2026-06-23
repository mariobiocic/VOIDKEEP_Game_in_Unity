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

    // Finish Him stanje
    private bool isInFinishHim = false;
    private int finishHimHits = 0;
    private const int hitsToKill = 10;
    private bool finishHimHitPlaying = false;

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

        // --- Finish Him faza ---
        if (isInFinishHim)
        {
            if (finishHimHitPlaying) return; // èekaj da animacija završi
            finishHimHits++;
            StartCoroutine(Flash());

            if (finishHimHits >= hitsToKill)
            {
                StartCoroutine(FinalDeath());
            }
            else
            {
                StartCoroutine(FinishHimHitAnim());
            }
            return;
        }

        // --- Normalna faza ---
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
            healthBar.value = currentHealth;

        StartCoroutine(Flash());

        if (currentHealth <= 0)
            StartCoroutine(EnterCloseToDeath());
    }

    IEnumerator EnterCloseToDeath()
    {
        isDead = true; // zaustavi normalni damage
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

        // Zaustavi AI
        var ai = GetComponent<BossAI>();
        if (ai != null) ai.enabled = false;

        if (animator != null)
            animator.SetTrigger("CloseToDeath");

        
        yield return new WaitForSeconds(1.5f);

        isDead = false; 
        isInFinishHim = true;
        finishHimHits = 0;

        if (animator != null)
            animator.SetTrigger("FinishHim");
    }

   
    IEnumerator FinishHimHitAnim()
    {
        finishHimHitPlaying = true;

        if (animator != null)
            animator.SetBool("isDamage", true);

        
        yield return new WaitForSeconds(0.4f);

        if (animator != null)
            animator.SetBool("isDamage", false);

        finishHimHitPlaying = false;
    }

    IEnumerator FinalDeath()
    {
        isInFinishHim = false;
        isDead = true;

        if (animator != null)
            animator.SetTrigger("Death");

        
        yield return new WaitForSeconds(0.5f);

      
        float duration = 1.5f;
        float elapsed = 0f;
        Color c = sr.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            sr.color = new Color(c.r, c.g, c.b, alpha);
            yield return null;
        }

        gameObject.SetActive(false);
    }

    IEnumerator Flash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        if (!isDead || isInFinishHim)
            sr.color = originalColor;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}