using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    private const string MASTER_VOLUME_PREF_KEY = "mastervolume";
    private const string MUSIC_VOLUME_PREF_KEY = "musicvolume";
    private const string SFX_VOLUME_PREF_KEY = "sfxvolume";
    private const string INTERFACE_VOLUME_PREF_KEY = "interfacevolume";

    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _masterSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private Slider _interfaceSlider;

    private void Start() {
        _masterSlider.value = getMasterVolume();
        _musicSlider.value = getMusicVolume();
        _sfxSlider.value = getSFXVolume();
        _interfaceSlider.value = getInterfaceVolume();
    }

    private void setMasterVolume(float volume) {
        _audioMixer.SetFloat("MasterVolume", volume);
    }

    private void setMusicVolume(float volume) {
        _audioMixer.SetFloat("MusicVolume", volume);
    }

    private void setSFXVolume(float volume) {
        _audioMixer.SetFloat("SFXVolume", volume);
    }

    private void setInterfaceVolume(float volume) {
        _audioMixer.SetFloat("InterfaceVolume", volume);
    }

    public void SetMasterVolume(float volume) {
        setMasterVolume(volume);
        PlayerPrefs.SetFloat(MASTER_VOLUME_PREF_KEY, volume);
    }

    public void SetMusicVolume(float volume) {
        setMusicVolume(volume);
        PlayerPrefs.SetFloat(MUSIC_VOLUME_PREF_KEY, volume);
    }

    public void SetSFXVolume(float volume) {
        setSFXVolume(volume);
        PlayerPrefs.SetFloat(SFX_VOLUME_PREF_KEY, volume);
    }

    public void SetInterfaceVolume(float volume) {
        setInterfaceVolume(volume);
        PlayerPrefs.SetFloat(INTERFACE_VOLUME_PREF_KEY, volume);
    }

    public void LoadSettings() {
        setMasterVolume(getMasterVolume());
        setMusicVolume(getMusicVolume());
        setSFXVolume(getSFXVolume());
        setInterfaceVolume(getInterfaceVolume());
    }

    private float getMasterVolume() {
        if (PlayerPrefs.HasKey(MASTER_VOLUME_PREF_KEY)) {
            return PlayerPrefs.GetFloat(MASTER_VOLUME_PREF_KEY);
        }
        return _masterSlider.value;
    }

    private float getMusicVolume() {
        if (PlayerPrefs.HasKey(MUSIC_VOLUME_PREF_KEY)) {
            return PlayerPrefs.GetFloat(MUSIC_VOLUME_PREF_KEY);
        }
        return _musicSlider.value;
    }

    private float getSFXVolume() {
        if (PlayerPrefs.HasKey(SFX_VOLUME_PREF_KEY)) {
            return PlayerPrefs.GetFloat(SFX_VOLUME_PREF_KEY);
        }
        return _sfxSlider.value;
    }

    private float getInterfaceVolume() {
        if (PlayerPrefs.HasKey(INTERFACE_VOLUME_PREF_KEY)) {
            return PlayerPrefs.GetFloat(INTERFACE_VOLUME_PREF_KEY);
        }
        return _interfaceSlider.value;
    }
}
