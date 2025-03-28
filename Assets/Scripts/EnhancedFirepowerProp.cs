using UnityEngine;

public class EnhancedFirepowerProp : MonoBehaviour
{
    [SerializeField] private float powerIncrease = 1.5f;
    [SerializeField] protected AudioClip collectSound;

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollect();
        }
    }
    private void OnCollect()
    {
        PlayerController.Instance.currentAttackPower = powerIncrease;
        SoundManager.Instance.PlaySoundFXClip(collectSound, transform);
        Destroy(gameObject);
    }
    
} 