using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    #region Variables
    private Animator _anim;
    private Player _player;
    private Rigidbody _rb;
    #endregion
    
    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _player = GetComponent<Player>();
        _rb = GetComponent<Rigidbody>();
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
    
    private void PlayIdleMoveAnimations()
    {
        Vector3 horizontalVelocity = new Vector3(
            _rb.linearVelocity.x,
            0f,
            _rb.linearVelocity.z);
        
        _anim.SetFloat("velocityMagnitude", horizontalVelocity.magnitude);
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

    public void PlayHurtAnimation() => _anim.SetTrigger("takeDamage");
    
    public void PlayDeathAnimation() => _anim.SetTrigger("die");
    #endregion
    
    #region Get Animation Lenght
    public float GetAnimationLength(string clipName)
    {
        foreach (var clip in _anim.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                return clip.length;
            }
        }
        
        return 0f;
    }
    #endregion
}