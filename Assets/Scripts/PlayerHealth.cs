using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public CameraFollow cam;

    public SpriteRenderer sr;
    private Color originalColor;

    [Header("Sounds")]
    public AudioSource audioSourcedamage;
    public AudioClip[] damageSounds;

    int lastDamageIndex = -1;


    void Start()
    {
        currentHealth = maxHealth;
        originalColor = sr.color;
        audioSourcedamage = gameObject.AddComponent<AudioSource>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        StartCoroutine(Flash());

        if (damageSounds.Length > 0)
        {
            int index;
            do
            {
                index = Random.Range(0, damageSounds.Length);
            } while (damageSounds.Length > 1 && index == lastDamageIndex);

            audioSourcedamage.pitch = Random.Range(0.8f, 1.2f);
            audioSourcedamage.volume = Random.Range(0.8f, 1f);
            audioSourcedamage.PlayOneShot(damageSounds[index]);

            lastDamageIndex = index;
        }

        cam.Shake(0.002f); // shake camera on damage

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player dead");
        // disable movement, show game over, respawn...
    }

    IEnumerator Flash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }
}