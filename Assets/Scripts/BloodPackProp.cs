using UnityEngine;

public class BloodPackProp : MonoBehaviour
{
    [SerializeField] private int healthRestore = 1;
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
        LivesManager.Instance.RestoreHealth(healthRestore);
        SoundManager.Instance.PlaySoundFXClip(collectSound, transform);
        Destroy(gameObject);
    }
} 