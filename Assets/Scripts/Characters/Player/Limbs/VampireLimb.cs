/// <summary>
/// Extends the Limb class with Vampire-specific hit behaviour.
/// Handles life steal, enemy knock-ups, and temporarily
/// freezing enemies when the Vampire's attacks successfully hit a target.
/// </summary>
public class VampireLimb : Limb
{
    #region Variables
    // Reference to the Vampire's health system used for life steal.
    private PlayerHealth _playerHealth;

    // Reference to the Vampire character used to
    // access attack states and Vampire-specific abilities.
    private Vampire _vampire;
    #endregion

    private void Awake()
    {
        _playerHealth = GetComponentInParent<PlayerHealth>();
        _vampire = GetComponentInParent<Vampire>();
    }

    /// <summary>
    /// Handles additional hit behaviour specific to the Vampire.
    /// Restores health through life steal and
    /// applies special effects based on the current attack state.
    /// </summary>
    protected override void CharacterHit()
    {
        if (_vampire != null && _playerHealth != null)
        {
            // Restore health based on the recovery data of the current attack.
            _playerHealth.Heal(_vampire.CurrentRecoveryData.HealAmount);

            if (_vampire.IsAscending && _enemyRB != null)
            {
                // Launch the enemy upward during the Ascension attack.
                _vampire.PerformUppercut(_enemyRB);
            }

            if (_vampire.IsAirAttacking && _enemyRB != null)
            {
                // Temporarily freeze the enemy during an air combo.
                _vampire.StartCoroutine(_vampire.EnableStatic(_enemyRB));
            }
        }
    }
}