using UnityEngine;

public class EnhancedFirepowerProp : MonoBehaviour
{
    [SerializeField] private float powerIncrease = 15f;
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
        PlayerBullet.Instance.IncreaseAttackPower(powerIncrease);
        SoundManager.Instance.PlaySoundFXClip(collectSound, transform);
        Destroy(gameObject);
    }
    
} 