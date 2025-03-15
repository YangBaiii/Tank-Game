using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SprintBar : MonoBehaviour
{
    [SerializeField] private bool isBillboarded = true;

    private float finalValue; // The final fill amount for the bar
    private float animationSpeed = 0.1f; // Speed at which the bar updates
    private float leftoverAmount = 0f;

    // Caches
    private PlayerController playerController;
    private Image image;
    private FollowCameraRotation followCameraRotation;

    private void Start()
    {
        // Get the PlayerController (assuming it’s a parent or related object)
        playerController = GetComponentInParent<PlayerController>();
        image = GetComponentInChildren<Image>(); // The Image that will be filled
        followCameraRotation = GetComponent<FollowCameraRotation>();

        // Optional rotation
        followCameraRotation.enabled = isBillboarded;
    }

    void Update()
    {
        // Update the animation speed, could be adjusted based on a variable
        animationSpeed = playerController.sprintCooldownDuration;

        // Update the sprint bar based on sprinting and cooldown status
        UpdateSprintBar();
    }

    private void UpdateSprintBar()
    {
        if (playerController.isSprinting)
        {
            // Sprinting: Decrease the bar as the player sprints
            image.fillAmount = playerController.sprintTimer / playerController.sprintDuration;
        }
        else
        {
            // Cooldown: Gradually refill the bar after sprinting ends
            StopAllCoroutines();
            StartCoroutine(ChangeFillAmount(playerController.cooldownTimer / playerController.sprintCooldownDuration));
        }
    }

    private IEnumerator ChangeFillAmount(float targetValue)
    {
        finalValue = targetValue;
        float cacheLeftoverAmount = this.leftoverAmount;

        float timeElapsed = 0;

        // Smoothly fill the bar during cooldown
        while (timeElapsed < animationSpeed)
        {
            float currentAmount = Mathf.Lerp(cacheLeftoverAmount, finalValue, timeElapsed / animationSpeed);
            this.leftoverAmount = currentAmount - finalValue;
            image.fillAmount = currentAmount;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        this.leftoverAmount = 0;
        image.fillAmount = finalValue;
    }
}
