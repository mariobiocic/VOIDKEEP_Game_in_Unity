using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio")]
    public Slider musicSlider;

    [Header("Resolution")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        musicSlider.value = savedVolume;
        musicSlider.onValueChanged.AddListener(SetMusicVolume);

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        int currentIndex = 0;
        var options = new System.Collections.Generic.List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            options.Add(resolutions[i].width + " x " + resolutions[i].height);
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
                currentIndex = i;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionIndex", currentIndex);
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetMusicVolume(float volume)
    {
        if (MusicManager.Instance != null)
            MusicManager.Instance.SetVolume(volume);
    }

    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResolutionIndex", index);
    }

    public void xMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}