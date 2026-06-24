using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject startButton;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
       // PlayerPrefs.DeleteAll(); 
        UpdateUI();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("intro");
    }

    public void ContinueGame()
    {
        int save = PlayerPrefs.GetInt("Save", -1);
        if (save == -1)
        {
            Debug.LogWarning("Nema sejva!");
            UpdateUI();
            return;
        }

        if (save < 0 || save >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("Invalid save data!");
            PlayerPrefs.DeleteKey("Save");
            UpdateUI();
            return;
        }

        
        PlayerPrefs.SetInt("LoadPlayerPosition", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(save);
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

    public void PlayAgain()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("intro");
    }

    void UpdateUI()
    {
        int save = PlayerPrefs.GetInt("Save", -1);
        bool hasSave = save != -1;
        continueButton.SetActive(hasSave);
        startButton.SetActive(!hasSave);
    }
}