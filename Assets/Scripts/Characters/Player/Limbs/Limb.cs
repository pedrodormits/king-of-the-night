using UnityEngine;

/// <summary>
/// Handles hit detection for a character's attack limbs.
/// Limb applies attack or ultimate damage when it hits a damageable
/// target and can trigger the corresponding impact audio and particle effects.
/// </summary>
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
    // Particle effect played when the limb hits a target.
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

    /// <summary>
    /// Assigns the attack data that will be used when this limb hits a target.
    /// </summary>
    public void SetAttackData(PlayerAttackSO attackData)
    {
        _playerAttackData = attackData;
    }

    /// <summary>
    /// Assigns the ultimate data that will be used when this limb
    /// hits a target during an ultimate attack.
    /// </summary>
    public void SetUltimateData(UltimateSO ultimateData)
    {
        _ultimateData = ultimateData;   
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            // Apply the damage from the current regular attack.
            if (_playerAttackData != null)
            {
                damageable.TakeDamage(_playerAttackData.Damage);
            }

            // Apply the damage from the current ultimate attack.
            if (_ultimateData != null)
            {
                damageable.TakeDamage(_ultimateData.Damage);
            }
        }
        
        _enemyRB = other.GetComponent<Rigidbody>();
        AudioSource audioSource = GetComponentInParent<AudioSource>();
        if (audioSource != null && _lightAttackImpact != null)
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

        // Allow derived classes to perform additional hit behaviour.
        CharacterHit();
    }
    
    /// <summary>
    /// Provides a method for derived limb classes to add
    /// character-specific behaviour when a hit occurs.
    /// </summary>
    protected virtual void CharacterHit(){}
}