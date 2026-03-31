using UnityEngine;

public class SawDamage : MonoBehaviour
{
    public int damage = 3;



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerFeet"))
        {
            GameObject player = other.transform.parent.gameObject;

            PlayerHealth health = player.GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
        }
    }
}
