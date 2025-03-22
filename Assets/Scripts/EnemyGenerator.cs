using UnityEngine;
using System.Collections.Generic;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private int numberOfEnemies = 10;
    [SerializeField] private float minDistanceBetweenObjects = 2.5f;
    
    private Vector2 playerPosition = new Vector2(3f, 0f);
    private List<Vector2> occupiedPositions = new List<Vector2>();

    void Start()
    {
        occupiedPositions.Add(playerPosition);
        GenerateEnemies();
        if (mapGenerator != null)
        {
            mapGenerator.GenerateMap(occupiedPositions);
        }
    }

    private void GenerateEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector2 randomPosition = GetRandomValidPosition();
            GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
            enemy.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 4) * 90);
            occupiedPositions.Add(randomPosition);
        }
    }

    private Vector2 GetRandomValidPosition()
    {
        int maxAttempts = 5000;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            float x = Random.Range(-3, 19);  
            float y = Random.Range(-9, 12);
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
