using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class Bat : MonoBehaviour
{
    #region Variables
    [Header("FLIGHT")]
    [SerializeField] private float _Speed = 10f;
    private Rigidbody _rb;

    [Header("DAMAGE")]
    [SerializeField] private PlayerAbilitySO _AbilityOS;
    
    [Header("POOLING")]
    [SerializeField] private PoolSO _PoolData;
    [SerializeField] private float _LifeTime = 5f;
    private float _currentLifeTime = 0f;
    
    #endregion

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.linearVelocity = transform.forward * _Speed;
    }

    private void Update() => DefineLifeTime();

    private void DefineLifeTime()
    {
        _currentLifeTime += Time.deltaTime;
        if (_currentLifeTime >= _LifeTime)
        {
            _currentLifeTime = 0;
            ObjectPool.Instance.ReturnObject(_PoolData, gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _currentLifeTime = 0;
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            damageable.TakeDamage(_AbilityOS.Damage);
            ObjectPool.Instance.ReturnObject(_PoolData, gameObject);
        }
        else
        {
            _currentLifeTime = 0;
            ObjectPool.Instance.ReturnObject(_PoolData, gameObject);
        }
    }
}