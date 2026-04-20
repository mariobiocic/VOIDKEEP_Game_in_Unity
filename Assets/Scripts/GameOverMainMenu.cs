using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverMainMenu : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu";
    public ScreenFade fade;

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
        StartCoroutine(Transition());
    }

    private IEnumerator Transition()
    {
        Time.timeScale = 1f;

        if (fade != null)
        {
            float fadeTime = fade.BeginFade(1); 
            yield return new WaitForSeconds(fadeTime);
        }

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

        SceneManager.LoadScene(mainMenuSceneName);
    }
}