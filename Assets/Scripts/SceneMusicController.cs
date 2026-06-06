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

    [SerializeField]
    private string[] silentScenes;

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
        
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {


        foreach (var silent in silentScenes)
        {
            if (silent == sceneName)
            {
                MusicManager.Instance.StopMusic();
                return;
            }
        }

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