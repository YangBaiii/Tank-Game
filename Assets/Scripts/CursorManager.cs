using System.Collections;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;
    [SerializeField] private Texture2D cursorTexture;
    private Vector2 cursorHotSpot;
    private Texture2D modifiedCursor; 

    void Awake()
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
        cursorHotSpot = new Vector2(24, 24); // Center for 48x48
        cursorTexture = ResizeTexture(cursorTexture, 48, 48);
        Cursor.SetCursor(cursorTexture, cursorHotSpot, CursorMode.Auto);
    }

    public void OnShoot()
    {
        if (Instance != null)
        {
            StartCoroutine(FlashRedCursor());
        }
    }

    private IEnumerator FlashRedCursor()
    {
        modifiedCursor = TintTexture(cursorTexture, Color.red);
        Cursor.SetCursor(modifiedCursor, cursorHotSpot, CursorMode.Auto);
        yield return new WaitForSeconds(0.1f);
        Cursor.SetCursor(cursorTexture, cursorHotSpot, CursorMode.Auto);
    }

    private Texture2D TintTexture(Texture2D original, Color color)
    {
        Texture2D tinted = new Texture2D(original.width, original.height);
        for (int x = 0; x < original.width; x++)
        {
            for (int y = 0; y < original.height; y++)
            {
                Color originalColor = original.GetPixel(x, y);
                tinted.SetPixel(x, y, new Color(color.r, color.g, color.b, originalColor.a));
            }
        }
        tinted.Apply();
        return tinted;
    }

    private Texture2D ResizeTexture(Texture2D source, int width, int height)
    {
        RenderTexture rt = new RenderTexture(width, height, 32);
        Graphics.Blit(source, rt);
        Texture2D result = new Texture2D(width, height);
        RenderTexture.active = rt;
        result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        result.Apply();
        RenderTexture.active = null;
        return result;
    }
}