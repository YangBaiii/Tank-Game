using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    private static int currentScore = 0;
    private int pointsPerEnemy = 20;

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
        FindScoreText();
        UpdateScoreDisplay();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindScoreText();
        UpdateScoreDisplay();
    }

    private void FindScoreText()
    {
        scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();
        if (scoreText == null)
        {
            Debug.LogWarning("ScoreText not found! Make sure the GameObject is named correctly.");
        }
    }

    public void AddScore()
    {
        currentScore += pointsPerEnemy;
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = $"{currentScore}";
        }
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    public void ResetScore(int score)
    {
        currentScore = score;
        UpdateScoreDisplay();
    }
}