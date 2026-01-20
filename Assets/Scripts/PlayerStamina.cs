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
    private float hideDelay = 0.5f;
    private float hideTimer = 0f;

    void Start()
    {
        currentStamina = maxStamina;
        staminaUIGroup.SetActive(false);
    }

    void Update()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        // Trošenje i regeneracija
        if (isSprinting && currentStamina > 0)
        {
            currentStamina -= drainRate * Time.deltaTime;
            hideTimer = hideDelay;
            staminaUIGroup.SetActive(true);
        }
        else
        {
            currentStamina += regenRate * Time.deltaTime;
            hideTimer -= Time.deltaTime;
            if (hideTimer <= 0f)
                staminaUIGroup.SetActive(false);
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        staminaFill.fillAmount = currentStamina / maxStamina;
    }
}