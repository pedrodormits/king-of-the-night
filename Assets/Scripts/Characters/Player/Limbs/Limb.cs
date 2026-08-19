using UnityEngine;

public class Limb : MonoBehaviour
{
    #region Variables
    [Header("Character")]
    protected PlayerAttackSO _playerAttackData;
    protected UltimateSO _ultimateData;
    protected Rigidbody _enemyRB;
    
    [Header("Audio")]
    [SerializeField] protected AudioClip _lightAttackImpact;
    
    [Header("Particles")]
    [SerializeField] protected ParticleSystem _lightAttackParticle;
    #endregion

    private void Start()
    {
        if (_lightAttackImpact == null)
        {
            Debug.Log("No audio clip assigned");
        }
        
        if (_lightAttackParticle == null)
        {
            Debug.Log("No particle assigned");
        }
    }

    public void SetAttackData(PlayerAttackSO attackData)
    {
        _playerAttackData = attackData;
    }

    public void SetUltimateData(UltimateSO ultimateData)
    {
        _ultimateData = ultimateData;   
    }
    
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
            _lightAttackParticle.transform.SetPositionAndRotation(
                transform.position,
                transform.rotation);
            
            _lightAttackParticle.Play();
        }
        
        UltimateAttack ultimateAttack = GetComponentInParent<UltimateAttack>();

        if (ultimateAttack != null && _playerAttackData != null)
        {
            ultimateAttack.PrepareUltimate(_playerAttackData.UltPoints);
        }

        CharacterHit();
    }
    
    protected virtual void CharacterHit(){}
}