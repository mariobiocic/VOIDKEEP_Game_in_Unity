using UnityEngine;
using System.Collections;

public class RockCollision2D : MonoBehaviour
{
    private int damage = 1;
    private GameObject player;
    private SpriteRenderer sr;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sr = GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {


        if (other.CompareTag("PlayerFeet"))
        {
            if (player != null)
            {
                player.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            }
            Destroy(gameObject);
        }

        if (other.CompareTag("Obsticle"))
        {
            //Debug.Log("Rock collided with " + other.gameObject.name);
            Destroy(gameObject);
        }
    }

    IEnumerator FadeAndDestroy()
    {
        GetComponent<Collider2D>().enabled = false;

        float duration = 0.4f;
        float timer = 0f;
        Color startColor = sr.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / duration);
            sr.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}