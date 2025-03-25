using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class MeleeEnemy : MonoBehaviour
{
    private float enemySpeed = 5f;
    private int[] angles = { 0, 90, 180, 270};
    private int randomAngle;
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

    private float minSpawnDelay = 0f;
    private float maxSpawnDelay = 1f;
    private float spawnDelay;
    private float bulletDetectionRadius = 5f;
    private float dodgeSpeed = 4f;
    private float dodgeDuration = 0.5f;
    private float dodgeCooldown = 1.5f;
    private float lastDodgeTime = 0f;
    private bool isDodging = false;
    private Vector2 dodgeDirection;
    private float dodgeTimer = 0f;
    
    private float minX = -3f;
    private float maxX = 19f;
    private float minY = -9f;
    private float maxY = 12f;

    private float rotationSpeed = 360f;
    private float targetAngle;
    private float stuckTimer = 0.5f;
    private float stuckThreshold = 1f;
    private Vector3 lastPosition;
    
    HealthSystemForDummies healthSystem;
    
    void Start()
    {
        healthSystem = GetComponent<HealthSystemForDummies>();
        healthSystem.OnIsAliveChanged.AddListener(PlayDestroyedAnimation);
        healthSystem.CurrentHealth = maxHealth;
        targetAngle = transform.eulerAngles.z;
        lastPosition = transform.position;
        player = FindObjectOfType<PlayerController>();
        
        // Debug log to check if player is found
        if (player == null)
        {
            Debug.LogWarning("Player not found in scene!");
        }
    }

    void Update()
    {
        // Check if stuck
        if (Vector3.Distance(transform.position, lastPosition) < 0.01f)
        {
            stuckTimer += Time.deltaTime;
            if (stuckTimer > stuckThreshold)
            {
                // Force a new random direction if stuck
                GenerateRandomAngle();
                stuckTimer = 0f;
            }
        }
        else
        {
            stuckTimer = 0f;
        }
        lastPosition = transform.position;

        if (isDodging)
        {
            HandleDodgeMovement();
        }
        else
        {
            MoveForward();
            CheckForBullets();
        }
    }

    void HandleDodgeMovement()
    {
        Vector3 newPosition = transform.position + (Vector3)dodgeDirection * dodgeSpeed * Time.deltaTime;
        if (IsWithinBoundaries(newPosition))
        {
            transform.position = newPosition;
            targetAngle = Mathf.Atan2(dodgeDirection.y, dodgeDirection.x) * Mathf.Rad2Deg;
            SmoothRotation();
        }
        else
        {
            isDodging = false;
            GenerateRandomAngle();
        }
        
        dodgeTimer += Time.deltaTime;
        if (dodgeTimer >= dodgeDuration)
        {
            isDodging = false;
            lastDodgeTime = Time.time;
        }
    }

    void MoveForward()
    {
        transform.Translate(Vector2.right * enemySpeed * Time.deltaTime);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Enemy")
            || collision.gameObject.CompareTag("Destructible") || collision.gameObject.CompareTag("UnDestructible"))
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
        Debug.Log($"Dealing {damageAmount} damage to player");
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySoundFXClip(explosionSound, transform);
        player.TakeDamage(damageAmount);
    }


    void CheckForBullets()
    {
        if (Time.time - lastDodgeTime < dodgeCooldown)
        {
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bulletDetectionRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("PlayerBullet"))
            {
                Vector2 bulletDirection = (collider.transform.position - transform.position).normalized;
                
                float dodgeAngle = 30f * Mathf.Deg2Rad;
                float randomSign = Random.value > 0.5f ? 1f : -1f;
                
                float cos = Mathf.Cos(dodgeAngle);
                float sin = Mathf.Sin(dodgeAngle) * randomSign;
                dodgeDirection = new Vector2(
                    bulletDirection.x * cos - bulletDirection.y * sin,
                    bulletDirection.x * sin + bulletDirection.y * cos
                ).normalized;

                isDodging = true;
                dodgeTimer = 0f;
                break;
            }
        }
    }

    bool IsWithinBoundaries(Vector3 position)
    {
        return position.x >= minX && position.x <= maxX && 
               position.y >= minY && position.y <= maxY;
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
        
        newAngle = newAngle % 360f;
        if (newAngle < 0) newAngle += 360f;
        
        transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }

    void GenerateRandomAngle()
    {
        randomAngle = angles[Random.Range(0, angles.Length)];
        randomAngle += Random.Range(-15, 16);
        transform.rotation = Quaternion.Euler(0, 0, randomAngle);
    }

    public void PlayDestroyedAnimation(bool isAlive)
    {
        if (!isAlive)
        {
            StopAllCoroutines();
            Instantiate(destroyPrefab, transform.position, Quaternion.identity);
            SoundManager.Instance.PlaySoundFXClip(destroyedSound, transform); ScoreManager.Instance.AddScore();
            }Destroy(gameObject);
        }
    }
