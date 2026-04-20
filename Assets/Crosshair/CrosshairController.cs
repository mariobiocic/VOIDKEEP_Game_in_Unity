using UnityEngine;
using UnityEngine.SceneManagement;

public class CrosshairController : MonoBehaviour
{
    private Camera cam;

    void Awake()
    {
        DontDestroyOnLoad(gameObject); 
        SceneManager.sceneLoaded += OnSceneLoaded; 
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            
            gameObject.SetActive(false);
        }
        else
        {
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;

            
            gameObject.SetActive(true);

            cam = Camera.main;
        }
    }

    void Update()
    {
        if (!gameObject.activeSelf || cam == null) return;

        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        transform.position = mousePos;
    }
}
