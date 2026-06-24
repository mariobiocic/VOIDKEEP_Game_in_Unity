using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Movement")]
    public float moveSpeed = 1f;
    public float stopDistance = 7f;

    [Header("Shooting")]
    public float fireRate = 2f;
    public int bulletsPerShot = 4;
    private float fireTimer = 0f;

    [Header("Kick")]
    public float kickRange = 4f;
    public float kickCooldown = 3f;
    public float kickDamageRadius = 3f;
    private float kickTimer = 0f;
    private bool isKicking = false;

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
        if (isKicking) return;

        float dist = Vector2.Distance(transform.position, player.position);
        kickTimer += Time.deltaTime;

        if (dist <= kickRange && kickTimer >= kickCooldown)
        {
            StartCoroutine(KickCoroutine());
            return;
        }

        if (dist > stopDistance)
            MoveTowardsPlayer();
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

        Flip();
    }

    void MoveTowardsPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rb.linearVelocity = dir * moveSpeed;
        animator.SetBool("isMoving", true);
        Flip();
    }

    void Flip()
    {
        if (player.position.x < transform.position.x)
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1f);
        else
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, 1f);
    }

    IEnumerator KickCoroutine()
    {
        isKicking = true;
        kickTimer = 0f;

        rb.linearVelocity = Vector2.zero;
        animator.SetBool("isMoving", false);
        animator.SetTrigger("Kick");

        yield return new WaitForSeconds(0.8f);

        isKicking = false;
    }

    // Pozovati u animation eventu
    public void DealKickDamage()
    {
        if (player == null) return;
        if (Vector2.Distance(transform.position, player.position) <= kickDamageRadius)
            player.GetComponent<PlayerHealth>()?.TakeDamage(1);
    }

    void Shoot()
    {
        if (bulletPrefab == null || firePoint == null) return;
        StartCoroutine(ShootCoroutine());
    }

    IEnumerator ShootCoroutine()
    {
        animator.SetTrigger("Fire");
        yield return new WaitForSeconds(0.4f);

        Vector2 toPlayer = (player.position - firePoint.position).normalized;
        Vector2 facing = transform.localScale.x > 0 ? Vector2.left : Vector2.right;

        for (int i = 0; i < bulletsPerShot; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity, null);
            bullet.transform.localScale = Vector3.one;

            BossBullet bb = bullet.GetComponent<BossBullet>();
            if (bb == null) bb = bullet.GetComponentInChildren<BossBullet>();
            if (bb != null)
                bb.Launch(toPlayer, facing);
        }
    }
}