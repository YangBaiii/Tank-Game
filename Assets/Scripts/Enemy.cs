using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float enemySpeed = 5f;
    private int[] angles = { 0, 90, 180, 270 };
    private int randomAngle;
    private Vector2 moveDirection;

    void Start()
    {
        GenerateRandomAngle(); // Set initial direction
    }

    void Update()
    {
        transform.position += (Vector3)(moveDirection * enemySpeed * Time.deltaTime);
    }

    void GenerateRandomAngle()
    {
        int newAngle;
        do
        {
            newAngle = angles[Random.Range(0, angles.Length)];
        } while (newAngle == Mathf.RoundToInt(transform.eulerAngles.z)); // Prevent the same angle

        transform.rotation = Quaternion.Euler(0, 0, newAngle);
        moveDirection = (Vector2)(Quaternion.Euler(0, 0, newAngle) * Vector2.right);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name + " | Tag: " + collision.gameObject.tag);
    
        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wall Detected!");
            GenerateRandomAngle();
        }
    }

}