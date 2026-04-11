using UnityEngine;
using System.Collections;
using Unity.VectorGraphics;
using UnityEngine.SceneManagement;

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

    public GameObject[] weapons;
    public GameObject staminaUI;
    public GameObject gameplayCanvas;

    public GameOverPrikaz gameOverPanel;
    public static bool GameIsOver = false;

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

        cam.Shake(0.002f);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (GameIsOver) return;
        GameIsOver = true;

        Debug.Log("Player dead");

        GetComponent<PlayerMovement2D>().enabled = false;
        GetComponent<PlayerStamina>().enabled = false;
        sr.enabled = false;

        staminaUI.SetActive(false);
        gameplayCanvas.SetActive(false);

        foreach (GameObject w in weapons)
            w.SetActive(false);

        gameOverPanel.ShowGameOver();
        Time.timeScale = 0f;
    }

    IEnumerator Flash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = originalColor;
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Healed! Current HP: " + currentHealth);
    }

    public void ResetPlayer()
    {
        currentHealth = maxHealth;

        GetComponent<PlayerMovement2D>().enabled = true;
        GetComponent<PlayerStamina>().enabled = true;

        sr.enabled = true;

        foreach (GameObject w in weapons)
            w.SetActive(true);

        staminaUI.SetActive(true);
        gameplayCanvas.SetActive(true);
    }
}