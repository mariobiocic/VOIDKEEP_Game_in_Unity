using UnityEngine;
using UnityEngine.SceneManagement;

public class UIRoot : MonoBehaviour
{
    private void Awake()
    {
        string scene = SceneManager.GetActiveScene().name;

        if (scene == "MainMenu")
        {
            Destroy(gameObject);
            return;
        }

        if (FindObjectsByType<UIRoot>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
