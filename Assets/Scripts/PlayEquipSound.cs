using UnityEngine;

public class PlayEquipSound : MonoBehaviour
{
    public AudioClip sound;
    public AudioSource audioSourceEquip;

    void Start()
    {
        audioSourceEquip = GetComponent<AudioSource>();

        if (audioSourceEquip == null)
        {
            audioSourceEquip = gameObject.AddComponent<AudioSource>();
        }
    }

    public void Play()
    {
        audioSourceEquip.PlayOneShot(sound);
    }
}
