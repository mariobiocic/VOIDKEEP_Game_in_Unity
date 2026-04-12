using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraDestroy : MonoBehaviour
{
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
            return;

        Camera cam = GetComponent<Camera>();

        if (cam != null && cam.CompareTag("MainCamera"))
        {
            Destroy(gameObject);
        }
    }
}
