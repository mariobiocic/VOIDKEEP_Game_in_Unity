using UnityEngine;

public class RockMover : MonoBehaviour
{
    public float fallSpeed = 3f;

    void Start()
    {
        enabled = false;
    }

    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }
}