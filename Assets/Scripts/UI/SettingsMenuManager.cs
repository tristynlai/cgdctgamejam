using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenuManager : MonoBehaviour
{
    public Slider MasterVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;
    public AudioMixer MainAudioMixer;

    public TMP_Dropdown TextSpeedDropdown;

    public void ChangeTextSpeed()
    {
        
    }

    public void ChangeMasterVolume()
    {
        MainAudioMixer.SetFloat("MasterVolume", MasterVolumeSlider.value);
    }

    public void ChangeMusicVolume()
    {
        MainAudioMixer.SetFloat("MusicVolume", MusicVolumeSlider.value);
    }

    public void ChangeSFXVolume()
    {
        MainAudioMixer.SetFloat("SFXVolume", SFXVolumeSlider.value);
    }

    public void ExitSettingsMenu()
    {
        gameObject.SetActive(false);
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
