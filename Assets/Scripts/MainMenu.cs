using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject continueButton;
    public GameObject startButton;
    
    private void Start()
    {
        //zakomentirati PlayerPrefs.DeleteAll(); nakon prvog pokretanja igre da se ne brišu save podaci svaki put
        PlayerPrefs.DeleteAll();
        UpdateUI();
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("intro");
    }

    public void ContinueGame()
    {
        int save = PlayerPrefs.GetInt("Save", -1);

        if (save >= 0 && save < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(save);
        }
        else
        {
            Debug.LogWarning("Invalid save data!");
            PlayerPrefs.DeleteKey("Save");
            UpdateUI();
        }
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


    void UpdateUI()
    {
        int save = PlayerPrefs.GetInt("Save", -1);

        bool hasSave = save != -1;

        continueButton.SetActive(hasSave);
        startButton.SetActive(!hasSave);
    }
}
