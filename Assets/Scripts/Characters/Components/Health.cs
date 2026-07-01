using UnityEngine;

/// <summary>
/// Manages the health of a player or entity, Handling damage, healing and death.
/// </summary>
public class Health : MonoBehaviour, IDamageable
{
    #region COMPONENTS
    private Player _Player;
    private PlayerAnimation _playerAnimation;
    #endregion
    
    #region HEALTH STATS
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _currentHealth;
    #endregion
    
    private void Awake()
    {
        _Player = GetComponent<Player>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    #region DAMAGE
    /// <summary>
    /// Reduces the health by the specified damage amount, without underflowing health.
    /// Triggers death if health reaches zero.
    /// </summary>
    /// <param name="damageAmount">The amount of damage to apply.</param>
    public void TakeDamage(int damageAmount)
    {
        _currentHealth = Mathf.Max(_currentHealth - damageAmount, 0);
        if (_currentHealth <= 0)
            Die();
    }
    #endregion
    
    #region DEATH
    private void Die()
    {
        if (_Player)
        {
            _Player.enabled = false;
            _playerAnimation.PlayDeathAnimation();
            GameManager.Instance.GameOver();
            if (TryGetComponent(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true;
            }
        }
        
        else
            Destroy(gameObject);
    }
    #endregion

    #region HEALING
    /// <summary>
    /// Heals the entity by the specified amount, without exceeding maximum health.
    /// </summary>
    /// <param name="amount">The amount of health to restore.</param>
    public void Heal(int amount) => _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
    #endregion
}