using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Vampire is a player character class that extends the base Player class.
/// It adds vampire-specific abilities such as air combos, lifesteal, teleport movement,
/// bat summoning, enemy stunning, and special attacks.
/// </summary>
public class Vampire : Player
{
    #region Variables
    private bool _canDoubleJump; // Determines if the player can perform a second jump.
    
    [Header("Air Combo")]
    [HideInInspector] public bool IsAirAttacking; // Tracks if the player is currently performing an air attack.
    [SerializeField] private float _StaticTime; // Duration enemies remain frozen during the air combo.
    
    [Header("Midnight Ascension")]
    [HideInInspector] public bool IsAscending; // Tracks if the vampire is performing an upward attack.
    [SerializeField] private float _UppercutForce; // Force applied when launching enemies upward.

    [Header("Velvet Piercer")]
    [SerializeField] private float _DiveSpeed; // Speed of the downward dive attack.
    
    [Header("Shadow Flit")]
    [SerializeField] private float _DriftSpeed; // Movement speed during the drifting ability.
    private bool _isDrifting; // Prevents normal movement while drifting.
    
    [Header("Umbral Seeker")]
    [SerializeField] private Transform _BatSpawnPoint; // Location where bats are spawned.
    
    [Header("Crimsom Gaze")]
    [SerializeField] private float _StunRange; // Range of the stun ability.
    [SerializeField] private float _StunDuration; // Duration enemies remain stunned.
    
    [Header("Life Steal")]
    [HideInInspector] public RecoverySO CurrentRecoveryData; // Current recovery data used by attacks.
    [SerializeField] private List <RecoverySO> _RecoveryDatas; // List containing recovery values for attacks.
    private Dictionary<string, RecoverySO> _recoveryDict = new();  // Stores recovery data using attack names as keys.
    #endregion
    
    protected override void Awake()
    {
        base.Awake(); // Initialize the base Player class first.
        DefineRecovery(); // Convert recovery data list into a dictionary for easier access.
    }

    protected override void Move()
    {
        if (!_isDrifting) // Disable normal movement while using Shadow Flit.
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

        if (IsGrounded) // Perform a normal jump when grounded.
        {
            Jump();
        }
        else if (_canDoubleJump) // Perform a second jump if available.
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
        _limbsDict["RightHand"].GetComponent<Limb>().SetAttackData(_groundAttacksDict["GroundHeavy"]);
        yield return base.GroundHeavyAttack();
        IsAscending = false;
    }

    /// <summary>
    /// Pushes the vampire upward during Midnight Ascension.
    /// </summary>
    public void PerformAscension() => _rb.AddForce(Vector3.up * _PlayerSO.JumpForce, ForceMode.Impulse);

    /// <summary>
    /// Launches an enemy upward using an uppercut attack.
    /// </summary>
    public void PerformUppercut(Rigidbody enemy) => enemy.AddForce(Vector3.up * _UppercutForce, ForceMode.Impulse);
    #endregion
    
    #region Air Heavy Attack
    /// <summary>
    /// Performs the aerial heavy attack.
    /// </summary>
    protected override IEnumerator AirHeavyAttack() 
    {
        CurrentRecoveryData = _recoveryDict["AirHeavyRecovery"];
        _limbsDict["Feet"].GetComponent<Limb>().SetAttackData(_airAttacksDict["AirHeavy"]);
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
            Vector3 diveDirection = (transform.forward + Vector3.down).normalized;
            _rb.MovePosition(_rb.position + diveDirection * _DiveSpeed * Time.fixedDeltaTime);
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
        _playerParticle.CurrentParticle.transform.SetPositionAndRotation(transform.position, transform.rotation);
        _playerParticle.PlayParticle();
        Vector3 driftDirection = transform.forward;
        
        _rb.linearVelocity = new Vector3(
            driftDirection.x * _DriftSpeed,
            _rb.linearVelocity.y,
            driftDirection.z * _DriftSpeed);
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
        ObjectPooler.Instance.SpawnFromPool("Bat", _BatSpawnPoint.position, _BatSpawnPoint.rotation);
    }
    #endregion
    
    #region Special Ability 3
    /// <summary>
    /// Stuns all enemies inside the ability radius.
    /// </summary>
    private void CastGaze() 
    {
        _playerParticle.CurrentParticle.transform.SetPositionAndRotation(transform.position, transform.rotation);
        _playerParticle.PlayParticle();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _StunRange);
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
        _limbsDict["RightHand"].GetComponent<Limb>().SetUltimateData(_PlayerSO.Ultimate);
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