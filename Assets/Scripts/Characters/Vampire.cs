using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        base.Awake();
        DefineRecovery();
    }

    protected override void Move()
    {
        if (!_isDrifting)
        {
            base.Move();
        }
    }

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

    private void DefineRecovery()
    {
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
        IsAirAttacking = true;
        CurrentRecoveryData = _recoveryDict["AirLightRecovery"];
        yield return base.PerformAirComboAttack();
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
        IsAscending = true;
        CurrentRecoveryData = _recoveryDict["GroundHeavyRecovery"];
        _limbsDict["RightHand"].GetComponent<Limb>().SetAttackData(_groundAttacksDict["GroundHeavy"]);
        yield return base.GroundHeavyAttack();
        IsAscending = false;
    }

    public void PerformAscension() => _rb.AddForce(Vector3.up * _PlayerSO.JumpForce, ForceMode.Impulse);

    public void PerformUppercut(Rigidbody enemy) => enemy.AddForce(Vector3.up * _UppercutForce, ForceMode.Impulse);
    #endregion
    
    #region Air Heavy Attack
    protected override IEnumerator AirHeavyAttack() 
    {
        CurrentRecoveryData = _recoveryDict["AirHeavyRecovery"];
        _limbsDict["Feet"].GetComponent<Limb>().SetAttackData(_airAttacksDict["AirHeavy"]);
        yield return base.AirHeavyAttack();
    }

    public void StartPiercer() => StartCoroutine(PerformPiercer());

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
    protected override IEnumerator SpecialAbility1() 
    {
        _isDrifting = true;
        yield return base.SpecialAbility1();
        _isDrifting = false;
    }

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
    public void StartSummon() 
    {
        _playerParticle.CurrentParticle.transform.SetPositionAndRotation(
            _BatSpawnPoint.position,
            _BatSpawnPoint.rotation);
        
        _playerParticle.PlayParticle();
    } 

    public void SendBat()
    {
        _playerParticle.StopParticle();
        ObjectPooler.Instance.SpawnFromPool("Bat", _BatSpawnPoint.position, _BatSpawnPoint.rotation);
    }
    #endregion
    
    #region Special Ability 3
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
    
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _StunRange);
    }
    #endregion
    
    protected override IEnumerator Ultimate() 
    {
        CurrentRecoveryData = _recoveryDict["UltimateRecovery"];
        _limbsDict["RightHand"].GetComponent<Limb>().SetUltimateData(_PlayerSO.Ultimate);
        yield return base.Ultimate();
    }
    
    protected override void OnCollisionStay(Collision collision) 
    {
        base.OnCollisionStay(collision);
        if (collision.gameObject.CompareTag("Ground")) 
        {
            _canDoubleJump = true;
        }
    }
}