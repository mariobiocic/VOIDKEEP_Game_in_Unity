using UnityEngine;

public class BulletVisual : MonoBehaviour
{
    private Vector2 target;
    public float speed = 35f;

    public void Init(Vector2 targetPos)
    {
        target = targetPos;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target) < 0.05f)
        {
            Destroy(gameObject);
        }
    }
}