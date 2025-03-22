using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource soundObject; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform)
    {
        AudioSource audioSource = Instantiate(soundObject, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = SettingsManager.Instance != null ? SettingsManager.Instance.GetSFXVolume() : 1f;
        audioSource.Play();
        float clipLength = audioClip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlayUISound(AudioClip audioClip)
    {
        AudioSource audioSource = Instantiate(soundObject);
        audioSource.clip = audioClip;
        audioSource.volume = SettingsManager.Instance != null ? SettingsManager.Instance.GetSFXVolume() : 1f;
        audioSource.Play();
        float clipLength = audioClip.length;
        Destroy(audioSource.gameObject, clipLength);
    }
}