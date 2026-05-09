using UnityEngine;
using UnityEngine.SceneManagement;

public class UIRootGameOver : MonoBehaviour
{
    private void Awake()
    {
        
        var all = FindObjectsByType<UIRootGameOver>(FindObjectsSortMode.None);
        foreach (var other in all)
        {
            if (other != this && other.gameObject.name == gameObject.name)
            {
                Destroy(gameObject);
                return;
            }
        }

        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu" || scene.name=="Credits" || scene.name=="Settings")
        {
            
            gameObject.SetActive(false);
        }
        else
        {
            
            gameObject.SetActive(true);
        }
    }
}