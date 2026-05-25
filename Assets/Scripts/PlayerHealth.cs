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

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;

        audioSourcedamage = GetComponent<AudioSource>();
        if (audioSourcedamage == null)
            audioSourcedamage = gameObject.AddComponent<AudioSource>();
    }

    void Start() {
        currentHealth = maxHealth;
        PlayerHealth.GameIsOver = false;

        if (gameOverPanel == null)
            gameOverPanel = GameOverPrikaz.Instance;
    }

    public void TakeDamage(int damage)
    {
        if (GameIsOver) return;
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        StartCoroutine(Flash());

        if (damageSounds.Length > 0)
        {
            int index;
            do { index = Random.Range(0, damageSounds.Length); }
            while (damageSounds.Length > 1 && index == lastDamageIndex);
            audioSourcedamage.pitch = Random.Range(0.8f, 1.2f);
            audioSourcedamage.volume = Random.Range(0.8f, 1f);
            audioSourcedamage.PlayOneShot(damageSounds[index]);
            lastDamageIndex = index;
        }

        cam.Shake(0.002f);
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (GameIsOver) return;
        GameIsOver = true;

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Static;
        }

        GetComponent<PlayerMovement2D>().enabled = false;
        GetComponent<PlayerStamina>().enabled = false;
        staminaUI.SetActive(false);
        gameplayCanvas.SetActive(false);

        // Sakrij sva oruzja pri smrti
        foreach (GameObject w in weapons)
            w.SetActive(false);

        if (gameOverPanel != null)
            gameOverPanel.ShowGameOver();
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
    }

    public void ResetPlayer()
    {
        GameIsOver = false;
        currentHealth = maxHealth;

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        sr.enabled = true;
        sr.color = originalColor;

        GetComponent<PlayerMovement2D>().enabled = true;
        GetComponent<PlayerStamina>().enabled = true;

        

        staminaUI.SetActive(true);
        gameplayCanvas.SetActive(true);

        if (gameOverPanel != null)
            gameOverPanel.HideGameOver();
    }
}