using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject continueButton;
    public GameObject startButton;
    
    private void Start()
    {
        if (PlayerPrefs.HasKey("Save"))
        {
            continueButton.SetActive(true);
            startButton.SetActive(false);
        }
        else
        {
            continueButton.SetActive(false);
            startButton.SetActive(true);
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
