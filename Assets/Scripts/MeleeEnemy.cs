using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeEnemy : MonoBehaviour
{
    private float enemySpeed = 5f;
    private int[] angles = { 0, 90, 180, 270 };
    private float maxHealth = 10f;
    private float damageAmount = 1f;
    private float damageInterval = 0.25f;
    private float lastDamageTime;
    private bool isCollidingWithPlayer = false;
    private PlayerController player;

    [SerializeField] private GameObject destroyPrefab;
    [SerializeField] private AudioClip destroyedSound;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private AudioClip explosionSound;

    private float bulletDetectionRadius = 5f;
    private float dodgeSpeed = 4f;
    private float dodgeDuration = 0.5f;
    private float dodgeCooldown = 1.5f;
    private float lastDodgeTime = 0f;
    private bool isDodging = false;
    private Vector2 dodgeDirection;
    private float dodgeTimer = 0f;

    private float rotationSpeed = 360f;
    private float targetAngle;
    private Vector3 lastPosition;
    private float stuckTime = 0f;
    private float stuckThreshold = 0.5f;

    private Rigidbody2D rb;

    HealthSystemForDummies healthSystem;

    void Start()
    {
        healthSystem = GetComponent<HealthSystemForDummies>();
        healthSystem.OnIsAliveChanged.AddListener(PlayDestroyedAnimation);
        healthSystem.CurrentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        targetAngle = transform.eulerAngles.z;
        lastPosition = transform.position;
        player = FindObjectOfType<PlayerController>();
        GenerateRandomAngle();
    }

    void Update()
    {
        if (isDodging)
        {
            HandleDodgeMovement();
        }
        else
        {
            MoveForward();
            CheckForBullets();
            HandleStuckDetection();
        }
    }

    void MoveForward()
    {
        rb.velocity = transform.right * enemySpeed;
    }

    void HandleDodgeMovement()
    {
        rb.velocity = dodgeDirection * dodgeSpeed;
        targetAngle = Mathf.Atan2(dodgeDirection.y, dodgeDirection.x) * Mathf.Rad2Deg;
        SmoothRotation();

        dodgeTimer += Time.deltaTime;
        if (dodgeTimer >= dodgeDuration)
        {
            isDodging = false;
            lastDodgeTime = Time.time;
        }
    }

    private void HandleStuckDetection()
    {
        if (Vector3.Distance(transform.position, lastPosition) < 0.05f)
        {
            stuckTime += Time.deltaTime;
            if (stuckTime > stuckThreshold)
            {
                GenerateRandomAngle();
                stuckTime = 0f;
            }
        }
        else
        {
            stuckTime = 0f;
        }
        lastPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy") ||
            collision.gameObject.CompareTag("Destructible") || collision.gameObject.CompareTag("UnDestructible"))
        {
            GenerateRandomAngle();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = true;
            lastDamageTime = Time.time;
            DealDamage();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isCollidingWithPlayer)
        {
            if (Time.time - lastDamageTime >= damageInterval)
            {
                DealDamage();
                lastDamageTime = Time.time;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollidingWithPlayer = false;
        }
    }

    private void DealDamage()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySoundFXClip(explosionSound, transform);
        player.TakeDamage(damageAmount);
    }

    void CheckForBullets()
    {
        if (Time.time - lastDodgeTime < dodgeCooldown)
            return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bulletDetectionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("PlayerBullet"))
            {
                Vector2 bulletDirection = (collider.transform.position - transform.position).normalized;

                float dodgeAngle = Random.Range(-30f, 30f) * Mathf.Deg2Rad;
                dodgeDirection = new Vector2(
                    bulletDirection.x * Mathf.Cos(dodgeAngle) - bulletDirection.y * Mathf.Sin(dodgeAngle),
                    bulletDirection.x * Mathf.Sin(dodgeAngle) + bulletDirection.y * Mathf.Cos(dodgeAngle)
                ).normalized;

                isDodging = true;
                dodgeTimer = 0f;
                break;
            }
        }
    }

    void SmoothRotation()
    {
        float currentAngle = transform.eulerAngles.z;
        float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);

        if (Mathf.Abs(angleDifference) < 1f)
        {
            transform.rotation = Quaternion.Euler(0, 0, targetAngle);
            return;
        }

        float rotationDirection = Mathf.Sign(angleDifference);
        float newAngle = currentAngle + rotationDirection * rotationSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }

    void GenerateRandomAngle()
    {
        targetAngle = angles[Random.Range(0, angles.Length)] + Random.Range(-15, 16);
        transform.rotation = Quaternion.Euler(0, 0, targetAngle);
        MoveForward();
    }

    public void PlayDestroyedAnimation(bool isAlive)
    {
        if (!isAlive)
        {
            StopAllCoroutines();
            Instantiate(destroyPrefab, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySoundFXClip(destroyedSound, transform);
            ScoreManager.Instance.AddScore();
            Destroy(gameObject);
        }
    }
}
