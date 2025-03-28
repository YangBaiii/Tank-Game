using UnityEngine;

public class BloodPackProp : MonoBehaviour
{
    [SerializeField] private float despawnTime = 10f;
    [SerializeField] private GameObject collectEffectPrefab;
    [SerializeField] private AudioClip collectSound;

    private bool isCollected = false; // Prevent multiple triggers

    private void Start()
    {
        Destroy(gameObject, despawnTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            isCollected = true; // Prevent duplicate collection
            PlayerController player = other.GetComponent<PlayerController>();
            OnCollect(player);
        }
    }

    private void OnCollect(PlayerController player)
    {
        SoundManager.Instance.PlaySoundFXClip(collectSound, transform);
        Destroy(gameObject);
        LivesManager.Instance.AddLife();
        Instantiate(collectEffectPrefab, transform.position, Quaternion.identity);
    }
}
