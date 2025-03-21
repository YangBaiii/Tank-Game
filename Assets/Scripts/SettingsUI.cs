using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Slider soundEffectSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button backButton;

    public AudioClip buttonClickSound;

    void Start()
    {
        soundEffectSlider.value = SettingsManager.Instance.GetSFXVolume();
        musicSlider.value = SettingsManager.Instance.GetMusicVolume();
        
        soundEffectSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        saveButton.onClick.AddListener(OnSaveButtonClicked);
        backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnSFXVolumeChanged(float value)
    {
        SettingsManager.Instance.SetSoundEffectVolume(value);
    }

    private void OnMusicVolumeChanged(float value)
    {
        SettingsManager.Instance.SetMusicVolume(value);
    }

    private void OnSaveButtonClicked()
    {
        SoundManager.Instance.PlayUISound(buttonClickSound);
        SettingsManager.Instance.SaveSettings();
    }

    private void OnBackButtonClicked()
    {
        SoundManager.Instance.PlayUISound(buttonClickSound);
        SettingsManager.Instance.HideSettings();
    }
} 