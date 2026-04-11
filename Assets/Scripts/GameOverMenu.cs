using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{

    public GameObject gameOverUI; // UI canvas za GameOver
    public GameObject player;
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Continue()
    {
        // Ako nema savea, idi u MainMenu
        if (!PlayerPrefs.HasKey("Save"))
        {
            Debug.LogWarning("No save data! Returning to Main Menu.");
            SceneManager.LoadScene("MainMenu");
            return;
        }

        // Reset health i UI
        PlayerHealth ph = player.GetComponent<PlayerHealth>();
        if (ph != null)
            ph.ResetPlayer();

     

        // Postavi player poziciju prema zadnjem save-u
        float x = PlayerPrefs.GetFloat("PlayerX", player.transform.position.x);
        float y = PlayerPrefs.GetFloat("PlayerY", player.transform.position.y);
        player.transform.position = new Vector3(x, y, player.transform.position.z);

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        // Sakrij GameOver UI
        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        Debug.Log($"Player respawned at saved position: {x}, {y}");
    }

}