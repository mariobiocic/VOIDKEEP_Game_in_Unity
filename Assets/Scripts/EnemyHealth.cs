using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 75;   
    [SerializeField] private Animator animator;
    private int currentHealth;
    private bool isTakingDamage = false;

    private bool isDead = false;

    public bool IsDead
    {
        get { return isDead; }
    }

    public bool IsTakingDamage
    {
        get { return isTakingDamage; }
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        isTakingDamage = true;
        animator.SetTrigger("TakeDamage");

        GetComponent<XenoidFollow>()?.EnterSpottedIdle();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void EndTakeDamage()
    {
        isTakingDamage = false;
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Death"); 
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
