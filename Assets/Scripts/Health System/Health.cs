using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("HEALTH BAR")]
    [SerializeField] protected HealthBar _HealthBar;
    
    #region Health Stats
    [Header("HEALTH STATS")]
    [SerializeField] protected CharacterSO _CharacterOS;
    protected int _currentHealth;
    public int MaxHealth => _CharacterOS.MaxHealth;
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
        _currentHealth = _CharacterOS.MaxHealth;
        _HealthBar.SetMaxHealth(_CharacterOS.MaxHealth);
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