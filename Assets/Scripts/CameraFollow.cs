using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  
    public float smooth = 5f;

    float shakeDuration = 0f;
    float shakeMagnitude = 0.05f;

    Vector3 originalPos;


    void Start()
    {
        originalPos = transform.localPosition;
    }


    void LateUpdate()
    {

        if (shakeDuration > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime;
            return;
        }

        if (target == null)
            return;

        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, target.position.x, smooth * Time.deltaTime);
        pos.y = Mathf.Lerp(pos.y, target.position.y, smooth * Time.deltaTime);
        transform.position = pos;
    }

    public void Shake(float duration)
    {
        shakeDuration = duration;
    }

}
