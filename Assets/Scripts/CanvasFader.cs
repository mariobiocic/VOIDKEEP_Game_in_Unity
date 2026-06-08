using System.Collections;
using UnityEngine;

public class CanvasFader : MonoBehaviour
{
    [Header("Kašnjenje prije pojave")]
    public float delayBeforeFadeIn = 2f;

    [Header("Trajanje animacija")]
    public float fadeInDuration = 1f;
    public float visibleDuration = 3f;
    public float fadeOutDuration = 1f;

    [Header("Opcije")]
    public bool playOnStart = true;
    public bool disableAfterFadeOut = true;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        if (playOnStart)
            Play();
    }

    public void Play()
    {
        StopAllCoroutines();
        canvasGroup.alpha = 0f;
        StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        yield return new WaitForSeconds(delayBeforeFadeIn);

        yield return StartCoroutine(Fade(0f, 1f, fadeInDuration));

        yield return new WaitForSeconds(visibleDuration);

        yield return StartCoroutine(Fade(1f, 0f, fadeOutDuration));

        if (disableAfterFadeOut)
            gameObject.SetActive(false);
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = to;
    }
}