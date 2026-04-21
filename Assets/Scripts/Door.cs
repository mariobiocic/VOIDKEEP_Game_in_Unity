using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private float interactRange = 1.5f;

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
        if (string.IsNullOrEmpty(sceneToLoad)) return;

        if (ScreenFade.Instance != null)
            ScreenFade.Instance.FadeAndLoad(sceneToLoad);
        else
            SceneManager.LoadScene(sceneToLoad);
    }
}