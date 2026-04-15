using UnityEngine;
using UnityEngine.SceneManagement;


public class Crosshair : MonoBehaviour
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