using UnityEngine;

public class BatHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 25;
    [SerializeField] private Animator animator;

    private int currentHealth;
    private bool isDead = false;

    public bool IsDead => isDead;

    void Start()
    {
        currentHealth = maxHealth;

        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;
        animator.SetTrigger("die");
        GetComponent<BatAI>()?.Die();
    }

    public void DestroySelf()
    {
        Destroy(gameObject); 
    }
}