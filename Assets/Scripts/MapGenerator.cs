using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject destructiblePrefab;
    [SerializeField] private GameObject nonDestructiblePrefab;

    private int destructibleCount = 15;
    private int nonDestructibleCount = 10;

    private List<Vector2> occupiedPositions = new List<Vector2>(); // Tracks all obstacles
    private float minDistance = 3.0f; // Minimum distance between any two obstacles

    void Start()
    {
        GenerateObstacles(destructiblePrefab, destructibleCount);
        GenerateObstacles(nonDestructiblePrefab, nonDestructibleCount);
    }

    void GenerateObstacles(GameObject obstaclePrefab, int count)
    {
        int attempts = 0;
        int maxAttempts = 5000;

        for (int i = 0; i < count; i++)
        {
            Vector2 randomPos;
            bool validPosition;

            do
            {
                float x = Random.Range(-3, 19);  
                float y = Random.Range(-9, 12);
                randomPos = new Vector2(x, y);
                validPosition = IsValidPosition(randomPos);

                attempts++;
                if (attempts > maxAttempts)
                {
                    Debug.LogWarning("Max attempts reached. Could not place all obstacles.");
                    return;
                }

            } while (!validPosition);

            occupiedPositions.Add(randomPos);
            Instantiate(obstaclePrefab, randomPos, Quaternion.identity);
        }
    }

    bool IsValidPosition(Vector2 newPos)
    {
        foreach (Vector2 existingPos in occupiedPositions)
        {
            if (Vector2.Distance(existingPos, newPos) < minDistance)
            {
                return false; // Too close to another obstacle
            }
        }
        return true;
    }
}