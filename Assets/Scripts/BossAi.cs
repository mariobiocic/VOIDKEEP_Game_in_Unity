using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Movement")]
    public float moveSpeed = 1.5f;
    public float stopDistance = 4f;      // koliko blizu stane prije pucanja

    [Header("Shooting")]
    public float fireRate = 2f;          // sekunde izmeðu hitaca
    private float fireTimer = 0f;

    [Header("Animator")]
    public Animator animator;

    private Rigidbody2D rb;
    private BossHealth health;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<BossHealth>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (health.IsDead) return;
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist > stopDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isMoving", false);

            fireTimer += Time.deltaTime;
            if (fireTimer >= fireRate)
            {
                fireTimer = 0f;
                Shoot();
            }
        }

        
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1f);
        else
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1f);
    }

    void MoveTowardsPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;
        animator.SetBool("isMoving", true);
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;

        animator.SetTrigger("Fire");

        Vector2 dir = (player.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.Euler(0, 0, angle));

        Rigidbody2D bRb = bullet.GetComponent<Rigidbody2D>();
        if (bRb != null)
            bRb.linearVelocity = dir * 6f;
    }
}