using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsToMain : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
