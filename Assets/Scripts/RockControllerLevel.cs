using System.Collections;
using UnityEngine;

public class RockControllerLevel : MonoBehaviour
{
    private Rocktrap[] spawners;

    [Header("Postavke")]
    [SerializeField] private float minActiveTime = 10f;
    [SerializeField] private float maxActiveTime = 16f;
    [SerializeField] private float minInactiveTime = 3f;
    [SerializeField] private float maxInactiveTime = 6f;
    [SerializeField] private int minSpawnersOn = 3;
    [SerializeField] private int maxSpawnersOn = 5;

    void Awake()
    {
        spawners = GetComponentsInChildren<Rocktrap>();
        foreach (var s in spawners)
            s.enabled = false;

        StartCoroutine(ControlSpawners());
    }

    IEnumerator ControlSpawners()
    {
        while (true)
        {
            int count = Random.Range(minSpawnersOn, Mathf.Min(maxSpawnersOn, spawners.Length) + 1);

            foreach (var s in spawners)
                s.enabled = false;

            Rocktrap[] shuffled = (Rocktrap[])spawners.Clone();
            for (int i = shuffled.Length - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
            }

            for (int i = 0; i < count; i++)
                shuffled[i].enabled = true;

            yield return new WaitForSeconds(Random.Range(minActiveTime, maxActiveTime));

            foreach (var s in spawners)
                s.enabled = false;

            yield return new WaitForSeconds(Random.Range(minInactiveTime, maxInactiveTime));
        }
    }
}