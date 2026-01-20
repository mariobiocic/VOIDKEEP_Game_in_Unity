using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  
    public float smooth = 5f;

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, target.position.x, smooth * Time.deltaTime);
        pos.y = Mathf.Lerp(pos.y, target.position.y, smooth * Time.deltaTime);
        transform.position = pos;
    }
}
