using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverContinue : MonoBehaviour
{
    public void ContinueFromGameOver()
    {
        
        Time.timeScale = 1f;

        
        int savedScene = PlayerPrefs.GetInt("Save", -1);

        if (savedScene == -1)
        {
            Debug.LogWarning("Nema sejva! Continue ne može nastaviti.");
            return;
        }

       
        PlayerPrefs.SetInt("LoadPlayerPosition", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(savedScene);
    }
}
