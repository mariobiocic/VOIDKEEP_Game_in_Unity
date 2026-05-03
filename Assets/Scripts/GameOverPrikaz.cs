using UnityEngine;

public class GameOverPrikaz : MonoBehaviour
{
    public float scaleAmount = 0.01f;
    public float speed = 1f;
    public RectTransform target;
    private Vector3 startScale;
    public GameOverMainMenu mainMenuButton;

    void Awake()
    {
        startScale = target.localScale;
        gameObject.SetActive(false);
    }

    void Update()
    {
        float s = 1 + Mathf.Sin(Time.unscaledTime * speed) * scaleAmount;
        target.localScale = startScale * s;
    }

    public void ShowGameOver()
    {
        gameObject.SetActive(true);
        if (mainMenuButton != null)
            mainMenuButton.Show();
        Time.timeScale = 0f;
    }

    public void HideGameOver()
    {
        gameObject.SetActive(false);
        target.localScale = startScale;

    }
}