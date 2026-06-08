using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smooth = 5f;
    float shakeDuration = 0f;
    float shakeMagnitude = 0.05f;
    Vector3 originalPos;
    private bool frozen = false; // DODAJ

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
        frozen = false; 
        var listener = GetComponent<AudioListener>();
        if (listener != null)
            listener.enabled = scene.name != "MiniCut";
        var cam = GetComponent<Camera>();
        if (cam != null)
            cam.enabled = scene.name != "MiniCut";
    }

    void Start()
    {
        originalPos = transform.position; 
    }

    void LateUpdate()
    {
        if (shakeDuration > 0)
        {
            
            Vector2 shakeOffset = Random.insideUnitCircle * shakeMagnitude;
            transform.position = new Vector3(
                originalPos.x + shakeOffset.x,
                originalPos.y + shakeOffset.y,
                originalPos.z
            );
            shakeDuration -= Time.deltaTime;
            if (shakeDuration <= 0)
                transform.position = originalPos; 
        }

        if (frozen || target == null) return; 

        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, target.position.x, smooth * Time.deltaTime);
        pos.y = Mathf.Lerp(pos.y, target.position.y, smooth * Time.deltaTime);
        transform.position = pos;
        originalPos = pos; 
    }

    public void Shake(float duration)
    {
        originalPos = transform.position; 
        shakeDuration = duration;
    }

    public void Freeze() {
        frozen = true; 
    }   
    public void Unfreeze()
    {
        frozen = false;
    }
}