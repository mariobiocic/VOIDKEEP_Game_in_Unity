using UnityEngine;
using UnityEngine.SceneManagement;

public class ElixirPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public void Interact()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}