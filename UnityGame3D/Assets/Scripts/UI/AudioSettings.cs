using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;

    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _interfaceSlider;

    private void Start() {
        setMasterVolume(_masterSlider.value);
        setMusicVolume(_musicSlider.value);
        setSFXVolume(_sfxSlider.value);
        setInterfaceVolume(_interfaceSlider.value);
    }

    public void setMasterVolume(float volume) {
        _audioMixer.SetFloat("MasterVolume", volume);
    }

    public void setMusicVolume(float volume) {
        _audioMixer.SetFloat("MusicVolume", volume);
    }

    public void setSFXVolume(float volume) {
        _audioMixer.SetFloat("SFXVolume", volume);
    }

    public void setInterfaceVolume(float volume) {
        _audioMixer.SetFloat("InterfaceVolume", volume);
    }
}
