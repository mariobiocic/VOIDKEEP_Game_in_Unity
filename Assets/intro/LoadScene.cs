using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string sceneName;

    void Start()
    {
        
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene(sceneName);
    }
}