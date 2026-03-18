using UnityEngine;

public class DamageTile : MonoBehaviour
{
    public int damage = 1;
    public float damageInterval = 2f;

    private float lastDamageTime;
    private bool playerInside;
    private GameObject player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerFeet"))
        {
            player = other.transform.parent.gameObject;
            playerInside = true;

            // odmah skini damage kad uðe
            PlayerHealth health = player.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
                lastDamageTime = Time.time;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerFeet"))
        {
            playerInside = false;
            player = null;
        }
    }

    private void Update()
    {
        if (playerInside && player != null)
        {
            if (Time.time >= lastDamageTime + damageInterval)
            {
                PlayerHealth health = player.GetComponent<PlayerHealth>();
                if (health != null)
                {
                    health.TakeDamage(damage);
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}