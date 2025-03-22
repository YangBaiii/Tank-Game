using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager instance;
    public static SettingsManager Instance
    {
        get { return instance; }
    }

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private AudioSource sfxSource; // Reference to the SFX audio source

    private static float soundEffectVolume = 1f;
    private static float musicVolume = 0.5f;
    private float tempSoundEffectVolume = 1f; // Temporary storage for unsaved changes
    private float tempMusicVolume = 0.5f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        soundEffectVolume = PlayerPrefs.GetFloat("SoundEffectVolume", 0.5f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        
        tempSoundEffectVolume = soundEffectVolume;
        tempMusicVolume = musicVolume;
        
        ApplySoundEffectVolume(soundEffectVolume);
        ApplyMusicVolume(musicVolume);
    }

    public void HideSettings()
    {
        ApplySoundEffectVolume(soundEffectVolume);
        ApplyMusicVolume(musicVolume);
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void SetSoundEffectVolume(float volume)
    {
        tempSoundEffectVolume = volume;
        ApplySoundEffectVolume(volume);
    }

    public void SetMusicVolume(float volume)
    {
        tempMusicVolume = volume;
        ApplyMusicVolume(volume);
    }

    private void ApplySoundEffectVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
    }

    private void ApplyMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        if (BackgroundMusic.Instance != null)
        {
            BackgroundMusic.Instance.SetVolume(volume);
        }
    }

    public void SaveSettings()
    {
        soundEffectVolume = tempSoundEffectVolume;
        musicVolume = tempMusicVolume;
        PlayerPrefs.SetFloat("SoundEffectVolume", soundEffectVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public float GetSFXVolume()
    {
        return soundEffectVolume;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }
} 