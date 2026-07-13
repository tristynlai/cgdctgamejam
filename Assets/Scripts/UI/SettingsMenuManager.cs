using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using Yarn.Unity;

public class SettingsMenuManager : MonoBehaviour
{
    [Header("Main Contents")]
    public GameObject SettingsContents;
    public GameObject CreditsContents;

    public Button SettingsButton;
    public Button CreditsButton;

    public Sprite SettingsSelected;
    public Sprite SettingsUnselected;

    public Sprite CreditsSelected;
    public Sprite CreditsUnselected;


    [Header("Volume Sliders")]
    public Slider MasterVolumeSlider;
    public Slider MusicVolumeSlider;
    public Slider SFXVolumeSlider;
    public AudioMixer MainAudioMixer;

    [Header("Text Speed Buttons")]
    public TMP_Dropdown TextSpeedDropdown;

    public Button TSp1Button;
    public Button TSp15Button;
    public Button TSp2Button;

    public Sprite Selected1XButton;
    public Sprite Unselected1XButton;

    public Sprite Selected15XButton;
    public Sprite Unselected15XButton;

    public Sprite Selected2XButton;
    public Sprite Unselected2XButton;

    [Header("Text Size Buttons")]
    public Button SmallButton;
    public Button MidButton;
    public Button BigButton;

    public Sprite SelectedSmallButton;
    public Sprite UnselectedSmallButton;

    public Sprite SelectedMidButton;
    public Sprite UnselectedMidButton;

    public Sprite SelectedBigButton;
    public Sprite UnselectedBigButton;

    private int TextSizeIndex = 1;
    private int TextSpeedIndex = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreditsContents.SetActive(false);
        SettingsContents.SetActive(true);

        ShowSettingsContents();

        LoadSettings();
    }

    public void ShowSettingsContents()
    {
        CreditsContents.SetActive(false);
        SettingsContents.SetActive(true);

        SettingsButton.GetComponent<Image>().sprite = SettingsSelected;
        CreditsButton.GetComponent<Image>().sprite = CreditsUnselected;
    }

    public void ShowCreditsContents()
    {
        RevertUnsavedChanges();
        
        CreditsContents.SetActive(true);
        SettingsContents.SetActive(false);

        SettingsButton.GetComponent<Image>().sprite = SettingsUnselected;
        CreditsButton.GetComponent<Image>().sprite = CreditsSelected;
    }

    public void TextSizeSmallPressed()
    {
        SmallButton.GetComponent<Image>().sprite = SelectedSmallButton;

        MidButton.GetComponent<Image>().sprite = UnselectedMidButton;
        BigButton.GetComponent<Image>().sprite = UnselectedBigButton;

        TextSizeIndex = 0;
    }
    public void TextSizeMidPressed()
    {
        MidButton.GetComponent<Image>().sprite = SelectedMidButton;

        SmallButton.GetComponent<Image>().sprite = UnselectedSmallButton;
        BigButton.GetComponent<Image>().sprite = UnselectedBigButton;

        TextSizeIndex = 1;
    }
    public void TextSizeBigPressed()
    {
        BigButton.GetComponent<Image>().sprite = SelectedBigButton;

        MidButton.GetComponent<Image>().sprite = UnselectedMidButton;
        SmallButton.GetComponent<Image>().sprite = UnselectedSmallButton;

        TextSizeIndex = 2;
    }

    public void ApplyTextSpeed()
    {
        int index = TextSpeedDropdown.value;
        PlayerPrefs.SetInt("TextSpeedIndex", index);
        PlayerPrefs.Save();
        Debug.Log("TextSpeedIndex: " + index);
    }

    public void TextSpeed1XPressed()
    {
        TSp1Button.GetComponent<Image>().sprite = Selected1XButton;

        TSp15Button.GetComponent<Image>().sprite = Unselected15XButton;
        TSp2Button.GetComponent<Image>().sprite = Unselected2XButton;

        TextSpeedIndex = 0;
    }
    public void TextSpeed15XPressed()
    {
        TSp15Button.GetComponent<Image>().sprite = Selected15XButton;

        TSp1Button.GetComponent<Image>().sprite = Unselected1XButton;
        TSp2Button.GetComponent<Image>().sprite = Unselected2XButton;

        TextSpeedIndex = 1;
    }
    public void TextSpeed2XPressed()
    {
        TSp2Button.GetComponent<Image>().sprite = Selected2XButton;

        TSp1Button.GetComponent<Image>().sprite = Unselected1XButton;
        TSp15Button.GetComponent<Image>().sprite = Unselected15XButton;

        TextSpeedIndex = 2;
    }

    //Volume
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
        RevertUnsavedChanges();

        gameObject.SetActive(false);
    }

    public void SaveSettings()
    {
        //Volume
        PlayerPrefs.SetFloat("MasterVolume", MasterVolumeSlider.value);
        PlayerPrefs.SetFloat("MusicVolume", MusicVolumeSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolumeSlider.value);

        //Text Speed
        PlayerPrefs.SetInt("TextSpeedIndex", TextSpeedIndex);

        //Text Size
        PlayerPrefs.SetInt("TextSizeIndex", TextSizeIndex);
        
        PlayerPrefs.Save();

        Debug.Log("Settings saved!");

    }

    public void LoadSettings()
    {
        //Volume
        MasterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0f);
        MusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0f);
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0f);

        MainAudioMixer.SetFloat("MasterVolume", MasterVolumeSlider.value);
        MainAudioMixer.SetFloat("MusicVolume", MusicVolumeSlider.value);
        MainAudioMixer.SetFloat("SFXVolume", SFXVolumeSlider.value);

        //Text Speed
        TextSpeedIndex = PlayerPrefs.GetInt("TextSpeedIndex", 0);
        switch (TextSpeedIndex)
        {
            case 0:
                TextSpeed1XPressed();
                break;
            case 1:
                TextSpeed15XPressed();
                break;
            case 2:
                TextSpeed2XPressed();
                break;
        }

        //Text Size
        TextSizeIndex = PlayerPrefs.GetInt("TextSizeIndex", 1);
        switch (TextSizeIndex)
        {
            case 0:
                TextSizeSmallPressed();
                break;
            case 1:
                TextSizeMidPressed();
                break;
            case 2:
                TextSizeBigPressed();
                break;
        }

        Debug.Log("Settings loaded!");

    }
    private void RevertUnsavedChanges()
    {
        //Volume
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 0f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0f);

        MasterVolumeSlider.SetValueWithoutNotify(masterVolume);
        MusicVolumeSlider.SetValueWithoutNotify(musicVolume);
        SFXVolumeSlider.SetValueWithoutNotify(sfxVolume);

        MainAudioMixer.SetFloat("MasterVolume", masterVolume);
        MainAudioMixer.SetFloat("MusicVolume", musicVolume);
        MainAudioMixer.SetFloat("SFXVolume", sfxVolume);

        //Text Speed
        TextSpeedIndex = PlayerPrefs.GetInt("TextSpeedIndex", 0);
        switch (TextSpeedIndex)
        {
            case 0:
                TextSpeed1XPressed();
                break;
            case 1:
                TextSpeed15XPressed();
                break;
            case 2:
                TextSpeed2XPressed();
                break;
        }

        //Text Size
        TextSizeIndex = PlayerPrefs.GetInt("TextSizeIndex", 1);
        switch (TextSizeIndex)
        {
            case 0:
                TextSizeSmallPressed();
                break;
            case 1:
                TextSizeMidPressed();
                break;
            case 2:
                TextSizeBigPressed();
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
