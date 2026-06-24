using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorBoss : MonoBehaviour, IInteractable
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private float interactRange = 1.5f;
    [SerializeField] private BossHealth bossHealth;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.transform.position, transform.position);

        if (distance <= interactRange && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    public void Interact()
    {
        if (bossHealth != null && bossHealth.CurrentHealth > 0)
        {
            Debug.Log("Ne možeš proæi! Boss još nije poražen.");
            return;
        }

        if (string.IsNullOrEmpty(sceneToLoad)) return;

        if (ScreenFade.Instance != null)
            ScreenFade.Instance.FadeAndLoad(sceneToLoad);
        else
            SceneManager.LoadScene(sceneToLoad);
    }
}