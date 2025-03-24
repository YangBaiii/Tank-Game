using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        resumeButton.onClick.AddListener(OnResumeClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        gameObject.SetActive(false);
    }

    private void OnResumeClicked()
    {
        PauseManager.Instance.ResumeGame();
    }

    private void OnQuitClicked()
    {
        PauseManager.Instance.QuitGame();
    }
} 