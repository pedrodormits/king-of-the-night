using System;
using UnityEngine;

/// <summary>
/// Manages the health of a player or entity, Handling damage, healing and death.
/// </summary>
public class Health : MonoBehaviour, IDamageable
{
    #region COMPONENTS
    [SerializeField] private HealthBar _HealthBar;
    private Player _Player;
    private PlayerAnimation _playerAnimation;
    #endregion
    
    #region HEALTH STATS
    [SerializeField] private int _MaxHealth;
    [SerializeField] private int _CurrentHealth;
    public int MaxHealth => _MaxHealth;
    public int CurrentHealth => _CurrentHealth;
    #endregion
    
    private void Awake()
    {
        _Player = GetComponent<Player>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void Start()
    {
        _CurrentHealth = _MaxHealth;
        _HealthBar.SetMaxHealth(_MaxHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(20);
        }
    }

    #region DAMAGE
    /// <summary>
    /// Reduces the health by the specified damage amount, without underflowing health.
    /// Triggers death if health reaches zero.
    /// </summary>
    /// <param name="damageAmount">The amount of damage to apply.</param>
    public void TakeDamage(int damageAmount)
    {
        _CurrentHealth = Mathf.Max(_CurrentHealth - damageAmount, 0);
        _HealthBar.SetHealth(_CurrentHealth);
        if (_CurrentHealth <= 0)
        {
            Die();
        }
    }
    #endregion
    
    #region HEALING
    /// <summary>
    /// Heals the entity by the specified amount, without exceeding maximum health.
    /// </summary>
    /// <param name="amount">The amount of health to restore.</param>
    public void Heal(int amount)
    {
        _CurrentHealth = Mathf.Min(_CurrentHealth + amount, _MaxHealth);
        _HealthBar.SetHealth(_CurrentHealth);
    }
    #endregion
    
    #region DEATH
    /// <summary>
    /// Handles the death behavior of the entity. If the entity is a player, disables player control,
    /// plays the death animation, triggers the game over state, and freezes any attached Rigidbody.
    /// Otherwise (e.g. for enemies), destroys the GameObject.
    /// </summary>
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
        {
            Destroy(gameObject);
        }
    }
    #endregion
}