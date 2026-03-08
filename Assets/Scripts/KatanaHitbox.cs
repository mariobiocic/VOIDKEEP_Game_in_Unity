using UnityEngine;

public class KatanaHitbox : MonoBehaviour
{
    public float knockbackForce = 6f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyHealth enemy = collision.collider.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            Debug.Log("Macheta pogodila: " + collision.collider.name);
            enemy.TakeDamage(40);

            Rigidbody2D enemyRb = collision.collider.GetComponent<Rigidbody2D>();

            if (enemyRb != null)
            {
                Vector2 dir = (collision.transform.position - transform.position).normalized;
                enemyRb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
            }
        }
    }
}