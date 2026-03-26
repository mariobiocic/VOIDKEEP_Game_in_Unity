using UnityEngine;

public class HealPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private int healAmount = 1;

    [Header("Visual")]
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite highlightSprite;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = normalSprite;
    }

    public void Interact()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            player.GetComponent<PlayerHealth>()?.Heal(healAmount);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sr.sprite = highlightSprite;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sr.sprite = normalSprite;
        }
    }
}