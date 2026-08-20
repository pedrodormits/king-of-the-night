using UnityEngine;

/// <summary>
/// Health manages the health points of a character and handles taking damage.
/// It also updates the HealthBar and triggers the death behaviour when health
/// reaches zero.
/// </summary>
public class Health : MonoBehaviour, IDamageable
{
    #region Variables
    [Header("Health Bar")]
    // Reference to the UI health bar used to display the current health.
    [SerializeField] protected HealthBar _HealthBar;

    [Header("Health Stats")]
    // Scriptable Object containing the character's health information.
    [SerializeField] protected CharacterSO _CharacterOS;
    
    // Stores the character's current health.
    [SerializeField] protected int _CurrentHealth;
    
    // Returns the maximum health defined in the CharacterSO.
    public int MaxHealth => _CharacterOS.MaxHealth;
    
    // Returns the character's current health.
    public int CurrentHealth => _CurrentHealth;
    #endregion
    
    // Initialize the character's health when the object starts.
    protected virtual void Start() => InitializeHealth();

    private void Update()
    {
        // Press T to test the damage system by dealing 20 damage.
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(20);
        }
    }

    /// <summary>
    /// Sets the character's health to its maximum value.
    /// It also initializes the health bar so it starts completely full.
    /// </summary>
    protected virtual void InitializeHealth()
    {
        // Set the current health to the maximum health.
        _CurrentHealth = _CharacterOS.MaxHealth;
        
        // Set the health bar to match the maximum health.
        _HealthBar.SetMaxHealth(_CharacterOS.MaxHealth);
    }
    
    #region Damage
    /// <summary>
    /// Reduces the character's health by the specified damage amount.
    /// </summary>
    public void TakeDamage(int damageAmount)
    {
        // Subtract the incoming damage from the current health.
        // Mathf.Max prevents the health from going below zero.
        _CurrentHealth = Mathf.Max(_CurrentHealth - damageAmount, 0);
        
        // Update the health bar to display the new health value.
        _HealthBar.SetHealth(_CurrentHealth);
        
        // Check if the character has no health remaining.
        if (_CurrentHealth <= 0)
        {
            Die();
        }
    }
    #endregion

    /// <summary>
    /// Called when the character's health reaches zero.
    /// This method is virtual so child classes can override it and add their
    /// own death behaviour.
    /// </summary>
    protected virtual void Die(){}
}