using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Player
{
    private bool _canDoubleJump;
    
    #region AIR COMBO)
    [Header("AIR COMBO")]
    [HideInInspector] public bool IsAirAttacking;
    [SerializeField] private float _StaticTime;
    #endregion
    
    #region HEAVY ATTACK (MIDNIGHT ASCENSION)
    [Header("MIDNIGHT ASCENSION")]
    [SerializeField] private float _UppercutForce;
    [HideInInspector] public bool IsAscending;
    #endregion

    [Header("VELVET PIERCER")]
    [SerializeField] private float _DiveSpeed;
    
    #region SPECIAL ABILITY 1 (SHADOW FLIT)
    [Header("SHADOW FLIT")]
    [SerializeField] private float _DriftSpeed;
    private bool _isDrifting;
    #endregion
    
    #region SPECIAL ABILITY 2 (UMBRAL SEEKER)
    [Header("UMBRAL SEEKER")]
    [SerializeField] private Transform _BatLauncher;
    [SerializeField] private GameObject _Bat;
    #endregion
    
    #region SPECIAL ABILITY 3 (CRIMSON GAZE)
    [Header("CRIMSON GAZE")]
    [SerializeField] private float _StunRange;
    [SerializeField] private float _StunDuration;
    #endregion
    
    #region LIFE STEAL
    [Header("LIFE STEAL")]
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

    #region DOUBLE JUMP (NIGHTFALL STEP)
    protected override void ExecuteJump() 
    {
        if (_isAttacking || !Input.GetButtonDown("Jump"))
        {
            return;
        }

        if (IsGrounded) Jump();
        else if (_canDoubleJump) 
        {
            Jump();
            _canDoubleJump = false;
        }
    }
    #endregion

    private void DefineRecovery() {
        _recoveryDict.Add("GroundLightRecovery", _RecoveryDatas[0]);
        _recoveryDict.Add("AirLightRecovery", _RecoveryDatas[1]);
        _recoveryDict.Add("GroundHeavyRecovery", _RecoveryDatas[2]);
        _recoveryDict.Add("AirHeavyRecovery", _RecoveryDatas[3]);
        _recoveryDict.Add("UltimateRecovery", _RecoveryDatas[4]);
    }

    #region COMBOS
    protected override IEnumerator PerformGroundComboAttack() 
    {
        CurrentRecoveryData = _recoveryDict["GroundLightRecovery"];
        yield return base.PerformGroundComboAttack();
    }
    
    protected override IEnumerator PerformAirComboAttack() {
        IsAirAttacking = true;
        CurrentRecoveryData = _recoveryDict["AirLightRecovery"];
        yield return base.PerformAirComboAttack();
        IsAirAttacking = false;
    }
    
    public IEnumerator EnableStatic(Rigidbody enemy)
    {
        enemy.isKinematic = true;
        yield return new WaitForSeconds(_StaticTime);
        enemy.isKinematic = false;
    }
    #endregion

    #region GROUND HEAVY ATTACK (MIDNIGHT ASCENSION)
    protected override IEnumerator GroundHeavyAttack() 
    {
        IsAscending = true;
        CurrentRecoveryData = _recoveryDict["GroundHeavyRecovery"];
        _limbsDict["RightHand"].GetComponent<Limb>().SetAttackData(_groundAttacksDict["GroundHeavy"]);
        yield return base.GroundHeavyAttack();
        IsAscending = false;
    }

    public void PerformAscension() => _rb.AddForce(Vector3.up * _PlayerSO.JumpForce, ForceMode.Impulse);

    public void PerformUppercut (Rigidbody enemy) => enemy.AddForce(Vector3.up * _UppercutForce, ForceMode.Impulse);
    #endregion
    
    #region AIR HEAVY ATTACK (VELVET PIERCER)
    protected override IEnumerator AirHeavyAttack() 
    {
        CurrentRecoveryData = _recoveryDict["AirHeavyRecovery"];
        _limbsDict["Feet"].GetComponent<Limb>().SetAttackData(_airAttacksDict["AirHeavy"]);
        yield return base.AirHeavyAttack();
    }

    public void StartPiercer() => StartCoroutine(PerformPiercer());

    public IEnumerator PerformPiercer() 
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

    #region SPECIAL ABILITY 1 (SHADOW FLIT)
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

    #region SPECIAL ABILITY 2 (UMBRAL SEEKER)
    public void StartSummon() 
    {
        _playerParticle.CurrentParticle.transform.SetPositionAndRotation(_BatLauncher.position,_BatLauncher.rotation);
        _playerParticle.PlayParticle();
    } 

    public void SendBat()
    {
        _playerParticle.StopParticle();
        Instantiate(_Bat, _BatLauncher.position, _BatLauncher.rotation);   
    }
    #endregion
    
    #region SPECIAL ABILITY 3 (CRIMSON GAZE)
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
    
    #region ULTIMATE ATTACK (VAMPIRE'S EMBRACE)
    protected override IEnumerator Ultimate() 
    {
        CurrentRecoveryData = _recoveryDict["UltimateRecovery"];
        _limbsDict["RightHand"].GetComponent<Limb>().SetUltimateData(_PlayerSO.Ultimate);
        yield return base.Ultimate();
    }
    #endregion

    #region COLLISION
    protected override void OnCollisionStay(Collision collision) 
    {
        base.OnCollisionStay(collision);
        if (collision.gameObject.CompareTag("Ground")) 
        {
            IsGrounded = true;
            _canDoubleJump = true;
        }
    }

    protected override void OnCollisionExit(Collision collision) 
    {
        base.OnCollisionExit(collision);
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }
    #endregion
}