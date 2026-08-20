using UnityEngine;

/// <summary>
/// The Bat class controls a flying bat projectile.
/// It uses the object pooling system so the bat can be reused
/// instead of destroyed and recreated every time it is spawned.
/// </summary>
[RequireComponent (typeof(Rigidbody))]
public class Bat : MonoBehaviour, IPooledObject
{
    #region Variables
    [Header("Flight")]
    [SerializeField] private float _MovementSpeed = 10f;
    private Rigidbody _rb;

    [Header("Damage")]
    // Scriptable Object containing ability damage information.
    [SerializeField] private PlayerAbilitySO _AbilitySO;
    
    [Header("Pooling")]
    // Maximum time the bat can exist before returning to the pool.
    [SerializeField] private float _LifeTime = 5f;
    
    private float _currentLifeTime = 0f;
    #endregion
    
    /// <summary>
    /// Called by the ObjectPooler whenever this object is spawned.
    /// Resets the lifetime timer so the bat
    /// gets a full duration every time it is reused.
    /// </summary>
    public void OnObjectSpawn() => _currentLifeTime = 0f;
    
    private void Awake() => _rb = GetComponent<Rigidbody>();

    private void Start()
    {
        if (_AbilitySO == null)
        {
            Debug.Log("AbilitySO is null");
        }
        
        _rb.linearVelocity = transform.forward * _MovementSpeed;    
    }
    
    private void Update() => ReturnToPool(); 

    /// <summary>
    /// Checks how long the bat has been active.
    /// If the lifetime is exceeded, the bat is returned to the object pool.
    /// </summary>
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
            
            // Apply damage using the value
            // stored in the ability Scriptable Object.
            damageable.TakeDamage(_AbilitySO.Damage);
            
            ReturnToPool();
        }
        else
        {
            _currentLifeTime = 0;
            ObjectPooler.Instance.ReturnObject(gameObject);
        }
    }
}