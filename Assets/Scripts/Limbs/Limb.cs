using UnityEngine;

public class Limb : MonoBehaviour
{
    #region VARIABLES
    [Header("CHARACTER")]
    protected PlayerAttackData _playerAttackData;
    protected UltimateData _ultimateData;
    protected Rigidbody _enemyRB;
    #endregion
    
    [Header("AUDIO")] [SerializeField] protected AudioClip _lightAttackImpact;
    
    [Header("PARTICLES")] [SerializeField] protected ParticleSystem _lightAttackParticle;
    
    public void SetAttackData(PlayerAttackData attackData) => _playerAttackData = attackData;
    
    public void SetUltimateData(UltimateData ultimateData) => _ultimateData = ultimateData;
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            if (_playerAttackData != null)
            {
                damageable.TakeDamage(_playerAttackData.Damage);
            }

            if (_ultimateData != null)
            {
                damageable.TakeDamage(_ultimateData.Damage);
            }
        }
        
        _enemyRB = other.GetComponent<Rigidbody>();
        AudioSource audioSource = GetComponentInParent<AudioSource>();
        if (audioSource != null)
        {
            if (_lightAttackImpact != null)
            {
                audioSource.PlayOneShot(_lightAttackImpact);
            }
        }

        if (audioSource != null)
        {
            audioSource.PlayOneShot(_lightAttackImpact);
        }

        if (_lightAttackParticle != null)
        {
            _lightAttackParticle.transform.SetPositionAndRotation(transform.position, transform.rotation);
            _lightAttackParticle.Play();
        }
        
        UltimateAttack ultimateAttack = GetComponentInParent<UltimateAttack>();
        if (ultimateAttack != null)
        {
            if (_playerAttackData != null)
            {
                ultimateAttack.PrepareUltimate(_playerAttackData.UltPoints);
            }
        }

        CharacterHit();
    }
    
    protected virtual void CharacterHit(){}
}