using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Continue()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("Save"));
    }
}
