using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    public float maxHealth = 10f;
    private float currentHealth;
    public GameObject explosionPrefab;
    public GameObject destroyPrefab;
    public AudioClip explosionSound;
    public AudioClip destroyedSound;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            // Object is destroyed
            Instantiate(destroyPrefab, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySoundFXClip(explosionSound, transform);
            Destroy(gameObject);
        }
        else
        {
            // Object is damaged but not destroyed
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }
    }
} 