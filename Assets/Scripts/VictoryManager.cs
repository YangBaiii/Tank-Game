using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager Instance { get; private set; }

    [SerializeField] private GameObject victoryUI;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button backButton;

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

    private void Start()
    {
        continueButton.onClick.AddListener(OnContinueClicked);
        backButton.onClick.AddListener(OnBackClicked);
    }

    public void ShowVictory()
    {
        victoryUI.SetActive(true);
        Time.timeScale = 0f; // Pause the game time
    }

    private void OnContinueClicked()
    {
        Time.timeScale = 1f; // Resume the game time
        int currentScore = ScoreManager.Instance.GetCurrentScore();
        float currentTime = TimeManager.Instance.GetCurrentTime();

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1).completed += (operation) =>
        {
            ScoreManager.Instance.ResetScore(currentScore);
            TimeManager.Instance.ResetTime(currentTime);
        };
    }

    private void OnBackClicked()
    {
        Time.timeScale = 1f; // Resume the game time
        SceneManager.LoadScene("MainMenu");
    }
}