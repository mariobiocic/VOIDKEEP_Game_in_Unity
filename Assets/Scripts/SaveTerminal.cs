using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveTerminal : MonoBehaviour, IInteractable
{
    [Header("Save")]

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

        PlayerPrefs.SetInt("Save", currentScene);
        PlayerPrefs.Save();

        Debug.Log("Game saved!");
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