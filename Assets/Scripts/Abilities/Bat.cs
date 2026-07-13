using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Bat : MonoBehaviour
{
    [Header("FLIGHT")]
    [SerializeField] private float _Speed = 10f;
    [SerializeField] private float _LifeTime = 5f;
    private Rigidbody _rb;

    [Header("DAMAGE")]
    [SerializeField] private PlayerAbilityOS _AbilityOS;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.linearVelocity = transform.forward * _Speed;
        Destroy(gameObject, _LifeTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            damageable.TakeDamage(_AbilityOS.Damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}