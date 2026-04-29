using UnityEngine;

public class LoadPlayerPosition : MonoBehaviour
{
    void Start()
    {
       
        var gameOverPanel = FindFirstObjectByType<GameOverPrikaz>();
        if (gameOverPanel != null)
            gameOverPanel.HideGameOver();

        
        Time.timeScale = 1f;

       
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            var playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.ResetPlayer();

            
            if (PlayerPrefs.GetInt("LoadPlayerPosition", 0) == 1)
            {
                float x = PlayerPrefs.GetFloat("PlayerX", player.transform.position.x);
                float y = PlayerPrefs.GetFloat("PlayerY", player.transform.position.y);
                player.transform.position = new Vector2(x, y);

                PlayerPrefs.SetInt("LoadPlayerPosition", 0);
                PlayerPrefs.Save();
            }
        }
    }
}