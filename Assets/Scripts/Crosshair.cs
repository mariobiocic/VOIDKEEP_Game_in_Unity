using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectsByType<Crosshair>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject); // sprijeèi duplikate
            return;
        }

        DontDestroyOnLoad(gameObject); // ostaje kroz scene
    }
}