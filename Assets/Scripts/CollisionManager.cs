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
        if (LivesManager.Instance != null)
        {
            return;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            HandleEnemyCollision(collision);
        }
        else if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            HandleEnemyBulletCollision(collision);
        }
    }

    private void HandleEnemyCollision(Collider2D collision)
    {
        healthSystem.AddToCurrentHealth(-2f);
        HealthSystemForDummies enemyHealth = collision.gameObject.GetComponent<HealthSystemForDummies>();
        if (enemyHealth != null)
        {
            enemyHealth.AddToCurrentHealth(-2f);
        }

        if (!healthSystem.IsAlive)
        {
            if (destroyPrefab != null)
            {
                Instantiate(destroyPrefab, transform.position, Quaternion.identity);
            }
            
            if (destroyedSound != null && SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySoundFXClip(destroyedSound, transform);
            }

            if (LivesManager.Instance != null)
            {
                LivesManager.Instance.LoseLife();
            }
            
            Destroy(gameObject);
        }
        else
        {
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }
            
            if (explosionSound != null && SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySoundFXClip(explosionSound, transform);
            }
        }

        if (enemyHealth != null && !enemyHealth.IsAlive)
        {
            if (destroyPrefab != null)
            {
                Instantiate(destroyPrefab, collision.transform.position, Quaternion.identity);
            }
            
            if (destroyedSound != null && SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySoundFXClip(destroyedSound, collision.transform);
            }
            
            Destroy(collision.gameObject);
        }
        else
        {
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, collision.transform.position, Quaternion.identity);
            }
            
            if (explosionSound != null && SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySoundFXClip(explosionSound, collision.transform);
            }
        }
    }

    private void HandleEnemyBulletCollision(Collider2D collision)
    {
        if (gameObject.CompareTag("Player"))
        {
            healthSystem.AddToCurrentHealth(-1f);
            if (!healthSystem.IsAlive)
            {
                if (destroyPrefab != null)
                {
                    Instantiate(destroyPrefab, transform.position, Quaternion.identity);
                }
                
                if (destroyedSound != null && SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySoundFXClip(destroyedSound, transform);
                }

                if (LivesManager.Instance != null)
                {
                    LivesManager.Instance.LoseLife();
                }
                
                Destroy(gameObject);
            }
            else
            {
                if (explosionPrefab != null)
                {
                    Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                }
                
                if (explosionSound != null && SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlaySoundFXClip(explosionSound, transform);
                }
            }
        }
        else if (gameObject.CompareTag("Destructible"))
        {
            DestructibleObject destructible = gameObject.GetComponent<DestructibleObject>();
            if (destructible != null)
            {
                destructible.TakeDamage(2f);
            }
        }
        Destroy(collision.gameObject);
    }
}