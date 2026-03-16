using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject continueButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        if (!PlayerPrefs.HasKey("Save"))
        {
            continueButton.SetActive(false);
      
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("intro");
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("Save"));
    }

    public void GoToSettingsMenu()
    {
        SceneManager.LoadScene("SettingsMenu");
    }
    public void GoToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
