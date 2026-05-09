using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMusicController : MonoBehaviour
{
    [System.Serializable]
    public struct SceneMusicEntry
    {
        public string sceneName;
        public string trackName;
    }

    [SerializeField]
    private SceneMusicEntry[] sceneMusicMap;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        foreach (var entry in sceneMusicMap)
        {
            if (entry.sceneName == sceneName)
            {
                MusicManager.Instance.PlayMusic(entry.trackName);
                return;
            }
        }

        Debug.LogWarning($"Nema pjesme za scenu: {sceneName}");
    }
}