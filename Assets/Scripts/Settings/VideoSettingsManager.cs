using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VideoSettingsManager : MonoBehaviour
{
    [Header("UI")]
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] filteredResolutions;
    private int savedResolutionIndex = 0;

    void Start()
    {
        LoadResolutions();
        LoadSettings();

        // MAKE SURE EVENTS ARE CONNECTED
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggle);
        resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    // ------------------------------------------------------------
    // LOAD RESOLUTIONS (ONLY 16:9)
    // ------------------------------------------------------------
    void LoadResolutions()
    {
        List<Resolution> resList = new List<Resolution>();

        foreach (Resolution r in Screen.resolutions)
        {
            float aspect = (float)r.width / r.height;

            if (Mathf.Abs(aspect - (16f / 9f)) < 0.01f)
            {
                resList.Add(r);
            }
        }

        filteredResolutions = resList.ToArray();

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        for (int i = 0; i < filteredResolutions.Length; i++)
        {
            Resolution r = filteredResolutions[i];
            options.Add($"{r.width} x {r.height}");
        }

        resolutionDropdown.AddOptions(options);
    }

    // ------------------------------------------------------------
    // FULLSCREEN TOGGLE
    // ------------------------------------------------------------
    public void OnFullscreenToggle(bool isFullscreen)
    {
        // MUST call SetResolution when toggling fullscreen or Unity ignores it
        Resolution r = filteredResolutions[savedResolutionIndex];

        Screen.SetResolution(r.width, r.height,
            isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);

        PlayerPrefs.SetInt("fullscreen", isFullscreen ? 1 : 0);
    }

    // ------------------------------------------------------------
    // RESOLUTION CHANGE
    // ------------------------------------------------------------
    public void OnResolutionChanged(int index)
    {
        savedResolutionIndex = index;

        Resolution r = filteredResolutions[index];

        // ALWAYS include fullscreen mode or Unity ignores the resolution change
        bool full = fullscreenToggle.isOn;

        Screen.SetResolution(
            r.width,
            r.height,
            full ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed
        );

        PlayerPrefs.SetInt("resolutionIndex", index);

        SaveManager.Instance.data.resolutionIndex = index;
        SaveManager.Instance.data.fullscreen = fullscreenToggle.isOn;
        SaveManager.Instance.Save();

    }

    // ------------------------------------------------------------
    // LOAD SETTINGS
    // ------------------------------------------------------------
    void LoadSettings()
    {
        bool fullscreen = PlayerPrefs.GetInt("fullscreen", 1) == 1;
        fullscreenToggle.isOn = fullscreen;

        savedResolutionIndex = PlayerPrefs.GetInt("resolutionIndex", 0);
        savedResolutionIndex = Mathf.Clamp(savedResolutionIndex, 0, filteredResolutions.Length - 1);

        resolutionDropdown.value = savedResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Apply saved settings
        Resolution r = filteredResolutions[savedResolutionIndex];

        Screen.SetResolution(
            r.width,
            r.height,
            fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed
        );

        var d = SaveManager.Instance.data;
        resolutionDropdown.value = d.resolutionIndex;
        fullscreenToggle.isOn = d.fullscreen;
    }
}
