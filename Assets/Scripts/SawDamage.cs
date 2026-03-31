using UnityEngine;

public class SawDamage : MonoBehaviour
{
    public int damage = 3;
    public float damageInterval = 1f; // koliko èesto pila oduzima damage
    public AudioClip sound;

    private AudioSource audioSource;
    private float lastDamageTime;
    private bool playerInside;
    private GameObject player;

    private void Start()
    {
        if (sound != null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.clip = sound;
            audioSource.loop = true; // stalno loop dok je pila aktivna
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;
            audioSource.Play();
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
}