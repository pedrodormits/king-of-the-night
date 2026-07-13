using UnityEngine;

public enum ResourceType
{
    Health,
    Mana
}

public class RecoveryItem : MonoBehaviour
{
    public ResourceType ResourceType;
    public RecoverySO RecoveryData;
    [SerializeField] private ParticleSystem _healParticle;
    // [SerializeField] ParticleSystem ManaParticle;
    [SerializeField] private AudioClip _healClip;

    private void Awake() => _healParticle = GameObject.Find("HealParticle").GetComponent<ParticleSystem>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Use(other.gameObject);
        }
    }

    public void Use(GameObject target)
    {
        var health = target.GetComponent<PlayerHealth>();

        switch (ResourceType)
        {
            case ResourceType.Health:
                if (health != null)
                {
                    health.Heal(RecoveryData.HealAmount);
                    _healParticle.Play();
                    AudioSource.PlayClipAtPoint(_healClip, UnityEngine.Camera.main.transform.position);
                }
                    
                break;
            
            // case ResourceType.Mana:
            
            // break;
        }

        Destroy(gameObject);
    }
}