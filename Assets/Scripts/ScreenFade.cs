using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SceneFadeDuration
{
    public string sceneName;
    public float duration = 0.5f;
}

public class ScreenFade : MonoBehaviour
{
    public static ScreenFade Instance;

    [SerializeField] private Image fadeImage;
    [SerializeField] private float defaultDuration = 0.5f;

    [Header("Per-Scene Duration")]
    [SerializeField] private List<SceneFadeDuration> sceneDurations = new List<SceneFadeDuration>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private float GetDurationForScene(string sceneName)
    {
        foreach (var entry in sceneDurations)
        {
            if (entry.sceneName == sceneName)
                return entry.duration;
        }
        return defaultDuration;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        float duration = GetDurationForScene(scene.name);
        StartCoroutine(Fade(1f, 0f, duration));
    }

    public void FadeAndLoad(string sceneName, float duration = -1)
    {
        float d = duration < 0 ? GetDurationForScene(sceneName) : duration;
        StartCoroutine(FadeOutAndLoad(sceneName, d));
    }

    private IEnumerator FadeOutAndLoad(string sceneName, float duration)
    {
        yield return StartCoroutine(Fade(0f, 1f, duration));
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, t / duration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = to;
        fadeImage.color = c;
    }
}