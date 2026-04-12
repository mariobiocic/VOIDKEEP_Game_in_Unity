using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMainMenu : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu";

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // VRATI vrijeme
        SceneManager.LoadScene(mainMenuSceneName);
    }
}