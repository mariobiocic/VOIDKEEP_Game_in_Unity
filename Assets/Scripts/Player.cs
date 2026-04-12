using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private void Awake()
    {
        string scene = SceneManager.GetActiveScene().name;

       
        if (scene == "MainMenu")
        {
            Destroy(gameObject);
            return;
        }

        Player[] players = Object.FindObjectsByType<Player>(FindObjectsSortMode.None);

        if (players.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}