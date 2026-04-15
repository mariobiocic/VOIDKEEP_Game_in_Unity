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

        
    }
}