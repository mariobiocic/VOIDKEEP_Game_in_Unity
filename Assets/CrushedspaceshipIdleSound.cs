using UnityEngine;

public class CrushedSpaceshipIdleSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip electroSound;

    private void Start()
    {
        Electro_zvuk();
    }

    public void Electro_zvuk()
    {
        if (audioSource != null && electroSound != null)
        {
            audioSource.clip = electroSound;
            audioSource.Play();
        }
    }
}