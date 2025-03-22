using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    private float normalSpeed = 2.5f;
    private float sprintSpeed = 5f;
    private float playerSpeed;
    private float maxHealth = 30f;
    public bool canSprint = true;
    public bool isSprinting = false;

    public float maxSprintDuration = 5f; // Maximum sprint duration
    public float sprintCooldown = 1.5f;  // Cooldown before sprint can be used again
    public float remainingSprintTime;    // Stores remaining sprint time
    public GameObject destroyPrefab;
    public AudioClip destroyedSound;
    public AudioClip shootSound;

    public UnityEvent<float> OnSprintTimeChanged;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawnPoint;
    HealthSystemForDummies healthSystem;

    void Start()
    {
        playerSpeed = normalSpeed;
        healthSystem = GetComponent<HealthSystemForDummies>();
        healthSystem.OnIsAliveChanged.AddListener(PlayDestroyedAnimation);
        healthSystem.CurrentHealth = maxHealth;
        remainingSprintTime = maxSprintDuration;
        if (OnSprintTimeChanged == null)
            OnSprintTimeChanged = new UnityEvent<float>();
    }

    void Update()
    {
        MovePlayer();
        RotateTowardsMouse();
        ShootBullet();
        HandleSprint();
    }

    void MovePlayer()
    {
        Vector2 moveDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        transform.position += (Vector3)(moveDirection * playerSpeed * Time.deltaTime);
    }

    void RotateTowardsMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void ShootBullet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, transform.rotation);
            if (shootSound != null && SoundManager.Instance != null)
            {
                SoundManager.Instance.PlaySoundFXClip(shootSound, transform);
            }
        }
    }

    void HandleSprint()
    {
        float previousSprintTime = remainingSprintTime;
        
        if (Input.GetKey(KeyCode.LeftShift) && canSprint && remainingSprintTime > 0)
        {
            isSprinting = true;
            playerSpeed = sprintSpeed;
            remainingSprintTime -= Time.deltaTime;
            
            if (remainingSprintTime <= 0)
            {
                isSprinting = false;
                canSprint = false;
                playerSpeed = normalSpeed;
                StartCoroutine(SprintCooldown());
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || !isSprinting)
        {
            playerSpeed = normalSpeed;
        }

        if (previousSprintTime != remainingSprintTime)
        {
            OnSprintTimeChanged.Invoke(remainingSprintTime);
        }
    }

    IEnumerator SprintCooldown()
    {
        yield return new WaitForSeconds(sprintCooldown);
        remainingSprintTime = maxSprintDuration;
        OnSprintTimeChanged.Invoke(remainingSprintTime);
        canSprint = true;
    }
    
    public void PlayDestroyedAnimation(bool isAlive)
    {
        bool characterIsDead = !isAlive;
        if (characterIsDead)
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
    }
}
