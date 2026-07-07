using UnityEngine;

public class PlayerHealth : Health
{
    #region COMPONENTS
    private Player _player;
    private PlayerAnimation _playerAnimation;
    #endregion
    
    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }
    
    #region HEALING
    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _MaxHealth);
        _HealthBar.SetHealth(_currentHealth);
    }
    #endregion
    
    #region DEATH
    protected override void Die()
    {
        _player.enabled = false;
        _playerAnimation.PlayDeathAnimation();
        GameManager.Instance.GameOver();
        if (TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }
    #endregion
}