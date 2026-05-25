using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float regenRate = 10f;
    public float drainRate = 25f;

    [Header("UI References")]
    public Image staminaFill;
    public GameObject staminaUIGroup;

    private float currentStamina;
    public float CurrentStamina => currentStamina;
    public float sprintRecoveryThreshold = 30f;
    public bool SprintUnlocked => currentStamina >= sprintRecoveryThreshold;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        staminaFill = GameObject.Find("Fill")?.GetComponent<Image>();

        staminaUIGroup = GameObject.Find("Stamina");

        if (staminaUIGroup != null)
            staminaUIGroup.SetActive(true);
    }

    void Start()
    {
        currentStamina = maxStamina;

        if (staminaUIGroup != null)
            staminaUIGroup.SetActive(true);
    }

    void Update()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        if (isSprinting && currentStamina > 0)
            currentStamina -= drainRate * Time.deltaTime;
        else
            currentStamina += regenRate * Time.deltaTime;

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        // Null provjera — neæe pucati ako je UI uništen
        if (staminaFill != null)
            staminaFill.fillAmount = currentStamina / maxStamina;
    }
}