using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject destructiblePrefab;
    [SerializeField] private GameObject nonDestructiblePrefab;
    [SerializeField] private float minDistanceBetweenObjects = 1.5f;
    [SerializeField] private int numberOfDestructibles = 15;
    [SerializeField] private int numberOfNonDestructibles = 10;
    
    private List<Vector2> occupiedPositions = new List<Vector2>();

    public void GenerateMap(List<Vector2> existingObjects)
    {
        occupiedPositions = new List<Vector2>(existingObjects);
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        
        // Set number of non-destructibles based on level
        numberOfNonDestructibles = currentSceneIndex == 1 ? 10 : 15;

        for (int i = 0; i < numberOfDestructibles; i++)
        {
            Vector2 randomPosition = GetRandomValidPosition();
            GameObject destructible = Instantiate(destructiblePrefab, randomPosition, Quaternion.identity);
            destructible.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 4) * 90);
            occupiedPositions.Add(randomPosition);
        }

        for (int i = 0; i < numberOfNonDestructibles; i++)
        {
            Vector2 randomPosition = GetRandomValidPosition();
            GameObject nonDestructible = Instantiate(nonDestructiblePrefab, randomPosition, Quaternion.identity);
            nonDestructible.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 4) * 90);
            occupiedPositions.Add(randomPosition);
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