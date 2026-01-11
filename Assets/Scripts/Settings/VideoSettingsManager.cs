using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VideoSettingsManager : MonoBehaviour
{
    [Header("UI")]
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;

    // Stored resolutions (only 16:9)
    private Resolution[] filteredResolutions;

    private int currentResolutionIndex = 0;

    private const string KEY_FULLSCREEN = "fullscreen";
    private const string KEY_RESOLUTION = "resolutionIndex";

    void Start()
    {
        LoadAvailableResolutions();
        LoadSettings();
    }

    // -----------------------------------------------------------
    // LOAD RESOLUTIONS
    // -----------------------------------------------------------
    void LoadAvailableResolutions()
    {
        List<Resolution> resList = new List<Resolution>();

        foreach (var res in Screen.resolutions)
        {
            float aspect = (float)res.width / res.height;

            // Only 16:9 ratios
            if (Mathf.Abs(aspect - (16f / 9f)) < 0.01f)
            {
                resList.Add(res);
            }
        }

        filteredResolutions = resList.ToArray();

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        for (int i = 0; i < filteredResolutions.Length; i++)
        {
            Resolution r = filteredResolutions[i];
            string option = $"{r.width} x {r.height}";
            options.Add(option);

            if (r.width == Screen.currentResolution.width &&
                r.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // -----------------------------------------------------------
    // FULLSCREEN
    // -----------------------------------------------------------
    public void OnToggleFullscreen(bool isFullscreen)
    {
        Screen.fullScreenMode = isFullscreen
            ? FullScreenMode.FullScreenWindow
            : FullScreenMode.Windowed;

        PlayerPrefs.SetInt(KEY_FULLSCREEN, isFullscreen ? 1 : 0);
    }

    // -----------------------------------------------------------
    // RESOLUTION CHANGE
    // -----------------------------------------------------------
    public void OnResolutionChanged(int index)
    {
        Resolution r = filteredResolutions[index];

        Screen.SetResolution(r.width, r.height, Screen.fullScreen);

        PlayerPrefs.SetInt(KEY_RESOLUTION, index);
    }

    // -----------------------------------------------------------
    // LOAD SAVED SETTINGS
    // -----------------------------------------------------------
    void LoadSettings()
    {
        // Fullscreen
        bool isFullscreen = PlayerPrefs.GetInt(KEY_FULLSCREEN, 1) == 1;
        fullscreenToggle.isOn = isFullscreen;

        Screen.fullScreenMode = isFullscreen
            ? FullScreenMode.FullScreenWindow
            : FullScreenMode.Windowed;

        // Resolution
        int savedIndex = PlayerPrefs.GetInt(KEY_RESOLUTION, currentResolutionIndex);
        savedIndex = Mathf.Clamp(savedIndex, 0, filteredResolutions.Length - 1);

        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();

        Resolution r = filteredResolutions[savedIndex];
        Screen.SetResolution(r.width, r.height, Screen.fullScreen);
    }
}
