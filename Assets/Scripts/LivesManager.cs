using UnityEngine;
using System.Collections;

public class LivesManager : MonoBehaviour
{
    public static LivesManager Instance { get; private set; }
    
    [SerializeField] private int maxLives = 10;
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 respawnPosition = new Vector3(3f, 0f, 0f);
    
    private GameObject[] heartObjects;
    private int currentLives;

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
        currentLives = maxLives;
        CreateHearts();
    }

    private void CreateHearts()
    {
        if (heartPrefab == null) return;

        heartObjects = new GameObject[maxLives];

        float startX = -15f;
        float startY = 1.5f;
        float spacingX = 2f; // Adjust for horizontal spacing
        float spacingY = 2.5f; // Adjust for row spacing

        for (int i = 0; i < maxLives; i++)
        {
            float x = startX + (i % 5) * spacingX;
            float y = startY - (i / 5) * spacingY;

            GameObject heart = Instantiate(heartPrefab);
            heart.transform.position = new Vector3(x, y, 0f); // Set position manually
            heartObjects[i] = heart;
        }
    }

    public void LoseLife()
    {
        if (currentLives > 0)
        {
            currentLives--;
            heartObjects[currentLives].SetActive(false); // Hide last heart
            
            if (currentLives > 0)
            {
                StartCoroutine(RespawnPlayer());
            }
            else
            {
                Debug.Log("Game Over!");
            }
        }
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(playerPrefab, respawnPosition, Quaternion.identity);
    }
}
