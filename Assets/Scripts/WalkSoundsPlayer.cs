using UnityEngine;

public class WalkSoundsPlayer : MonoBehaviour
{
    public AudioSource audioSourceWalk;
    public AudioClip[] walkRunSounds;
    private int lastIndex = -1;

    void Start()
    {
        audioSourceWalk = gameObject.AddComponent<AudioSource>();
    }


    public void PlayRandomSound()
    {
        if (walkRunSounds.Length == 0)
            return;

        int index;
        do
        {
            index = Random.Range(0, walkRunSounds.Length);
        }
        while (index == lastIndex);

        lastIndex = index;

        audioSourceWalk.clip = walkRunSounds[index];
        audioSourceWalk.pitch = Random.Range(0.95f, 1.05f);
        audioSourceWalk.Play();

    }
}
