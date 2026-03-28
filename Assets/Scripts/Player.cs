using UnityEngine;

public class Player : MonoBehaviour
{
    private void Awake()
    {
        // Zamjena za FindObjectsOfType<Player>()
        Player[] players = Object.FindObjectsByType<Player>(FindObjectsSortMode.None);

        if (players.Length > 1)
        {
            Destroy(gameObject); // izbriši duplikate
            return;
        }

        DontDestroyOnLoad(gameObject); // Player + child kamera ostaju kroz scene
    }
}