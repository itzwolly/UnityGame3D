using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsSettings : MonoBehaviour
{
    [SerializeField] private Dropdown _resolutionDropDown;
    [SerializeField] private Dropdown _screenDropdown;
    [SerializeField] private Dropdown _qualityDropdown;

    private void Start() {
        initResolution();
        initScreen();
        initQuality();
    }

    private void initResolution() {
        _resolutionDropDown.ClearOptions();
        _resolutionDropDown.AddResolutionOptions();
    }

    private void initScreen() {
        _screenDropdown.ClearOptions();
        _screenDropdown.AddScreenOptions();
    }

    private void initQuality() {
        _qualityDropdown.ClearOptions();
        _qualityDropdown.AddQualityOptions();
    }

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
    }

    public void SetScreenMode(int screenModeIndex) {
        Screen.fullScreenMode = (FullScreenMode)screenModeIndex;
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetVsync(bool enabled) {
        QualitySettings.vSyncCount = (enabled) ? 1 : 0;
    }
}
