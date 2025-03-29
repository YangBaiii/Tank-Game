using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject meleeEnemyPrefab;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private PropSpawner propSpawner;
    [SerializeField] private float minDistanceBetweenObjects = 2.5f;
    [SerializeField] private float respawnDelay = 2f;
    
    private Vector2 playerPosition = new Vector2(3f, 0f);
    public List<Vector2> occupiedPositions;
    private int currentRegularEnemies = 0;
    private int currentMeleeEnemies = 0;
    private int maxRegularEnemies = 0;
    private int maxMeleeEnemies = 0;

    void Start()
    {
        occupiedPositions = new List<Vector2>();
        occupiedPositions.Add(playerPosition);
        GenerateEnemies();
        mapGenerator.GenerateMap(occupiedPositions);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 2)
        {
            propSpawner.GenerateMap(occupiedPositions);
        }
    }

    private void GenerateEnemies()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        maxRegularEnemies = currentSceneIndex == 1 ? 1 : 5; // 10 enemies for level 1, 5 for level 2
        maxMeleeEnemies = currentSceneIndex == 1 ? 0 : 5; // 0 melee enemies for level 1, 5 for level 2

        // Generate initial regular enemies
        for (int i = 0; i < maxRegularEnemies; i++)
        {
            SpawnRegularEnemy();
        }

        // Generate initial melee enemies for level 2
        for (int i = 0; i < maxMeleeEnemies; i++)
        {
            SpawnMeleeEnemy();
        }
    }

    public void OnEnemyDestroyed(bool isMelee)
    {
        if (isMelee)
        {
            currentMeleeEnemies--;
            if (currentMeleeEnemies < maxMeleeEnemies)
            {
                Invoke("SpawnMeleeEnemy", respawnDelay);
            }
        }
        else
        {
            currentRegularEnemies--;
            if (currentRegularEnemies < maxRegularEnemies)
            {
                Invoke("SpawnRegularEnemy", respawnDelay);
            }
        }
    }

    private void SpawnRegularEnemy()
    {
        Vector2 newPos = GetRandomValidPosition();
        if (newPos != Vector2.zero)
        {
            GameObject enemy = Instantiate(enemyPrefab, newPos, Quaternion.identity);
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.SetGenerator(this);
            }
            occupiedPositions.Add(newPos);
            currentRegularEnemies++;
        }
    }

    private void SpawnMeleeEnemy()
    {
        Vector2 newPos = GetRandomValidPosition();
        if (newPos != Vector2.zero)
        {
            GameObject enemy = Instantiate(meleeEnemyPrefab, newPos, Quaternion.identity);
            MeleeEnemy enemyScript = enemy.GetComponent<MeleeEnemy>();
            if (enemyScript != null)
            {
                enemyScript.SetGenerator(this);
            }
            occupiedPositions.Add(newPos);
            currentMeleeEnemies++;
        }
    }

    private Vector2 GetRandomValidPosition()
    {
        int maxAttempts = 5000;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            float x = Random.Range(-2, 18);  
            float y = Random.Range(-8, 11);
            Vector2 randomPosition = new Vector2(x, y);
            if (IsValidPosition(randomPosition))
            {
                return randomPosition;
            }

            attempts++;
        }

        Debug.LogWarning("Could not find valid position after " + maxAttempts + " attempts");
        return Vector2.zero;
    }

    private bool IsValidPosition(Vector2 position)
    {
        foreach (Vector2 occupiedPos in occupiedPositions)
        {
            if (Vector2.Distance(position, occupiedPos) < minDistanceBetweenObjects)
            {
                return false;
            }
        }
        return true;
    }
}
