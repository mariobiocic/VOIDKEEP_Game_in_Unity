using System.Collections;
using UnityEngine;

public class RockController : MonoBehaviour
{
    private Rocktrap[] spawners;

    [Header("Faza 1 - 50% HP")]
    [SerializeField] private float minActiveTime1 = 10f;
    [SerializeField] private float maxActiveTime1 = 16f;
    [SerializeField] private float minInactiveTime1 = 3f;
    [SerializeField] private float maxInactiveTime1 = 6f;
    [SerializeField] private int minSpawnersOn1 = 3;
    [SerializeField] private int maxSpawnersOn1 = 5;

    [Header("Faza 2 - 25% HP")]
    [SerializeField] private float minActiveTime2 = 14f;
    [SerializeField] private float maxActiveTime2 = 20f;
    [SerializeField] private float minInactiveTime2 = 1f;
    [SerializeField] private float maxInactiveTime2 = 3f;
    [SerializeField] private int minSpawnersOn2 = 5;
    [SerializeField] private int maxSpawnersOn2 = 7;

    [SerializeField] private BossHealth bossHealth;

    private bool phase1Triggered = false;
    private bool phase2Triggered = false;
    private bool phase1Started = false;
    private bool phase2Started = false;

    
    private float minActiveTime;
    private float maxActiveTime;
    private float minInactiveTime;
    private float maxInactiveTime;
    private int minSpawnersOn;
    private int maxSpawnersOn;

    void Awake()
    {
        spawners = GetComponentsInChildren<Rocktrap>();
        foreach (var s in spawners)
            s.enabled = false;
    }

    void Update()
    {
        if (bossHealth == null) return;

        float hp = bossHealth.CurrentHealth;
        float max = bossHealth.MaxHealth;

        if (!phase1Triggered && hp <= max * 0.5f)
        {
            phase1Triggered = true;
            SetParams(1);
            if (!phase1Started)
            {
                phase1Started = true;
                StartCoroutine(ControlSpawners());
            }
        }

        if (!phase2Triggered && hp <= max * 0.25f)
        {
            phase2Triggered = true;
            SetParams(2);
            
        }
    }

    void SetParams(int phase)
    {
        if (phase == 1)
        {
            minActiveTime = minActiveTime1;
            maxActiveTime = maxActiveTime1;
            minInactiveTime = minInactiveTime1;
            maxInactiveTime = maxInactiveTime1;
            minSpawnersOn = minSpawnersOn1;
            maxSpawnersOn = maxSpawnersOn1;
        }
        else
        {
            minActiveTime = minActiveTime2;
            maxActiveTime = maxActiveTime2;
            minInactiveTime = minInactiveTime2;
            maxInactiveTime = maxInactiveTime2;
            minSpawnersOn = minSpawnersOn2;
            maxSpawnersOn = maxSpawnersOn2;
        }
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