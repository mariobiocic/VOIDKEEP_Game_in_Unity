using UnityEngine;

public class BulletTracer : MonoBehaviour
{
    public float tracerLifetime = 0.2f;
    public float forwardOffset = 0.2f;

    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ShowTracer(Vector3 firePointPos, Vector2 dir, Quaternion rotation)
    {
        gameObject.SetActive(true);

        transform.position = firePointPos + (Vector3)(dir * forwardOffset);
        transform.rotation = rotation;

        CancelInvoke();
        Invoke(nameof(HideTracer), tracerLifetime);
    }

    void HideTracer()
    {
        gameObject.SetActive(false);
    }
}
