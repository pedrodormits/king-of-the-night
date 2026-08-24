public class PlayerAnimation : Animation
{
    private Player _player;

    private void Awake()
    {
        base.Awake();
        _player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        UpdateIsGrounded();
        PlayIdleMoveAnimations();
        PlayJumpFallAnimations();
    }
    
    #region Animation State Upadetes
    private void UpdateIsGrounded()
    {
        _anim.SetBool("isGrounded", _player.IsGrounded);
    } 
    
    private void PlayJumpFallAnimations()
    {
        _anim.SetFloat("verticalVelocity", _rb.linearVelocity.y);
    }
    #endregion

    #region Combo Index
    public void SetGroundComboIndex(int groundComboIndex)
    {
        _anim.SetInteger("groundComboIndex", groundComboIndex);
    }

    public void SetAirComboIndex(int airComboIndex)
    {
        _anim.SetInteger("airComboIndex", airComboIndex);
    } 
    #endregion

    #region Animation Triggers
    public void PlayHeavyAttackAnimation()
    {
        _anim.SetTrigger(_player.CurrentPlayerAttackData.AnimTrigger);
    }

    public void PlayAbilityAnimation()
    {
        _anim.SetTrigger(_player.CurrentPlayerAbilityData.AnimTrigger);
    }

    public void PlayUltimateAttackAnimation()
    {
        _anim.SetTrigger("UltimateAttack");
    }
    #endregion
}