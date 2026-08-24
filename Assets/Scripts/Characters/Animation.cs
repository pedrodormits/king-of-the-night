using UnityEngine;

public class Animation : MonoBehaviour
{
    #region Variables
    protected Animator _anim;
    protected Rigidbody _rb;
    #endregion
    
    protected void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
    }
    
    protected void FixedUpdate() => PlayIdleMoveAnimations();
    
    protected void PlayIdleMoveAnimations()
    {
        Vector3 horizontalVelocity = new Vector3(
            _rb.linearVelocity.x,
            0f,
            _rb.linearVelocity.z);
        
        _anim.SetFloat("velocityMagnitude", horizontalVelocity.magnitude);
    }
    
    #region Animation Triggers
    public void PlayHurtAnimation() => _anim.SetTrigger("takeDamage");
    
    public void PlayStunAnimation() => _anim.SetTrigger("stun");
    
    public void PlayDeathAnimation() => _anim.SetTrigger("die");
    #endregion
    
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
}