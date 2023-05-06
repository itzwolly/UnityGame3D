using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEvents : MonoBehaviour
{
    public event EventHandler PlayClicked;
    public event EventHandler SettingsClicked;
    public event EventHandler SaveSettingsClicked;
    public event EventHandler QuitClicked;

    public void Play() {
        PlayClicked?.Invoke(this, EventArgs.Empty);
    }

    public void OpenSettings() {
        SettingsClicked?.Invoke(this, EventArgs.Empty);
    }

    public void SaveSettings() {
        // Save changes to disk
        PlayerPrefs.Save();
        // Alert listeners about the save
        SaveSettingsClicked?.Invoke(this, EventArgs.Empty);
    }

    public void Quit() {
        QuitClicked?.Invoke(this, EventArgs.Empty);    
    }
}
