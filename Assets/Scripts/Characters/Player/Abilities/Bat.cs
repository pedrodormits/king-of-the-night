using UnityEngine;

/// <summary>
/// The Bat class controls a flying bat projectile.
/// It uses the object pooling system so the bat can be reused instead of destroyed
/// and recreated every time it is spawned.
/// </summary>
[RequireComponent (typeof(Rigidbody))]
public class Bat : MonoBehaviour, IPooledObject
{
    #region Variables
    [Header("Flight")]
    [SerializeField] private float _Speed = 10f; // Movement speed of the bat.
    private Rigidbody _rb; // Reference to the bat's Rigidbody component.

    [Header("Damage")]
    [SerializeField] private PlayerAbilitySO _AbilityOS; // Scriptable Object containing ability damage information.
    
    [Header("Pooling")]
    [SerializeField] private float _LifeTime = 5f; // Maximum time the bat can exist before returning to the pool.
    private float _currentLifeTime = 0f; // Tracks how long the bat has been active.
    #endregion
    
    /// <summary>
    /// Called by the ObjectPooler whenever this object is spawned.
    /// Resets the lifetime timer so the bat gets a full duration every time it is reused.
    /// </summary>
    public void OnObjectSpawn() => _currentLifeTime = 0f;

    // Get and store the Rigidbody component attached to this object.
    private void Awake() => _rb = GetComponent<Rigidbody>();

    // Give the bat forward movement when it is created.
    private void Start() => _rb.linearVelocity = transform.forward * _Speed;

    private void Update() => ReturnToPool(); // Check if the bat has reached its maximum lifetime.

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

    private void OnTriggerEnter(Collider other) // Called when the bat collides with another collider.
    {
        if (other.CompareTag("Enemy")) // Check if the bat hit an enemy.
        {
            _currentLifeTime = 0; // Reset lifetime after hitting an enemy.
            
            // Get the damageable component from the enemy.
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
            
            // Apply damage using the value stored in the ability Scriptable Object.
            damageable.TakeDamage(_AbilityOS.Damage);
            ReturnToPool(); // Return the bat to the pool after dealing damage.
        }
        else // If the bat hits anything else, immediately return it to the pool.
        {
            _currentLifeTime = 0;
            ObjectPooler.Instance.ReturnObject(gameObject);
        }
    }
}