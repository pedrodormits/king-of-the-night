using UnityEngine;

public class PlayerHealth : Health
{
    #region Variables
    private Player _player;
    private PlayerAnimation _playerAnimation;
    #endregion
    
    private void Awake()
    {
        _player = GetComponent<Player>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }
    
    #region Healing
    public void Heal(int amount)
    {
        _CurrentHealth = Mathf.Min(
            _CurrentHealth +
            amount,
            _CharacterOS.MaxHealth);
        
        _HealthBar.SetHealth(_CurrentHealth);
    }
    #endregion
    
    #region Death
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