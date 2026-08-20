using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the Vampire character and its unique abilities.
/// Handles movement, jumping, attacks, special
/// abilities, the ultimate attack, and the life steal system.
/// </summary>
public class Vampire : Player
{
    #region Variables
    private bool _canDoubleJump;

    [Header("Air Combo")]
    [HideInInspector] public bool IsAirAttacking;

    // Duration for which an enemy remains frozen during an air combo.
    [SerializeField] private float _StaticTime;

    [Header("Midnight Ascension")]
    // Indicates whether the Vampire is currently performing Midnight Ascension.
    [HideInInspector] public bool IsAscending;

    // Upward force applied to enemies hit by the Ascension attack.
    [SerializeField] private float _UppercutForce;

    [Header("Crimson Piercer")]
    [SerializeField] private float _DiveSpeed;

    [Header("Shadow Flit")]
    // Horizontal movement speed used during Shadow Flit.
    [SerializeField] private float _DriftSpeed;

    // Prevents normal movement while Shadow Flit is active.
    private bool _isDrifting;

    [Header("Umbral Seeker")]
    [SerializeField] private Transform _BatSpawnPoint;

    [Header("Crimson Gaze")]
    [SerializeField] private float _StunRange;
    [SerializeField] private float _StunDuration;

    [Header("Life Steal")]
    // Recovery data used by the Vampire's current attack.
    [HideInInspector] public RecoverySO CurrentRecoveryData;

    // List containing the recovery data for each attack type.
    [SerializeField] private List<RecoverySO> _RecoveryDatas;

    // Stores recovery data using attack names as keys.
    private Dictionary<string, RecoverySO> _recoveryDict = new();
    #endregion
    
    protected override void Awake()
    {
        base.Awake();
        DefineRecovery();
    }

    private void Start()
    {
        if (_BatSpawnPoint == null)
        {
            Debug.Log("BatSpawnPoint not set");
        }

        if (_RecoveryDatas == null)
        {
            Debug.Log("RecoveryDatas not set");
        }
    }

    /// <summary>
    /// Handles the Vampire's movement.
    /// Normal movement is disabled while Shadow Flit is active.
    /// </summary>
    protected override void Move()
    {
        if (!_isDrifting)
        {
            base.Move();
        }
    }

    /// <summary>
    /// Handles the Vampire's jump input.
    /// Allows the Vampire to perform an additional jump while airborne.
    /// </summary>
    protected override void ExecuteJump()
    {
        if (_isAttacking || !Input.GetButtonDown("Jump"))
        {
            return;
        }

        if (IsGrounded)
        {
            Jump();
        }
        else if (_canDoubleJump)
        {
            Jump();
            _canDoubleJump = false;
        }
    }

    /// <summary>
    /// Creates a dictionary containing the recovery data for each attack type.
    /// The dictionary allows the
    /// appropriate recovery data to be accessed by name.
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
    /// Sets the recovery data for the Vampire's
    /// ground light attack before executing the base ground combo.
    /// </summary>
    protected override IEnumerator PerformGroundComboAttack()
    {
        CurrentRecoveryData = _recoveryDict["GroundLightRecovery"];
        yield return base.PerformGroundComboAttack();
    }
    #region Air Combo

    /// <summary>
    /// Performs the Vampire's air combo.
    /// Sets the appropriate recovery data and enables
    /// the air attack state while the combo is being performed.
    /// </summary>
    protected override IEnumerator PerformAirComboAttack()
    {
        IsAirAttacking = true;
        CurrentRecoveryData = _recoveryDict["AirLightRecovery"];
        yield return base.PerformAirComboAttack();
        IsAirAttacking = false;
    }

    /// <summary>
    /// Temporarily freezes an enemy by disabling its physics.
    /// This allows the Vampire to keep enemies suspended during an air combo.
    /// </summary>
    public IEnumerator EnableStatic(Rigidbody enemy)
    {
        // Disable physics so the enemy remains in place.
        enemy.isKinematic = true;

        // Keep the enemy frozen for the configured duration.
        yield return new WaitForSeconds(_StaticTime);

        // Restore the enemy's normal physics behaviour.
        enemy.isKinematic = false;
    }
    #endregion

    #region Ground Heavy Attack
    /// <summary>
    /// Performs the Vampire's ground heavy attack.
    /// Enables the Ascension state and
    /// assigns the correct attack data to the limb.
    /// </summary>
    protected override IEnumerator GroundHeavyAttack()
    {
        // Enable the Ascension state for Vampire-specific hit behaviour.
        IsAscending = true;
        
        CurrentRecoveryData = _recoveryDict["GroundHeavyRecovery"];

        // Assign the ground heavy attack data to the Vampire's right hand.
        _limbsDict["RightHand"].GetComponent<Limb>().SetAttackData(
            _groundAttacksDict["GroundHeavy"]);

        yield return base.GroundHeavyAttack();
        IsAscending = false;
    }

    /// <summary>
    /// Launches the Vampire upward as part of Midnight Ascension.
    /// </summary>
    public void PerformAscension()
    {
        _rb.AddForce(Vector3.up * _CharacterSO.JumpForce, ForceMode.Impulse);
    }

    /// <summary>
    /// Launches an enemy upward when hit by Midnight Ascension.
    /// </summary>
    public void PerformUppercut(Rigidbody enemy)
    {
        enemy.AddForce(Vector3.up * _UppercutForce, ForceMode.Impulse);
    }
    #endregion

    #region Air Heavy Attack
    /// <summary>
    /// Performs the Vampire's air heavy attack.
    /// Assigns the correct attack data to the feet used by the attack.
    /// </summary>
    protected override IEnumerator AirHeavyAttack()
    {
        CurrentRecoveryData = _recoveryDict["AirHeavyRecovery"];

        // Assign the air heavy attack data to the Vampire's feet.
        _limbsDict["Feet"].GetComponent<Limb>().SetAttackData(
            _airAttacksDict["AirHeavy"]);

        yield return base.AirHeavyAttack();
    }

    /// <summary>
    /// Starts the Crimson Piercer attack.
    /// </summary>
    public void StartPiercer() => StartCoroutine(PerformPiercer());

    /// <summary>
    /// Moves the Vampire diagonally downward during Crimson Piercer.
    /// Gravity is disabled so the dive
    /// can be controlled using the configured speed.
    /// </summary>
    private IEnumerator PerformPiercer()
    {
        // Disable gravity so the dive movement can be controlled manually.
        _rb.useGravity = false;

        while (!IsGrounded)
        {
            // Combine forward and
            // downward movement to create the dive direction.
            Vector3 diveDirection = (
                transform.forward +
                Vector3.down).normalized;

            // Move the Vampire in the calculated dive direction.
            _rb.MovePosition(
                _rb.position +
                diveDirection *
                _DiveSpeed *
                Time.fixedDeltaTime);

            // Wait for the next physics update before moving again.
            yield return new WaitForFixedUpdate();
        }

        // Restore gravity after the Vampire reaches the ground.
        _rb.useGravity = true;
    }
    #endregion

    #region Special Ability 1
    /// <summary>
    /// Performs Shadow Flit while temporarily disabling normal movement.
    /// </summary>
    protected override IEnumerator SpecialAbility1()
    {
        // Disable normal movement while Shadow Flit is active.
        _isDrifting = true;

        yield return base.SpecialAbility1();

        // Restore normal movement after the ability has finished.
        _isDrifting = false;
    }

    /// <summary>
    /// Moves the Vampire forward during Shadow Flit.
    /// Preserves the current vertical velocity while changing horizontal movement.
    /// </summary>
    public void PerformDrift()
    {
        _playerParticle.CurrentParticle.transform.SetPositionAndRotation(
            transform.position,
            transform.rotation);

        _playerParticle.PlayParticle();
        Vector3 driftDirection = transform.forward;

        // Apply horizontal drift movement while preserving vertical velocity.
        _rb.linearVelocity = new Vector3(
            driftDirection.x * _DriftSpeed,
            _rb.linearVelocity.y,
            driftDirection.z * _DriftSpeed);
    }
    #endregion

    #region Special Ability 2
    /// <summary>
    /// Starts the Umbral Seeker summon effect at the bat spawn point.
    /// </summary>
    public void StartSummon()
    {
        _playerParticle.CurrentParticle.transform.SetPositionAndRotation(
            _BatSpawnPoint.position,
            _BatSpawnPoint.rotation);

        _playerParticle.PlayParticle();
    }

    /// <summary>
    /// Stops the summon effect and spawns a bat using the object pool.
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
    /// Casts Crimson Gaze and stuns all enemies within the configured range.
    /// </summary>
    private void CastGaze()
    {
        _playerParticle.CurrentParticle.transform.SetPositionAndRotation(
            transform.position,
            transform.rotation);

        _playerParticle.PlayParticle();

        // Find all colliders within the stun range.
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
    /// Draws the Crimson Gaze stun range in
    /// the Unity Editor when the Vampire is selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            _StunRange);
    }

    #endregion

    /// <summary>
    /// Performs the Vampire's ultimate attack.
    /// Sets the ultimate recovery data and assigns
    /// the ultimate attack data to the Vampire's right hand.
    /// </summary>
    protected override IEnumerator Ultimate()
    {
        CurrentRecoveryData = _recoveryDict["UltimateRecovery"];

        // Assign the ultimate attack data to the Vampire's right hand.
        _limbsDict["RightHand"].GetComponent<Limb>().SetUltimateData(
            _PlayerSO.Ultimate);

        yield return base.Ultimate();
    }

    /// <summary>
    /// Restores the Vampire's ability to double jump after touching the ground.
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