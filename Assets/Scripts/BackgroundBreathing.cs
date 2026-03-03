using UnityEngine;

public class BackgroundBreathing : MonoBehaviour
{
    public float scaleAmount = 0.01f;
    public float speed = 1f;

    private Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        float s = 1 + Mathf.Sin(Time.time * speed) * scaleAmount;
        transform.localScale = startScale * s;
    }
}
