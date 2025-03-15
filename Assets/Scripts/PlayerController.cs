using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float normalSpeed = 2.5f;
    private float sprintSpeed = 5f;
    private float playerSpeed;
    public bool canSprint = true; // Tracks if sprint is available

    public float sprintDuration = 2.5f; // How long the player can sprint
    public float sprintCooldownDuration = 1.5f; // Cooldown duration after sprinting
    public float sprintTimer = 0f; // Timer to track sprint duration
    public float cooldownTimer = 0f; // Timer to track cooldown period
    public bool isSprinting = false; // Is the player currently sprinting?

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject destroyPrefab;
    [SerializeField] private GameObject explosionPrefab;

    public SprintBar sprintBar; // Reference to the sprint bar script

    void Start()
    {
        playerSpeed = normalSpeed;
    }

    void Update()
    {
        HandleSprint();
        MovePlayer();
        RotateTowardsMouse();
        ShootBullet();
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
        }
    }

    void HandleSprint()
    {
        if (canSprint)
        {
            if (Input.GetKey(KeyCode.LeftShift) && !isSprinting)
            {
                isSprinting = true;
                sprintTimer = sprintDuration; // Reset sprint timer
                playerSpeed = sprintSpeed; // Set sprint speed
            }

            if (isSprinting)
            {
                sprintTimer -= Time.deltaTime;

                if (sprintTimer <= 0f) // Sprint ended
                {
                    isSprinting = false;
                    playerSpeed = normalSpeed;
                    cooldownTimer = sprintCooldownDuration; // Start cooldown
                    canSprint = false; // Disable sprinting during cooldown
                }
            }
        }
        else
        {
            // Handle cooldown and refilling the sprint bar
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer <= 0f)
            {
                canSprint = true; // Enable sprint again after cooldown
            }
        }
    }
}
