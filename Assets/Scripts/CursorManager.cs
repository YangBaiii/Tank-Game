using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }

    [SerializeField] private Texture2D customCursor;
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Vector2 cursorHotspot = new Vector2(24, 24);

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
        SetCustomCursor();
    }

    public void SetCustomCursor()
    {
        if (customCursor != null)
        {
            Cursor.SetCursor(customCursor, cursorHotspot, CursorMode.Auto);
        }
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void OnShoot()
    {
        StartCoroutine(FlashRedCursor());
    }

    private System.Collections.IEnumerator FlashRedCursor()
    {
        Color[] colors = customCursor.GetPixels();
            Color[] redColors = new Color[colors.Length];
            for (int i = 0; i < colors.Length; i++)
            {
                redColors[i] = new Color(1, 0, 0, colors[i].a);
            }
            Texture2D redCursor = new Texture2D(customCursor.width, customCursor.height);
            redCursor.SetPixels(redColors);
            redCursor.Apply();
            Cursor.SetCursor(redCursor, cursorHotspot, CursorMode.Auto);

            yield return new WaitForSeconds(0.1f);

        SetCustomCursor();
    }
}