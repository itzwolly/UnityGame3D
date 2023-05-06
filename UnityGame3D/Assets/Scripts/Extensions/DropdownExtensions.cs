using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public static class DropdownExtensions
{
    public static void AddResolutionOptions(this Dropdown dropdown, int startWidth, int startHeight) {
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        int index = 0;
        foreach (Resolution res in Screen.resolutions.Where(o => o.refreshRate == Screen.resolutions.Max(o => o.refreshRate))) {
            string option = res.width + "x" + res.height;
            options.Add(option);

            // Get current resolution index
            if (res.width == startWidth && res.height == startHeight) {
                currentResolutionIndex = index;
            }

            index++;
        }

        dropdown.AddOptions(options);
        dropdown.value = currentResolutionIndex;
        dropdown.RefreshShownValue();
    }

    public static void AddScreenOptions(this Dropdown dropdown, FullScreenMode startMode) {
        List<string> options = new List<string>();
        int currentFullScreenModeIndex = 0;
        FullScreenMode[] fullScreenModes = (FullScreenMode[])Enum.GetValues(typeof(FullScreenMode));

        for (int i = 0; i < fullScreenModes.Length; i++) {
            FullScreenMode mode = fullScreenModes[i];
            options.Add(mode.ToString());

            // Get current fullscreen mode index
            if (mode == startMode) {
                currentFullScreenModeIndex = i;
            }
        }

        dropdown.AddOptions(options);
        dropdown.value = currentFullScreenModeIndex;
        dropdown.RefreshShownValue();
    }

    public static void AddQualityOptions(this Dropdown dropdown, int currentQualityLevel) {
        List<string> options = new List<string>();

        for (int i = 0; i < QualitySettings.names.Length; i++) {
            string qualityName = QualitySettings.names[i];
            options.Add(qualityName);
        }

        dropdown.AddOptions(options);
        dropdown.value = currentQualityLevel;
        dropdown.RefreshShownValue();
    }
}
