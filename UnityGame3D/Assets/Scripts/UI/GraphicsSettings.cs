using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GraphicsSettings : MonoBehaviour
{
    private const string RESOLUTION_KEY = "resolution";
    private const string SCREEN_MODE_KEY = "fullscreenmode";
    private const string QUALITY_LEVEL_KEY = "qualitylevel";
    private const string VSYNC_KEY = "vsync";

    [SerializeField] private Dropdown _resolutionDropDown;
    [SerializeField] private Dropdown _screenDropdown;
    [SerializeField] private Dropdown _qualityDropdown;
    [SerializeField] private Toggle _vsyncToggle;

    private void Start() {
        initScreen();
        initQuality();
        initVsync();
        initResolution();
    }

    private void initResolution() {
        _resolutionDropDown.ClearOptions();
        Resolution res = getResolution();
        _resolutionDropDown.AddResolutionOptions(res.width, res.height);
    }

    private void initScreen() {
        _screenDropdown.ClearOptions();
        _screenDropdown.AddScreenOptions(getFullScreenMode());
    }

    private void initQuality() {
        _qualityDropdown.ClearOptions();
        _qualityDropdown.AddQualityOptions(getQualityLevel());
    }

    private void initVsync() {
        _vsyncToggle.isOn = getVsync() > 0;
    }

    private void setResolution(int width, int height) {
        Screen.SetResolution(width, height, getFullScreenMode());
    }

    private void setScreenMode(int screenModeIndex) {
        Screen.fullScreenMode = (FullScreenMode)screenModeIndex;
    }

    private void setQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    private void setVsync(int count) {
        QualitySettings.vSyncCount = count;
    }

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = Screen.resolutions.Where(o => o.refreshRate == Screen.resolutions.Max(o => o.refreshRate)).ToList()[resolutionIndex];
        setResolution(resolution.width, resolution.height);
        PlayerPrefs.SetString(RESOLUTION_KEY, resolution.width + "x" + resolution.height);
    }

    public void SetScreenMode(int screenModeIndex) {
        setScreenMode(screenModeIndex);
        PlayerPrefs.SetInt(SCREEN_MODE_KEY, screenModeIndex);
    }

    public void SetQuality(int qualityIndex) {
        setQuality(qualityIndex);
        PlayerPrefs.SetInt(QUALITY_LEVEL_KEY, qualityIndex);
    }

    public void SetVsync(bool enabled) {
        int count = (enabled) ? 1 : 0;
        setVsync(count);
        PlayerPrefs.SetInt(VSYNC_KEY, count);
    }

    public void LoadSettings() {
        setScreenMode((int) getFullScreenMode());
        setQuality(getQualityLevel());
        setVsync(getVsync());
        Resolution res = getResolution();
        setResolution(res.width, res.height);
    }

    private Resolution getResolution() {
        Resolution res = new Resolution();

        if (PlayerPrefs.HasKey(RESOLUTION_KEY)) {
            string[] resolution = PlayerPrefs.GetString(RESOLUTION_KEY).Split('x');
            res.width = Convert.ToInt32(resolution[0]);
            res.height = Convert.ToInt32(resolution[1]);
            res.refreshRate = Screen.currentResolution.refreshRate;
        } else {
            res = Screen.currentResolution;
        }

        return res;
    }

    private FullScreenMode getFullScreenMode() {
        if (PlayerPrefs.HasKey(SCREEN_MODE_KEY)) {
            return (FullScreenMode)PlayerPrefs.GetInt(SCREEN_MODE_KEY);
        }
        return Screen.fullScreenMode;
    }

    private int getQualityLevel() {
        if (PlayerPrefs.HasKey(QUALITY_LEVEL_KEY)) {
            return PlayerPrefs.GetInt(QUALITY_LEVEL_KEY);
        }
        return QualitySettings.GetQualityLevel();
    }

    private int getVsync() {
        if (PlayerPrefs.HasKey(VSYNC_KEY)) {
            return PlayerPrefs.GetInt(VSYNC_KEY);
        }
        return QualitySettings.vSyncCount;
    }
}
