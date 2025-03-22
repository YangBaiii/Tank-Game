using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private float bulletSpeed = 10f;
    public float destructibleDamage = 5f;
    public float enemyDamage = 2f;
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
        if (transform.position.x < -5 || transform.position.x > 20 || transform.position.y < -20 || transform.position.y > 20)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Destructible"))
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
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySoundFXClip(explosionSound, transform);
            HealthSystemForDummies enemyHealth = collision.gameObject.GetComponent<HealthSystemForDummies>();
            if (enemyHealth != null)
            {
                enemyHealth.AddToCurrentHealth(-enemyDamage);
            }
            Destroy(gameObject);
        }
    }
}
