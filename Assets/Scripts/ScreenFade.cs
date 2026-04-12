using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    public float fadeSpeed = 2f;
    private Image img;

    void Awake()
    {
        img = GetComponent<Image>();
    }

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        Color c = img.color;
        c.a = 1f;
        img.color = c;

        while (img.color.a > 0f)
        {
            c.a -= Time.unscaledDeltaTime * fadeSpeed;
            img.color = c;
            yield return null;
        }

        c.a = 0f;
        img.color = c;
    }

    public IEnumerator FadeOut()
    {
        Color c = img.color;

        while (img.color.a < 1f)
        {
            c.a += Time.unscaledDeltaTime * fadeSpeed;
            img.color = c;
            yield return null;
        }

        c.a = 1f;
        img.color = c;
    }
}