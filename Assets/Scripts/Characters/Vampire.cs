using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Extends the base Player class with unique abilities such as double jumping,
/// aerial combo attacks, upward launches, dive attacks, drifting movement,
/// bat summoning, area stun, and life steal recovery mechanics.
/// </summary>
public class Vampire : Player
{
    #region Variables
    private bool _canDoubleJump;
    
    [Header("Air Combo")]
    [HideInInspector] public bool IsAirAttacking;
    [SerializeField] private float _StaticTime;
    
    [Header("Midnight Ascension")]
    [HideInInspector] public bool IsAscending;
    [SerializeField] private float _UppercutForce;

    [Header("Velvet Piercer")]
    [SerializeField] private float _DiveSpeed;
    
    [Header("Shadow Flit")]
    [SerializeField] private float _DriftSpeed;
    private bool _isDrifting;
    
    [Header("Umbral Seeker")]
    [SerializeField] private Transform _BatSpawnPoint;
    
    [Header("Crimsom Gaze")]
    [SerializeField] private float _StunRange;
    [SerializeField] private float _StunDuration;
    
    [Header("Life Steal")]
    [HideInInspector] public RecoverySO CurrentRecoveryData;
    [SerializeField] private List <RecoverySO> _RecoveryDatas;
    private Dictionary<string, RecoverySO> _recoveryDict = new();
    #endregion
    
    protected override void Awake()
    {
        // Initialize the base Player functionality first.
        base.Awake();
        
        // Creates the recovery lookup dictionary for Vampire attacks.
        DefineRecovery();
    }

    protected override void Move()
    {
        // Disable normal movement while the Vampire is performing a drift movement.
        if (!_isDrifting)
        {
            base.Move();
        }
    }

    protected override void ExecuteJump() 
    {
        // Prevent jumping while attacking or when the jump button is not pressed.
        if (_isAttacking || !Input.GetButtonDown("Jump"))
        {
            return;
        }

        // Perform a normal jump when grounded.
        if (IsGrounded)
        {
            Jump();
        }
        
        // Allow one additional jump while airborne.
        else if (_canDoubleJump) 
        {
            Jump();
            _canDoubleJump = false;
        }
    }

    /// <summary>
    /// Populates the recovery lookup table for each attack type.
    /// </summary>
    private void DefineRecovery()
    {
        // Store recovery data using attack names as keys.
        // This allows attacks to quickly retrieve their recovery values.
        _recoveryDict.Add("GroundLightRecovery", _RecoveryDatas[0]);
        _recoveryDict.Add("AirLightRecovery", _RecoveryDatas[1]);
        _recoveryDict.Add("GroundHeavyRecovery", _RecoveryDatas[2]);
        _recoveryDict.Add("AirHeavyRecovery", _RecoveryDatas[3]);
        _recoveryDict.Add("UltimateRecovery", _RecoveryDatas[4]);
    }

    protected override IEnumerator PerformGroundComboAttack() 
    {
        CurrentRecoveryData = _recoveryDict["GroundLightRecovery"];
        yield return base.PerformGroundComboAttack();
    }
    
    #region Air Combo
    protected override IEnumerator PerformAirComboAttack()
    {
        // Enable the air attack state while the combo is being performed.
        IsAirAttacking = true;
        
        // Set the recovery data used for the aerial light attack.
        CurrentRecoveryData = _recoveryDict["AirLightRecovery"];
        yield return base.PerformAirComboAttack();
        
        // Disable the air attack state after the attack finishes.
        IsAirAttacking = false;
    }
    
    public IEnumerator EnableStatic(Rigidbody enemy)
    {
        // Temporarily disable physics so the enemy remains frozen in place.
        enemy.isKinematic = true;
        
        // Keep the enemy frozen for the configured duration.
        yield return new WaitForSeconds(_StaticTime);
        
        // Restore normal physics behaviour.
        enemy.isKinematic = false;
    }
    #endregion
    
    #region Ground Heavy Attack
    protected override IEnumerator GroundHeavyAttack() 
    {
        // Enable the ascending state for the animation and ability logic.
        IsAscending = true;
        
        // Assign recovery data for this specific attack.
        CurrentRecoveryData = _recoveryDict["GroundHeavyRecovery"];
        
        // Apply the correct attack data to the attacking limb.
        _limbsDict["RightHand"].GetComponent<Limb>().SetAttackData(_groundAttacksDict["GroundHeavy"]);
        yield return base.GroundHeavyAttack();
        
        // Disable ascending state after the attack finishes.
        IsAscending = false;
    }

    /// <summary>
    /// Applies an upward impulse to launch the Vampire into the air.
    /// Used during the Ground Heavy Attack.
    /// </summary>
    public void PerformAscension() => _rb.AddForce(Vector3.up * _PlayerSO.JumpForce, ForceMode.Impulse);

    /// <summary>
    /// Launches the targeted enemy upward with an uppercut force.
    /// </summary>
    public void PerformUppercut(Rigidbody enemy) => enemy.AddForce(Vector3.up * _UppercutForce, ForceMode.Impulse);
    #endregion
    
    #region Air Heavy Attack
    protected override IEnumerator AirHeavyAttack() 
    {
        CurrentRecoveryData = _recoveryDict["AirHeavyRecovery"];
        _limbsDict["Feet"].GetComponent<Limb>().SetAttackData(_airAttacksDict["AirHeavy"]);
        yield return base.AirHeavyAttack();
    }

    /// <summary>
    /// Starts the dive attack coroutine.
    /// </summary>
    public void StartPiercer() => StartCoroutine(PerformPiercer());

    /// <summary>
    /// Drives the Vampire diagonally downward until the ground is reached,
    /// temporarily disabling gravity during the dive.
    /// </summary>
    private IEnumerator PerformPiercer() 
    {
        // Disable gravity to allow controlled downward movement.
        _rb.useGravity = false;
        while (!IsGrounded) 
        {
            // Combine forward movement with downward movement for the dive direction.
            Vector3 diveDirection = (transform.forward + Vector3.down).normalized;
            
            // Move the Vampire manually during the dive.
            _rb.MovePosition(_rb.position + diveDirection * _DiveSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        
        // Restore gravity after landing.
        _rb.useGravity = true;
    }
    #endregion

    #region Special Ability 1
    protected override IEnumerator SpecialAbility1() 
    {
        // Prevent normal movement while the drift ability is active.
        _isDrifting = true;
        yield return base.SpecialAbility1();
        
        // Restore normal movement after the ability ends.
        _isDrifting = false;
    }

    /// <summary>
    /// Performs a forward drifting movement while playing the associated particle effect.
    /// </summary>
    public void PerformDrift() 
    {
        // Move the particle effect to the Vampire's current position.
        _playerParticle.CurrentParticle.transform.SetPositionAndRotation(transform.position, transform.rotation);
        
        // Play the drift visual effect.
        _playerParticle.PlayParticle();
        
        // Apply forward movement while keeping the current vertical velocity.
        Vector3 driftDirection = transform.forward;
        
        _rb.linearVelocity = new Vector3(
            driftDirection.x * _DriftSpeed,
            _rb.linearVelocity.y,
            driftDirection.z * _DriftSpeed);
    }
    #endregion

    #region Special Ability 2
    /// <summary>
    /// Starts the bat summoning visual effect at the spawn point.
    /// </summary>
    public void StartSummon() 
    {
        // Move the summon particle effect to the bat spawn location.
        _playerParticle.CurrentParticle.transform.SetPositionAndRotation(
            _BatSpawnPoint.position,
            _BatSpawnPoint.rotation);
        
        // Start the summoning visual effect.
        _playerParticle.PlayParticle();
    } 

    /// <summary>
    /// Stops the summoning effect and spawns a bat projectile from the object pool.
    /// </summary>
    public void SendBat()
    {
        // Stop the summoning effect before launching the projectile.
        _playerParticle.StopParticle();
        
        // Spawn a bat projectile using the object pool instead of Instantiate.
        ObjectPooler.Instance.SpawnFromPool("Bat", _BatSpawnPoint.position, _BatSpawnPoint.rotation);
    }
    #endregion
    
    #region Special Ability 3
    /// <summary>
    /// Stuns all enemies within range and plays the associated particle effect.
    /// </summary>
    private void CastGaze() 
    {
        // Move and play the stun ability particle effect.
        _playerParticle.CurrentParticle.transform.SetPositionAndRotation(transform.position, transform.rotation);
        _playerParticle.PlayParticle();
        
        // Detect all objects inside the stun radius.
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _StunRange);
        foreach (var hit in hitColliders) 
        {
            // Apply stun only to enemies found inside the area.
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Stun(_StunDuration);
            }
        }
    }
    
    private void OnDrawGizmosSelected() 
    {
        // Draw the stun radius in the editor for easier ability balancing.
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _StunRange);
    }
    #endregion
    
    protected override IEnumerator Ultimate() 
    {
        // Assign recovery data for the ultimate ability.
        CurrentRecoveryData = _recoveryDict["UltimateRecovery"];
        
        // Apply ultimate attack data to the correct limb.
        _limbsDict["RightHand"].GetComponent<Limb>().SetUltimateData(_PlayerSO.Ultimate);
        yield return base.Ultimate();
    }
    
    protected override void OnCollisionStay(Collision collision) 
    {
        // Keep the original Player collision behaviour.
        base.OnCollisionStay(collision);
        
        // Allow double jumping again after touching the ground.
        if (collision.gameObject.CompareTag("Ground")) 
        {
            _canDoubleJump = true;
        }
    }
}