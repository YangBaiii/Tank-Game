using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float bulletSpeed = 8f;
    public float destructibleDamage = 2.5f;
    public float playerDamage = 1f;
    public AudioClip bulletHit;
    public GameObject explosionPrefab;
    public AudioClip explosionSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * bulletSpeed * Time.deltaTime;
        if (transform.position.x < -4 || transform.position.x > 20 || transform.position.y < -20 || transform.position.y > 20)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySoundFXClip(explosionSound, transform);
            HealthSystemForDummies playerHealth = collision.gameObject.GetComponent<HealthSystemForDummies>();
            if (playerHealth != null)
            {
                playerHealth.AddToCurrentHealth(-playerDamage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Destructible"))
        {
            SoundManager.Instance.PlaySoundFXClip(bulletHit, transform);
            DestructibleObject destructible = collision.gameObject.GetComponent<DestructibleObject>();
            if (destructible != null)
            {
                destructible.TakeDamage(destructibleDamage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("UnDestructible"))
        {
            SoundManager.Instance.PlaySoundFXClip(bulletHit, transform);
            Destroy(gameObject);
        }
    }
}
