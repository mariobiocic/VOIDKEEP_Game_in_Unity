using UnityEngine;

public class UIRootGameOver : MonoBehaviour
{
    private void Start()
    {
        
        var panel = GetComponentInChildren<GameOverPrikaz>(true);
        if (panel != null)
            panel.HideGameOver();

        Time.timeScale = 1f; 
    }
}