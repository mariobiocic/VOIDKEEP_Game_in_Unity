using UnityEngine;

public class EnemyWalkSound : MonoBehaviour
{
    public AudioClip[] walkRunSounds;
    private int lastIndex = -1;

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

        AudioSource.PlayClipAtPoint(walkRunSounds[index], transform.position, 0.5f);
    }
}