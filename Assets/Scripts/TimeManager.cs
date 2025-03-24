using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private float gameTime = 0f; 
    [SerializeField] private bool isGameActive = true;
    
    private float currentTime;
    private bool isPaused = false;
    private float pauseStartTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentTime = gameTime;
        UpdateTimeDisplay();
    }

    void Update()
    {
        if (isGameActive && !isPaused)
        {
            currentTime += Time.deltaTime;
            UpdateTimeDisplay();
        }
    }

    void UpdateTimeDisplay()
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
}