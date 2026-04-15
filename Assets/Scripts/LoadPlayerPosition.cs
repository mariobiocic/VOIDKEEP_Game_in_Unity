using UnityEngine;

public class LoadPlayerPosition : MonoBehaviour
{
    void Start()
    {
        int shouldLoad = PlayerPrefs.GetInt("LoadPlayerPosition", 0);

        if (shouldLoad == 1)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null)
            {
                float x = PlayerPrefs.GetFloat("PlayerX", player.transform.position.x);
                float y = PlayerPrefs.GetFloat("PlayerY", player.transform.position.y);

                player.transform.position = new Vector2(x, y);
            }

            // Resetiraj flag da se ne ponavlja
            PlayerPrefs.SetInt("LoadPlayerPosition", 0);
            PlayerPrefs.Save();
        }
    }
}
