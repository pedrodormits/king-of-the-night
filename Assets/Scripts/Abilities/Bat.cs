using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Bat : MonoBehaviour, IPooledObject
{
    #region Variables
    [Header("FLIGHT")]
    [SerializeField] private float _Speed = 10f;
    private Rigidbody _rb;

    [Header("DAMAGE")]
    [SerializeField] private PlayerAbilitySO _AbilityOS;
    
    [Header("POOLING")]
    [SerializeField] private float _LifeTime = 5f;
    private float _currentLifeTime = 0f;
    #endregion
    
    public void OnObjectSpawn() => _currentLifeTime = 0;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.linearVelocity = transform.forward * _Speed;
    }

    private void Update() => ReturnToPool();

    private void ReturnToPool()
    {
        _currentLifeTime += Time.deltaTime;
        if(_currentLifeTime >= _LifeTime)
        {
            ObjectPooler.Instance.ReturnObject(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _currentLifeTime = 0;
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            damageable.TakeDamage(_AbilityOS.Damage);
            ReturnToPool();
        }
        else
        {
            _currentLifeTime = 0;
            ReturnToPool();
        }
    }
}