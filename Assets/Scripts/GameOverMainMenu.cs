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
        Time.timeScale = 1f;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Camera cam = player.GetComponentInChildren<Camera>();
            if (cam != null)
            {
                cam.transform.SetParent(null);
                Destroy(cam.gameObject);
            }
            Destroy(player);
        }

        if (ScreenFade.Instance != null)
            ScreenFade.Instance.FadeAndLoad(mainMenuSceneName);
        else
            SceneManager.LoadScene(mainMenuSceneName);
    }
}