using UnityEngine;
using UnityEngine.Video;

public class PlayIntroVideo : MonoBehaviour
{
    public VideoClip videoClip;
    private VideoPlayer videoPlayer;
    private AudioSource audioSource;

    void Start()
    {
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        audioSource = gameObject.AddComponent<AudioSource>();

        videoPlayer.clip = videoClip;
        videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
        videoPlayer.targetCamera = Camera.main;
        videoPlayer.targetCameraAlpha = 1.0f;

        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        videoPlayer.EnableAudioTrack(0, true);
        videoPlayer.SetTargetAudioSource(0, audioSource);

        videoPlayer.playOnAwake = false;
        audioSource.playOnAwake = false;
        videoPlayer.waitForFirstFrame = true;
        videoPlayer.skipOnDrop = true;

        videoPlayer.Prepare();
        videoPlayer.prepareCompleted += OnPrepared;

        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = Color.black;
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 473; // prilagodi visini videa
    }

    void OnPrepared(VideoPlayer vp)
    {
        vp.Play();
        audioSource.Play();
    }
}