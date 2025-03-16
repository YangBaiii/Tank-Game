using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FollowCameraRotation))]
public class SprintCooldownBar : MonoBehaviour
{
    [SerializeField] bool isBillboarded = true;
    [SerializeField] private Image sprintBar;
    [SerializeField] private float smoothSpeed = 10f;
    
    private PlayerController playerController;
    private FollowCameraRotation followCameraRotation;

    private void Start()
    {
        sprintBar = GetComponentInChildren<Image>();
        playerController = GetComponentInParent<PlayerController>();
        followCameraRotation = GetComponent<FollowCameraRotation>();
        followCameraRotation.enabled = isBillboarded;
        sprintBar.fillAmount = 1f;
    }

    void Update()
    {
        float targetFillAmount;
        
        if (playerController.isSprinting)
        {
            // When sprinting, show remaining sprint time (100% to 0%)
            targetFillAmount = playerController.remainingSprintTime / playerController.maxSprintDuration;
        }
        else if (!playerController.canSprint)
        {
            // During cooldown, show cooldown progress (0% to 100%)
            targetFillAmount = playerController.remainingSprintTime / playerController.maxSprintDuration;
        }
        else
        {
            // Ready to sprint
            targetFillAmount = 1f;
        }

        // Smoothly update the fill amount
        sprintBar.fillAmount = Mathf.Lerp(sprintBar.fillAmount, targetFillAmount, Time.deltaTime * smoothSpeed);
    }
}