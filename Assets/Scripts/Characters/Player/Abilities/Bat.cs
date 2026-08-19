using UnityEngine;

/// <summary>
/// The Bat class controls a flying bat projectile.
/// It uses the object pooling system so the bat can be reused instead of
/// destroyed and recreated every time it is spawned.
/// </summary>
[RequireComponent (typeof(Rigidbody))]
public class Bat : MonoBehaviour, IPooledObject
{
    #region Variables
    [Header("Flight")]
    // Movement speed of the bat.
    [SerializeField] private float _Speed = 10f;
    
    // Reference to the bat's Rigidbody component.
    private Rigidbody _rb;

    [Header("Damage")]
    // Scriptable Object containing ability damage information.
    [SerializeField] private PlayerAbilitySO _AbilityOS;
    
    [Header("Pooling")]
    // Maximum time the bat can exist before returning to the pool.
    [SerializeField] private float _LifeTime = 5f;
    
    // Tracks how long the bat has been active.
    private float _currentLifeTime = 0f;
    #endregion
    
    /// <summary>
    /// Called by the ObjectPooler whenever this object is spawned.
    /// Resets the lifetime timer so the bat gets a full duration every time it
    /// is reused.
    /// </summary>
    public void OnObjectSpawn() => _currentLifeTime = 0f;

    // Get and store the Rigidbody component attached to this object.
    private void Awake() => _rb = GetComponent<Rigidbody>();

    // Give the bat forward movement when it is created.
    private void Start() => _rb.linearVelocity = transform.forward * _Speed;

    // Check if the bat has reached its maximum lifetime.
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

    // Called when the bat collides with another collider.
    private void OnTriggerEnter(Collider other)
    {
        // Check if the bat hit an enemy.
        if (other.CompareTag("Enemy"))
        {
            // Reset lifetime after hitting an enemy.
            _currentLifeTime = 0;
            
            // Get the damageable component from the enemy.
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            
            // Apply damage using the value stored in the ability Scriptable
            // Object.
            damageable.TakeDamage(_AbilityOS.Damage);
            
            // Return the bat to the pool after dealing damage.
            ReturnToPool();
        }
        
        // If the bat hits anything else, immediately return it to the pool.
        else
        {
            _currentLifeTime = 0;
            ObjectPooler.Instance.ReturnObject(gameObject);
        }
    }
}