using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SliderFadeIn : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 4f;

    private Image[] images;

    private void Awake()
    {
        images = GetComponentsInChildren<Image>();
    }

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, time / fadeDuration);

            foreach (Image img in images)
            {
                Color c = img.color;
                c.a = alpha;
                img.color = c;
            }

            yield return null;
        }
    }
}