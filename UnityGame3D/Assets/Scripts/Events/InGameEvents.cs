using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameEvents : MonoBehaviour
{
    public event EventHandler ResumeGameClicked;
    public event EventHandler SettingsClicked;
    public event EventHandler SaveSettingsClicked;
    public event EventHandler QuitToMenuClicked;

    public void Resume() {
        ResumeGameClicked?.Invoke(this, EventArgs.Empty);
    }

    public void Settings() {
        SettingsClicked?.Invoke(this, EventArgs.Empty);
    }

    public void SaveSettings() {
        SaveSettingsClicked?.Invoke(this, EventArgs.Empty);
    }

    public void QuitToMenu() {
        QuitToMenuClicked?.Invoke(this, EventArgs.Empty);
    }
}
