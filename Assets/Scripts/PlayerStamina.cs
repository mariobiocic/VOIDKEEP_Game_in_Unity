using UnityEngine;
using UnityEngine.UI;

public class PlayerStamina : MonoBehaviour
{
    [Header("Stamina Settings")]
    public float maxStamina = 100f;
    public float regenRate = 10f;
    public float drainRate = 25f;

    [Header("UI References")]
    public Image staminaFill; // Fill image unutar Slidera
    public GameObject staminaUIGroup; // Parent ikone + bara

    private float currentStamina;
    public float CurrentStamina => currentStamina;

    public float sprintRecoveryThreshold = 30f;
    public bool SprintUnlocked => currentStamina >= sprintRecoveryThreshold;

    void Start()
    {
        currentStamina = maxStamina;
        staminaUIGroup.SetActive(true);
    }

    void Update()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        // Trošenje i regeneracija
        if (isSprinting && currentStamina > 0)
        {
            currentStamina -= drainRate * Time.deltaTime;
        }
        else
        {
            currentStamina += regenRate * Time.deltaTime;
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        staminaFill.fillAmount = currentStamina / maxStamina;
    }
}