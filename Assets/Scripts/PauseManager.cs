using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    private bool isPaused = false;
    [SerializeField] private GameObject PauseMenu;
    private PlayerController player;
    private CursorManager cursorManager;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        cursorManager = FindObjectOfType<CursorManager>();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        PauseMenu.SetActive(isPaused);
        player.SetPaused(isPaused); // Disable movement but keep script active

        // Handle cursor visibility and type
        if (cursorManager != null)
        {
            if (isPaused)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                cursorManager.SetDefaultCursor();
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                cursorManager.SetCustomCursor();
            }
        }
    }

    public void ResumeGame()
    {
        TogglePause();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}