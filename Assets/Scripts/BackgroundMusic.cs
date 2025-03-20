using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    public static BackgroundMusic Instance
    {
        get { return instance; }
    }

    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;
    private AudioSource audioSource;
    private int currentMusicIndex = 0;

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
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = 0.5f; 
        SceneManager.sceneLoaded += OnSceneLoaded;

        PlayMenuMusic();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            PlayGameMusic();
        }
        else
        {
            PlayMenuMusic();
        }
    }

    public void PlayMenuMusic()
    {
        if (currentMusicIndex != 0)
        {
            currentMusicIndex = 0;
            audioSource.clip = menuMusic;
            audioSource.Play();
        }
    }

    public void PlayGameMusic()
    {
        if (currentMusicIndex != 1)
        {
            currentMusicIndex = 1;
            audioSource.clip = gameMusic;
            audioSource.Play();
        }
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = Mathf.Clamp01(volume);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
} 