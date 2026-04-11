using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveTerminal : MonoBehaviour, IInteractable
{
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
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        // Pronaði igraèa
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Sprema poziciju igraèa, tako da respawn bude na istom mjestu gdje je bio kod terminala
            PlayerPrefs.SetInt("Save", currentScene);
            PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
            PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);

            PlayerPrefs.Save();
            Debug.Log("Game saved at player's position near terminal!");
        }
        else
        {
            Debug.LogWarning("Player not found! Cannot save position.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            sr.sprite = highlightSprite;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            sr.sprite = normalSprite;
    }
}