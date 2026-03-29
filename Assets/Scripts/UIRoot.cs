using UnityEngine;

public class UIRoot : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectsByType<UIRoot>(FindObjectsSortMode.None).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}