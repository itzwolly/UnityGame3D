using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] _panels;

    private int _currentPanelIndex = 0;

    public event EventHandler<int> PageChanged;

    private void Start() {
        showCurrentPanel();
    }

    public void NextPage() {
        _currentPanelIndex = (_currentPanelIndex + 1) % _panels.Length;

        showCurrentPanel();

        PageChanged?.Invoke(this, _currentPanelIndex);
    }

    public void PreviousPage() {
        _currentPanelIndex = (_currentPanelIndex - 1) % _panels.Length;

        showCurrentPanel();

        PageChanged?.Invoke(this, _currentPanelIndex);
    }

    private void showCurrentPanel() {
        foreach (GameObject panel in _panels) {
            panel.SetActive(false);
        }
        CurrentPanel.SetActive(true);
    }

    public GameObject CurrentPanel {
        get => _panels[_currentPanelIndex];
    }
}
