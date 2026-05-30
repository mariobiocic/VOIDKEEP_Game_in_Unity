using UnityEngine;

public class BatSounds : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Sounds")]
    public AudioClip flySound;
    public AudioClip attackSound;
    public AudioClip dieSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayFly()
    {
        if (flySound == null) return;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(flySound);
    }

    public void PlayAttack()
    {
        if (attackSound == null) return;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(attackSound);
    }

    public void PlayDie()
    {
        if (dieSound == null) return;
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(dieSound);
    }
}