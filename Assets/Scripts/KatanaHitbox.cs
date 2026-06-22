using UnityEngine;

public class KatanaHitbox : MonoBehaviour
{
    public float knockbackForce = 6f;

    public KatanaSound katanaSound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyHealth enemy = collision.collider.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            enemy.TakeDamage(40);
            ApplyKnockback(collision);
            katanaSound?.PlayRandomHit();
        }

        BatHealth bat = collision.collider.GetComponentInParent<BatHealth>();
        if (bat != null)
        {
            bat.TakeDamage(40);
            ApplyKnockback(collision);
            katanaSound?.PlayRandomHit();
        }

        BossHealth boss = collision.collider.GetComponentInParent<BossHealth>();
        if (boss != null)
        {
            boss.TakeDamage(40);
            ApplyKnockback(collision);
            katanaSound?.PlayRandomHit();
        }
    }

    private void ApplyKnockback(Collision2D collision)
    {
        Rigidbody2D enemyRb = collision.collider.GetComponent<Rigidbody2D>();
        if (enemyRb != null)
        {
            Vector2 dir = (collision.transform.position - transform.position).normalized;
            enemyRb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
        }
    }
}