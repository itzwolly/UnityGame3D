using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class DropdownExtensions
{
    public static void AddResolutionOptions(this Dropdown dropdown) {
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        Resolution currentResolution = Screen.currentResolution;

        for (int i = 0; i < Screen.resolutions.Length; i++) {
            Resolution res = Screen.resolutions[i];
            string option = res.width + "x" + res.height;
            options.Add(option);

            // Get current resolution index
            if (res.width == currentResolution.width && res.height == currentResolution.height) {
                currentResolutionIndex = i;
            }
        }

        dropdown.AddOptions(options);
        dropdown.value = currentResolutionIndex;
        dropdown.RefreshShownValue();
    }

    public static void AddScreenOptions(this Dropdown dropdown) {
        List<string> options = new List<string>();
        int currentFullScreenModeIndex = 0;
        FullScreenMode currentFullScreenMode = Screen.fullScreenMode;
        FullScreenMode[] fullScreenModes = (FullScreenMode[])Enum.GetValues(typeof(FullScreenMode));

        for (int i = 0; i < fullScreenModes.Length; i++) {
            FullScreenMode mode = fullScreenModes[i];
            options.Add(mode.ToString());

            // Get current fullscreen mode index
            if (mode == currentFullScreenMode) {
                currentFullScreenModeIndex = i;
            }
        }

        dropdown.AddOptions(options);
        dropdown.value = currentFullScreenModeIndex;
        dropdown.RefreshShownValue();
    }

    public static void AddQualityOptions(this Dropdown dropdown) {
        List<string> options = new List<string>();
        int currentQualityLevel = QualitySettings.GetQualityLevel();

        for (int i = 0; i < QualitySettings.names.Length; i++) {
            string qualityName = QualitySettings.names[i];
            options.Add(qualityName);
        }

        dropdown.AddOptions(options);
        dropdown.value = currentQualityLevel;
        dropdown.RefreshShownValue();
    }
}
