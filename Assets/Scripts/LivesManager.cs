using System;
using UnityEngine;
using System.Collections;
using Unity.Mathematics;

public class LivesManager : MonoBehaviour
{
    public static LivesManager Instance { get; private set; }
    
    private int maxLives = 10; // Initial max lives
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
        heartObjects = new GameObject[11]; // Allow up to 20 hearts dynamically
        CreateHearts();
    }

    private void CreateHearts()
    {
        if (heartPrefab == null) return;

        float startX = -15f;
        float startY = 1.5f;
        float spacingX = 2f; // Adjust for horizontal spacing
        float spacingY = 2.5f; // Adjust for row spacing

        for (int i = 0; i < currentLives; i++)
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
            UpdateHearts();

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

    public void AddLife()
    {
        currentLives++; 
        UpdateHearts();
    }

    private void UpdateHearts()
    {
        // If new heart needed, create it
        if (heartObjects[currentLives - 1] == null)
        {
            float startX = -15f;
            float startY = 1.5f;
            float spacingX = 2f;
            float spacingY = 2.5f;

            float x = startX + ((currentLives - 1) % 5) * spacingX;
            float y = startY - ((currentLives - 1) / 5) * spacingY;

            GameObject newHeart = Instantiate(heartPrefab);
            newHeart.transform.position = new Vector3(x, y, 0f);
            heartObjects[currentLives - 1] = newHeart;
        }

        // Update heart visibility
        for (int i = 0; i < heartObjects.Length; i++)
        {
            if (heartObjects[i] != null)
            {
                heartObjects[i].SetActive(i < currentLives);
            }
        }
    }

    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(1f);
        Instantiate(playerPrefab, respawnPosition, Quaternion.identity);
    }
}
