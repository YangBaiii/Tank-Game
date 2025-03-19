using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private float bulletSpeed = 8f;
    public float destructibleDamage = 2.5f;
    public float playerDamage = 1f;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthSystemForDummies playerHealth = collision.gameObject.GetComponent<HealthSystemForDummies>();
            if (playerHealth != null)
            {
                playerHealth.AddToCurrentHealth(-playerDamage);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Destructible"))
        {
            DestructibleObject destructible = collision.gameObject.GetComponent<DestructibleObject>();
            if (destructible != null)
            {
                destructible.TakeDamage(destructibleDamage);
            }
            Destroy(gameObject);
        }
    }
}
