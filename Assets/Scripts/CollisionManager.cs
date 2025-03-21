using System;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject destroyPrefab;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip destroyedSound;

    HealthSystemForDummies healthSystem;

    void Start()
    {
        healthSystem = GetComponent<HealthSystemForDummies>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HandleEnemyCollision(collision);
        }
        else if (collision.gameObject.CompareTag("Destructible") || collision.gameObject.CompareTag("UnDestructible"))
        {
            HandleObstaclesCollision(collision);
        }
        else if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            HandleEnemyBulletCollision(collision);
        }
    }

    private void HandleEnemyCollision(Collider2D collision)
    {
        // Both player and enemy take 5 damage
        healthSystem.AddToCurrentHealth(-5f);
        HealthSystemForDummies enemyHealth = collision.gameObject.GetComponent<HealthSystemForDummies>();
        if (enemyHealth != null)
        {
            enemyHealth.AddToCurrentHealth(-5f);
        }

        if (!healthSystem.IsAlive)
        {
            Instantiate(destroyPrefab, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySoundFXClip(destroyedSound, transform);
            Destroy(gameObject);
        }
        else
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySoundFXClip(explosionSound, transform);
        }

        if (enemyHealth != null && !enemyHealth.IsAlive)
        {
            Instantiate(destroyPrefab, collision.transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySoundFXClip(destroyedSound, collision.transform);
            Destroy(collision.gameObject);
        }
        else
        {
            Instantiate(explosionPrefab, collision.transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySoundFXClip(explosionSound, collision.transform);
        }
    }

    private void HandleObstaclesCollision(Collider2D collision)
    {
        bool isDestructible = collision.gameObject.CompareTag("Destructible");
        
        // Both player and obstacle take 5 damage
        healthSystem.AddToCurrentHealth(-5f);
        
        if (isDestructible)
        {
            DestructibleObject destructible = collision.gameObject.GetComponent<DestructibleObject>();
            if (destructible != null)
            {
                destructible.TakeDamage(5f);
            }
        }

        if (!healthSystem.IsAlive)
        {
            Instantiate(destroyPrefab, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySoundFXClip(destroyedSound, transform);
            Destroy(gameObject);
        }
        else
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySoundFXClip(explosionSound, transform);
        }
    }

    private void HandleEnemyBulletCollision(Collider2D collision)
    {
        if (gameObject.CompareTag("Player"))
        {
            healthSystem.AddToCurrentHealth(-1f);
            if (!healthSystem.IsAlive)
            {
                Instantiate(destroyPrefab, transform.position, Quaternion.identity);
                SoundManager.Instance.PlaySoundFXClip(destroyedSound, transform);
                Destroy(gameObject);
            }
            else
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                SoundManager.Instance.PlaySoundFXClip(explosionSound, transform);
            }
        }
        else if (gameObject.CompareTag("Destructible"))
        {
            DestructibleObject destructible = gameObject.GetComponent<DestructibleObject>();
            if (destructible != null)
            {
                destructible.TakeDamage(2.5f);
            }
        }
        Destroy(collision.gameObject);
    }
}