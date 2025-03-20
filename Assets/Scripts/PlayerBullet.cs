using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private float bulletSpeed = 10f;
    public float destructibleDamage = 5f;
    public float enemyDamage = 2f;
    public AudioClip bulletHit;
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
            SoundManager.Instance.PlaySoundFXClip(bulletHit, transform, 1f);
            DestructibleObject destructible = collision.gameObject.GetComponent<DestructibleObject>();
            if (destructible != null)
            {
                destructible.TakeDamage(destructibleDamage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("UnDestructible"))
        {
            SoundManager.Instance.PlaySoundFXClip(bulletHit, transform, 1f);
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            HealthSystemForDummies enemyHealth = collision.gameObject.GetComponent<HealthSystemForDummies>();
            if (enemyHealth != null)
            {
                enemyHealth.AddToCurrentHealth(-enemyDamage);
            }
            Destroy(gameObject);
        }
    }
}
