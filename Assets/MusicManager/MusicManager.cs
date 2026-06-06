using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField] private MusicLibrary musicLibrary;
    [SerializeField] private AudioSource musicSource;

    private float targetVolume = 1f; 

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Uèitaj spremljenu glasnoæu pri startu
        targetVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicSource.volume = targetVolume;
    }

    public void SetVolume(float volume)
    {
        targetVolume = volume;
        musicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        AudioClip nextClip = musicLibrary.GetClipFromName(trackName);
        if (musicSource.clip == nextClip && musicSource.isPlaying) return;
        StopAllCoroutines();
        StartCoroutine(AnimateMusicCrossfade(nextClip, fadeDuration));
    }

    IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration = 0.5f)
    {
        float percent = 0;
        float startVolume = musicSource.volume;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(startVolume, 0, percent); // fade iz trenutnog
            yield return null;
        }
        musicSource.clip = nextTrack;
        musicSource.Play();
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(0, targetVolume, percent); // fade do targetVolume
            yield return null;
        }
    }

    public void StopMusic(float fadeDuration = 0.5f)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateMusicFadeOut(fadeDuration));
    }

    IEnumerator AnimateMusicFadeOut(float fadeDuration)
    {
        float percent = 0;
        float startVolume = musicSource.volume;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, percent);
            yield return null;
        }
        musicSource.Stop();
        musicSource.clip = null;
    }
}