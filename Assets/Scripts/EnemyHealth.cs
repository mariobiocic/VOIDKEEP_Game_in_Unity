using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 75;   
    [SerializeField] private Animator animator;
    private int currentHealth;
    private bool isTakingDamage = false;

    private bool isDead = false;

    [Header("Sounds")]
    private AudioSource audioSource;
    public AudioClip[] damageSounds;
    public AudioClip[] deathSounds;

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

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }


    public void TakeDamage(int damage)
    {
        if (isDead) return;


      
        currentHealth -= damage;
        isTakingDamage = true;
        animator.SetTrigger("TakeDamage");

        if (damageSounds.Length > 0)
        {
            int index = Random.Range(0, damageSounds.Length);
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(damageSounds[index]);
        }

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
        if (isDead) return;

        isDead = true;

        // pokreni death animaciju
        animator.SetTrigger("Death");

        // zaustavi AI
        XenoidFollow follow = GetComponent<XenoidFollow>();
        if (follow != null)
            follow.enabled = false;

    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
