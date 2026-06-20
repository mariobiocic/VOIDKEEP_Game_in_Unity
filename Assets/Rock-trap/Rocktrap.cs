using System.Collections;
using UnityEngine;

public class Rocktrap : MonoBehaviour
{
    public GameObject rockPrefab;
    public float delayBeforeFirstRock = 3f;
    public float minInterval = 3f;
    public float maxInterval = 7f;
    public float fallSpeed = 3f;

    private float timer = 0f;
    private float currentInterval;
    private bool firstSpawned = false;

    void Start()
    {
        currentInterval = Random.Range(minInterval, maxInterval);
    }

    void Update()
    {
        timer += Time.deltaTime;
        float waitTime = firstSpawned ? currentInterval : delayBeforeFirstRock;

        if (timer >= waitTime)
        {
            SpawnRock();
            timer = 0f;
            firstSpawned = true;
            currentInterval = Random.Range(minInterval, maxInterval);
        }
    }

    void SpawnRock()
    {
        GameObject rock = Instantiate(rockPrefab, transform.position, Quaternion.identity);

        SpriteRenderer sr = rock.GetComponent<SpriteRenderer>();
        Collider2D col = rock.GetComponent<Collider2D>();

        if (sr != null) sr.enabled = false;
        if (col != null) col.enabled = false;

        StartCoroutine(RevealRock(rock));
    }

    IEnumerator RevealRock(GameObject rock)
    {
        yield return new WaitForSeconds(1f);
        if (rock == null) yield break;

        SpriteRenderer sr = rock.GetComponent<SpriteRenderer>();
        Collider2D col = rock.GetComponent<Collider2D>();

        if (sr != null) sr.enabled = true;
        if (col != null) col.enabled = true;

        RockMover mover = rock.GetComponent<RockMover>();
        if (mover != null) mover.enabled = true;
    }
}