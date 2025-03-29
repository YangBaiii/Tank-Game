using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float gameTime = 0f;
    [SerializeField] private bool isGameActive = true;

    private static float currentTime;
    private bool isPaused = false;
    private float pauseStartTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // Listen for scene changes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        FindTimeText();
        currentTime = gameTime;
        UpdateTimeDisplay();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindTimeText();
        UpdateTimeDisplay();
    }

    private void FindTimeText()
    {
        timeText = GameObject.Find("TimeText")?.GetComponent<TextMeshProUGUI>();
        if (timeText == null)
        {
            Debug.LogWarning("TimeText not found! Make sure the GameObject is named correctly.");
        }
    }

    private void Update()
    {
        if (isGameActive && !isPaused)
        {
            currentTime += Time.deltaTime;
            UpdateTimeDisplay();
        }
    }

    private void UpdateTimeDisplay()
    {
        if (timeText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timeText.text = string.Format("{0:00} {1:00}", minutes, seconds);
        }
    }

    public void PauseGame()
    {
        if (!isPaused)
        {
            isPaused = true;
            pauseStartTime = Time.time;
        }
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            isPaused = false;
            float pauseDuration = Time.time - pauseStartTime;
            currentTime += pauseDuration;
        }
    }

    public void StartGame()
    {
        isGameActive = true;
        currentTime = gameTime;
        UpdateTimeDisplay();
    }

    public void StopGame()
    {
        isGameActive = false;
    }

    private void GameOver()
    {
        isGameActive = false;
        Debug.Log("Game Over - Time's Up!");
        // Add your game over logic here
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public bool IsGameActive()
    {
        return isGameActive;
    }

    public bool IsPaused()
    {
        return isPaused;
    }

    public void ResetTime(float time)
    {
        currentTime = time;
        UpdateTimeDisplay();
    }
}
