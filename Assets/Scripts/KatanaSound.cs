using UnityEngine;

public class KatanaSound : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip[] swingSounds;
    public AudioClip[] hitSounds;

    

    [Header("Audio Settings")]
    public AudioSource audioSource;
    public AudioSource audioSorceHit;

    private int lastSwingIndex = -1;
    private int lastHitIndex = -1;

    void Start()
    {
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();


        }

        if (audioSorceHit == null)
        {
            audioSorceHit = gameObject.AddComponent<AudioSource>(); 
        }
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayRandomSwing();
        }
    }

    public void PlayRandomSwing()
    {
        if ( swingSounds.Length == 0 || audioSource == null)
            return;

        int index;
        do
        {
            index = Random.Range(0, swingSounds.Length);
        } while (swingSounds.Length > 1 && index == lastSwingIndex);

        
        audioSource.pitch = Random.Range(1.0f, 1.2f);
        audioSource.volume = Random.Range(1.0f, 1.2f);

        audioSource.PlayOneShot(swingSounds[index]);

        lastSwingIndex = index;
    }

    public void PlayRandomHit()
    {
        if (hitSounds.Length == 0 || audioSorceHit == null)
            return;

        int index;
        do
        {
            index = Random.Range(0, hitSounds.Length);
        } while (hitSounds.Length > 1 && index == lastHitIndex);

        audioSorceHit.pitch = Random.Range(0.9f, 1.1f);
        audioSorceHit.volume = Random.Range(0.9f, 1.1f);
        audioSorceHit.PlayOneShot(hitSounds[index]);

        lastHitIndex = index;
    }
}