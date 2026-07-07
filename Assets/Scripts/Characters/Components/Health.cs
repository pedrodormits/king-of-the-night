using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("HEALTH BAR")]
    [SerializeField] protected HealthBar _HealthBar;
    
    #region HEALTH STATS
    [SerializeField] protected int _MaxHealth;
    protected int _currentHealth;
    public int MaxHealth => _MaxHealth;
    public int CurrentHealth => _currentHealth;
    #endregion

    protected virtual void Start() => InitializeHealth();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(20);
        }
    }

    protected virtual void InitializeHealth()
    {
        _currentHealth = _MaxHealth;
        _HealthBar.SetMaxHealth(_MaxHealth);
    }

    #region DAMAGE
    public void TakeDamage(int damageAmount)
    {
        _currentHealth = Mathf.Max(_currentHealth - damageAmount, 0);
        _HealthBar.SetHealth(_currentHealth);
        if (_currentHealth <= 0)
        {
            Die();
        }
    }
    #endregion
    
    protected virtual void Die()
    {
        
    }
}