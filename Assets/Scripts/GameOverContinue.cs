using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverContinue : MonoBehaviour
{
    public GameOverPrikaz gameOverPanel;

    public void ContinueFromGameOver()
    {
        int savedScene = PlayerPrefs.GetInt("Save", -1);
        if (savedScene == -1)
        {
            Debug.LogWarning("Nema sejva! Continue ne može nastaviti.");
            return;
        }

        if (gameOverPanel != null)
            gameOverPanel.HideGameOver();

        Time.timeScale = 1f;

        PlayerPrefs.SetInt("LoadPlayerPosition", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(savedScene);
    }
}