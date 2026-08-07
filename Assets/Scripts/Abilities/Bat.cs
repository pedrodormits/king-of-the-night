using System;
using UnityEngine;

/// <summary>
/// Represents a bat projectile that flies forward, damages enemies on contact,
/// and automatically returns itself to the object pool after a set lifetime
/// or immediately after colliding with another object.
/// </summary>
[RequireComponent (typeof(Rigidbody))]
public class Bat : MonoBehaviour, IPooledObject
{
    #region Variables
    [Header("Flight")]
    [SerializeField] private float _Speed = 10f;
    private Rigidbody _rb;

    [Header("Damage")]
    [SerializeField] private PlayerAbilitySO _AbilityOS;
    
    [Header("Pooling")]
    [SerializeField] private float _LifeTime = 5f;
    private float _currentLifeTime = 0f;
    #endregion
    
    // Reset the lifetime timer.
    public void OnObjectSpawn() => _currentLifeTime = 0f;

    // Cache the Rigidbody reference once.
    private void Awake() => _rb = GetComponent<Rigidbody>();

    // Launch the bat in its forward direction.
    private void Start() => _rb.linearVelocity = transform.forward * _Speed;

    private void Update() => ReturnToPool();

    /// <summary>
    /// Updates the bat's lifetime and automatically returns it to the object pool
    /// once its maximum lifetime has been reached.
    /// </summary>
    private void ReturnToPool()
    {
        // Keep track of the bat's lifetime.
        _currentLifeTime += Time.deltaTime;
        
        // Return the bat to the pool once its lifetime expires.
        if(_currentLifeTime >= _LifeTime)
        {
            ObjectPooler.Instance.ReturnObject(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Deal damage if the collided object implements IDamageable.
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
            
            // Return the projectile to the pool after any collision.
            ObjectPooler.Instance.ReturnObject(gameObject);
        }
    }
}