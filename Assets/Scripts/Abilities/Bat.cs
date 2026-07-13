using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Bat : MonoBehaviour
{
    [Header("FLIGHT")]
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _lifeTime = 5f;
    private Rigidbody _rb;

    [Header("DAMAGE")]
    [SerializeField] private PlayerAbilityOS _AbilityOS;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.linearVelocity = transform.forward * _speed;
        Destroy(gameObject, _lifeTime);
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