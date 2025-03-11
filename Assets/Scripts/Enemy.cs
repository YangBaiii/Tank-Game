using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    private float enemySpeed = 5f;
    private int[] angles = { 0, 90, 180, 270};
    private int randomAngle;
    
    [SerializeField] private GameObject enemyBulletPrefab;
    [SerializeField] private Transform enemyBulletSpawnPoint;
    [SerializeField] private GameObject explosionPrefab;

    private float minSpawnDelay = 1f;
    private float maxSpawnDelay = 2f;

    private float spawnDelay;
    // Start is called before the first frame update
    void Start()
    {
        ShootBullet();
    }

    void Update()
    {
        transform.position += transform.right * enemySpeed * Time.deltaTime;
    }

    void GenerateRandomAngle()
    {
        randomAngle = angles[Random.Range(0, angles.Length)];
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            float currentAngle = transform.eulerAngles.z;
            int newAngle;
            do
            {
                GenerateRandomAngle();
                newAngle = randomAngle;
            } 
            while (Mathf.Approximately(newAngle, currentAngle));

            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("TurningPoint"))
        {
            GenerateRandomAngle();
            transform.rotation = Quaternion.Euler(0, 0, randomAngle);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           Destroy(collision);
           Destroy(gameObject);
           Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }

    private void ShootBullet()
    {
        Instantiate(enemyBulletPrefab, enemyBulletSpawnPoint.transform.position, transform.rotation);
        StartCoroutine(ShootNextBullet());
    }

    IEnumerator ShootNextBullet()
    {
        spawnDelay = Random.Range(minSpawnDelay, maxSpawnDelay);
        yield return new WaitForSeconds(spawnDelay);
        ShootBullet();
    }
}