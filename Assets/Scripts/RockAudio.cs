using UnityEngine;

public class RockAudio : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip rockRoll;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    
   

    public void RockRollSound()
    {
        audioSource.PlayOneShot(rockRoll);
    }
}
