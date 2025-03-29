using UnityEngine;
using System.Collections;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

public class LivesManager : MonoBehaviour
{
    public static LivesManager Instance { get; private set; }

    private int maxLives = 10;
    private int currentLives;
    
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 respawnPosition = new Vector3(3f, 0f, 0f);

    private GameObject[] heartObjects;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            currentLives = maxLives; // Initialize only once
            heartObjects = new GameObject[maxLives]; 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CreateHearts(); // Recreate hearts on scene load
    }

    private void Start()
    {
        CreateHearts();
    }

    private void CreateHearts()
    {
        if (heartPrefab == null) return;

        // Clear existing hearts before creating new ones
        foreach (var heart in heartObjects)
        {
            if (heart != null)
                Destroy(heart);
        }

        float startX = -15f;
        float startY = 2.5f;
        float spacingX = 2f;
        float spacingY = 2.5f;

        for (int i = 0; i < currentLives; i++)
        {
            float x = startX + (i % 5) * spacingX;
            float y = startY - (i / 5) * spacingY;

            GameObject heart = Instantiate(heartPrefab);
            heart.transform.position = new Vector3(x, y, 0f);
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
        if (currentLives < maxLives)
        {
            currentLives++;
            UpdateHearts();
        }
    }

    private void UpdateHearts()
    {
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
