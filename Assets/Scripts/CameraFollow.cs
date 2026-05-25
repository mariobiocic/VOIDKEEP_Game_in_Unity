using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  
    public float smooth = 5f;

    float shakeDuration = 0f;
    float shakeMagnitude = 0.05f;

    Vector3 originalPos;



    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var listener = GetComponent<AudioListener>();
        if (listener != null)
            listener.enabled = scene.name != "MiniCut";

        var cam = GetComponent<Camera>();
        if (cam != null)
            cam.enabled = scene.name != "MiniCut";
    }

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
