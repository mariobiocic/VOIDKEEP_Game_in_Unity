using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class Torch_sound : MonoBehaviour
{
    public int damage = 3;
    public float damageInterval = 1f;
    public AudioClip sound;

    private AudioSource audioSource;
    void Start()
    {
        if (sound != null)
        {
            audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = sound;
            audioSource.loop = true; // stalno loop 
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 1f;
            audioSource.Play();
        }

     
    }
}
