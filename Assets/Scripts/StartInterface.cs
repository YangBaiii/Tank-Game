using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartInterface : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject quitConfirmationPanel;

    [Header("Buttons")]
    [SerializeField] private Button startButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button leaderboardsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button confirmQuitButton;
    [SerializeField] private Button cancelQuitButton;

    [Header("Scene Indices")]
    [SerializeField] private int gameSceneIndex = 1;
    [SerializeField] private int settingsSceneIndex = 2;
    [SerializeField] private int leaderboardsSceneIndex = 3;

    private void Start()
    {
        // Add listeners to buttons
        startButton.onClick.AddListener(OnStartButtonClick);
        settingsButton.onClick.AddListener(OnSettingsButtonClick);
        leaderboardsButton.onClick.AddListener(OnLeaderboardsButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
        confirmQuitButton.onClick.AddListener(OnConfirmQuit);
        cancelQuitButton.onClick.AddListener(OnCancelQuit);

        // Initialize panels
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
        if (quitConfirmationPanel != null)
            quitConfirmationPanel.SetActive(false);
    }

    private void OnStartButtonClick()
    {
        SceneManager.LoadScene(gameSceneIndex);
    }

    private void OnSettingsButtonClick()
    {
        SceneManager.LoadScene(settingsSceneIndex);
    }

    private void OnLeaderboardsButtonClick()
    {
        SceneManager.LoadScene(leaderboardsSceneIndex);
    }

    private void OnQuitButtonClick()
    {
        if (quitConfirmationPanel != null && mainMenuPanel != null)
        {
            mainMenuPanel.SetActive(false);
            quitConfirmationPanel.SetActive(true);
        }
        else
        {
            QuitGame();
        }
    }

    public void OnConfirmQuit()
    {
        QuitGame();
    }

    public void OnCancelQuit()
    {
        if (quitConfirmationPanel != null && mainMenuPanel != null)
        {
            quitConfirmationPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
    }

    private void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
} 