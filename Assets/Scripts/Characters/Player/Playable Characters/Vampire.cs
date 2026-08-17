using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vampire is a player character class that extends the base Player class.
/// It adds vampire-specific abilities such as air combos, lifesteal, teleport
/// movement, bat summoning, enemy stunning, and special attacks.
/// </summary>
public class Vampire : Player
{
    #region Variables
    // Determines if the player can perform a second jump.
    private bool _canDoubleJump;
    
    [Header("Air Combo")]
    // Tracks if the player is currently performing an air attack.
    [HideInInspector] public bool IsAirAttacking;
    
    // Duration enemies remain frozen during the air combo.
    [SerializeField] private float _StaticTime;
    
    [Header("Midnight Ascension")]
    // Tracks if the vampire is performing an upward attack.
    [HideInInspector] public bool IsAscending;
    
    // Force applied when launching enemies upward.
    [SerializeField] private float _UppercutForce;

    [Header("Velvet Piercer")]
    // Speed of the downward dive attack.
    [SerializeField] private float _DiveSpeed;
    
    [Header("Shadow Flit")]
    // Movement speed during the drifting ability.
    [SerializeField] private float _DriftSpeed;
    
    // Prevents normal movement while drifting.
    private bool _isDrifting;
    
    [Header("Umbral Seeker")]
    // Location where bats are spawned.
    [SerializeField] private Transform _BatSpawnPoint;
    
    [Header("Crimsom Gaze")]
    // Range of the stun ability.
    [SerializeField] private float _StunRange;
    
    // Duration enemies remain stunned.
    [SerializeField] private float _StunDuration;
    
    [Header("Life Steal")]
    // Current recovery data used by attacks.
    [HideInInspector] public RecoverySO CurrentRecoveryData;
    
    // List containing recovery values for attacks.
    [SerializeField] private List <RecoverySO> _RecoveryDatas;
    
    // Stores recovery data using attack names as keys.
    private Dictionary<string, RecoverySO> _recoveryDict = new();
    #endregion
    
    protected override void Awake()
    {
        // Initialize the base Player class first.
        base.Awake();
        
        // Convert recovery data list into a dictionary for easier access.
        DefineRecovery();
    }

    protected override void Move()
    {
        // Disable normal movement while using Shadow Flit.
        if (!_isDrifting)
        {
            base.Move();
        }
    }
    
    /// <summary>
    /// Handles normal jumping and double jumping.
    /// </summary>
    protected override void ExecuteJump() 
    {
        // Prevent jumping while attacking or when jump input is not pressed.
        if (_isAttacking || !Input.GetButtonDown("Jump"))
        {
            return;
        }

        // Perform a normal jump when grounded.
        if (IsGrounded)
        {
            Jump();
        }
        
        // Perform a second jump if available.
        else if (_canDoubleJump)
        {
            Jump();
            _canDoubleJump = false;
        }
    }

    /// <summary>
    /// Stores recovery data using readable attack names.
    /// </summary>
    private void DefineRecovery()
    {
        _recoveryDict.Add("GroundLightRecovery", _RecoveryDatas[0]);
        _recoveryDict.Add("AirLightRecovery", _RecoveryDatas[1]);
        _recoveryDict.Add("GroundHeavyRecovery", _RecoveryDatas[2]);
        _recoveryDict.Add("AirHeavyRecovery", _RecoveryDatas[3]);
        _recoveryDict.Add("UltimateRecovery", _RecoveryDatas[4]);
    }

    /// <summary>
    /// Uses the correct recovery data for ground light attacks.
    /// </summary>
    protected override IEnumerator PerformGroundComboAttack() 
    {
        CurrentRecoveryData = _recoveryDict["GroundLightRecovery"];
        yield return base.PerformGroundComboAttack();
    }
    
    #region Air Combo
    /// <summary>
    /// Performs an aerial combo attack.
    /// </summary>
    protected override IEnumerator PerformAirComboAttack()
    {
        IsAirAttacking = true;
        CurrentRecoveryData = _recoveryDict["AirLightRecovery"];
        yield return base.PerformAirComboAttack();
        IsAirAttacking = false;
    }
    
    /// <summary>
    /// Freezes an enemy temporarily by disabling physics.
    /// </summary>
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
    /// <summary>
    /// Performs a heavy ground attack that launches the vampire upward.
    /// </summary>
    protected override IEnumerator GroundHeavyAttack() 
    {
        IsAscending = true;
        CurrentRecoveryData = _recoveryDict["GroundHeavyRecovery"];
        _limbsDict["RightHand"].GetComponent<Limb>().SetAttackData(
            _groundAttacksDict["GroundHeavy"]);
        
        yield return base.GroundHeavyAttack();
        IsAscending = false;
    }

    /// <summary>
    /// Pushes the vampire upward during Midnight Ascension.
    /// </summary>
    public void PerformAscension()
    {
        _rb.AddForce(Vector3.up * _CharacterSO.JumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Launches an enemy upward using an uppercut attack.
    /// </summary>
    public void PerformUppercut(Rigidbody enemy)
    {
        enemy.AddForce(Vector3.up * _UppercutForce, ForceMode.Impulse);
    }
    #endregion
    
    #region Air Heavy Attack
    /// <summary>
    /// Performs the aerial heavy attack.
    /// </summary>
    protected override IEnumerator AirHeavyAttack() 
    {
        CurrentRecoveryData = _recoveryDict["AirHeavyRecovery"];
        _limbsDict["Feet"].GetComponent<Limb>().SetAttackData(
            _airAttacksDict["AirHeavy"]);
        
        yield return base.AirHeavyAttack();
    }

    /// <summary>
    /// Starts the Velvet Piercer dive attack.
    /// </summary>
    public void StartPiercer() => StartCoroutine(PerformPiercer());

    /// <summary>
    /// Makes the vampire dive diagonally downward until hitting the ground.
    /// </summary>
    private IEnumerator PerformPiercer() 
    {
        _rb.useGravity = false;
        while (!IsGrounded) 
        {
            Vector3 diveDirection = (
                transform.forward +
                Vector3.down).normalized;
            
            _rb.MovePosition(
                _rb.position +
                diveDirection * 
                _DiveSpeed *
                Time.fixedDeltaTime);
            
            yield return new WaitForFixedUpdate();
        }
        
        _rb.useGravity = true;
    }
    #endregion

    #region Special Ability 1
    /// <summary>
    /// Activates the drifting movement ability.
    /// </summary>
    protected override IEnumerator SpecialAbility1() 
    {
        _isDrifting = true;
        yield return base.SpecialAbility1();
        _isDrifting = false;
    }

    /// <summary>
    /// Moves the vampire forward while drifting.
    /// </summary>
    public void PerformDrift() 
    {
        _playerParticle.CurrentParticle.transform.SetPositionAndRotation(
            transform.position,
            transform.rotation);
        
        _playerParticle.PlayParticle();
        Vector3 driftDirection = transform.forward;
        
        _rb.linearVelocity = new Vector3(
            driftDirection.x *
            _DriftSpeed,
            _rb.linearVelocity.y,
            driftDirection.z *
            _DriftSpeed);
    }
    #endregion

    #region Special Ability 2
    /// <summary>
    /// Shows the summon effect before spawning the bat.
    /// </summary>
    public void StartSummon() 
    {
        _playerParticle.CurrentParticle.transform.SetPositionAndRotation(
            _BatSpawnPoint.position,
            _BatSpawnPoint.rotation);
        
        _playerParticle.PlayParticle();
    } 

    /// <summary>
    /// Stops the summon effect and creates a bat projectile using the object pool.
    /// </summary>
    public void SendBat()
    {
        _playerParticle.StopParticle();
        ObjectPooler.Instance.SpawnFromPool(
            "Bat",
            _BatSpawnPoint.position,
            _BatSpawnPoint.rotation);
    }
    #endregion
    
    #region Special Ability 3
    /// <summary>
    /// Stuns all enemies inside the ability radius.
    /// </summary>
    private void CastGaze() 
    {
        _playerParticle.CurrentParticle.transform.SetPositionAndRotation(
            transform.position,
            transform.rotation);
        
        _playerParticle.PlayParticle();
        Collider[] hitColliders = Physics.OverlapSphere(
            transform.position,
            _StunRange);
        
        foreach (var hit in hitColliders) 
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Stun(_StunDuration);
            }
        }
    }
    
    /// <summary>
    /// Displays the stun range in the Unity editor.
    /// </summary>
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _StunRange);
    }
    #endregion
    
    /// <summary>
    /// Performs the vampire ultimate attack.
    /// </summary>
    protected override IEnumerator Ultimate() 
    {
        CurrentRecoveryData = _recoveryDict["UltimateRecovery"];
        _limbsDict["RightHand"].GetComponent<Limb>().SetUltimateData(
            _PlayerSO.Ultimate);
        
        yield return base.Ultimate();
    }
    
    /// <summary>
    /// Enables double jumping whenever the player touches the ground.
    /// </summary>
    protected override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);
        if (collision.gameObject.CompareTag("Ground")) 
        {
            _canDoubleJump = true;
        }
    }
}