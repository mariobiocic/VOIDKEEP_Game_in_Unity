using UnityEngine;

public class MusicMainMenu : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip menuMusic;

    [Header("Fade Settings")]
    public float fadeInTime = 2f;
    public float fadeOutTime = 2f;

    void Start()
    {
        audioSource.clip = menuMusic;
        audioSource.volume = 0f;      // poèinje tiho
        audioSource.loop = true;      // ponavlja se
        audioSource.Play();

        StartCoroutine(FadeIn());
    }

    private System.Collections.IEnumerator FadeIn()
    {
        float t = 0f;

        while (t < fadeInTime)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, t / fadeInTime);
            yield return null;
        }
    }

    public void FadeOutAndStop()
    {
        StartCoroutine(FadeOut());
    }

    private System.Collections.IEnumerator FadeOut()
    {
        float startVolume = audioSource.volume;
        float t = 0f;

        while (t < fadeOutTime)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeOutTime);
            yield return null;
        }

        audioSource.Stop();
    }
}