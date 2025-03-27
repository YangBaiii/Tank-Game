using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PropSpawner : MonoBehaviour
{
    [SerializeField] private GameObject enhancedFirepowerPrefab;
    [SerializeField] private GameObject bloodPackPrefab;
    private float minDistance = 2.5f;

    public List<Vector2> occupiedPositions;
    public void GenerateMap(List<Vector2> existingObjects)
    {
        occupiedPositions = new List<Vector2>(existingObjects); // Copy pre-filled positions
        GenerateProps(enhancedFirepowerPrefab, 1);
        GenerateProps(bloodPackPrefab, 1);
    }

    private void GenerateProps(GameObject obstaclePrefab, int count)
    {
        int attempts = 0;
        int maxAttempts = 10000;

        for (int i = 0; i < count; i++)
        {
            Vector2 randomPos;
            bool validPosition;

            do
            {
                float x = Random.Range(-2, 18);  
                float y = Random.Range(-8, 11);
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

    private bool IsValidPosition(Vector2 newPos)
    {
        foreach (Vector2 existingPos in occupiedPositions)
        {
            if (Vector2.Distance(existingPos, newPos) < minDistance)
            {
                return false;
            }
        }
        return true;
    }
} 