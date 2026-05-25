using UnityEngine;
using UnityEngine.SceneManagement;

public class Crosshairhide : MonoBehaviour
{
    [System.Serializable]
    public struct SceneCrosshairEntry
    {
        public string sceneName;
        public bool hideCrosshair;
    }

    [SerializeField]
    private SceneCrosshairEntry[] sceneCrosshairMap;

    private Camera cam;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (var entry in sceneCrosshairMap)
        {
            if (entry.sceneName == scene.name)
            {
                if (entry.hideCrosshair)
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
                return;
            }
        }
    }

    private void Update()
    {
        if (!gameObject.activeSelf || cam == null) return;
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        transform.position = mousePos;
    }
}