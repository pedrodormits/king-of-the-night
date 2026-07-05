using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampire : Player
{
    // DOUBLE JUMP
    [Header("NIGHTFALL STEP")] private bool _canDoubleJump;
    
    #region AIR COMBO)
    [Header("AIR COMBO")]
    [SerializeField] public float _staticTime;
    [HideInInspector] public bool IsAirAttacking;
    #endregion
    
    #region HEAVY ATTACK (MIDNIGHT ASCENSION)
    [Header("MIDNIGHT ASCENSION")]
    [SerializeField] public float UppercutForce;
    [HideInInspector] public bool IsAscending;
    #endregion

    // AIR HEAVY ATTACK
    [Header("VELVET PIERCER")] [SerializeField] private float _diveSpeed;
    
    #region SPECIAL ABILITY 1 (SHADOW FLIT)
    [Header("SHADOW FLIT")]
    [SerializeField] private float _driftSpeed;
    private bool _isDrifting;
    #endregion
    
    #region SPECIAL ABILITY 2 (UMBRAL SEEKER)
    [Header("UMBRAL SEEKER")]
    [SerializeField] private Transform _batLauncher;
    [SerializeField] private GameObject _bat;
    #endregion
    
    #region SPECIAL ABILITY 3 (CRIMSON GAZE)
    [Header("CRIMSON GAZE")]
    [SerializeField] private float _stunRange;
    [SerializeField] private float _stunDuration;
    #endregion
    
    #region LIFE STEAL
    [Header("LIFE STEAL")]
    [SerializeField] private List <RecoveryData> _recoveryDatas;
    [HideInInspector] public RecoveryData CurrentRecoveryData;
    private Dictionary<string, RecoveryData> _recoveryDict = new();
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
        _recoveryDict.Add("GroundLightRecovery", _recoveryDatas[0]);
        _recoveryDict.Add("AirLightRecovery", _recoveryDatas[1]);
        _recoveryDict.Add("GroundHeavyRecovery", _recoveryDatas[2]);
        _recoveryDict.Add("AirHeavyRecovery", _recoveryDatas[3]);
        _recoveryDict.Add("UltimateRecovery", _recoveryDatas[4]);
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
        yield return new WaitForSeconds(_staticTime);
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

    public void PerformUppercut (Rigidbody enemy) => enemy.AddForce(Vector3.up * UppercutForce, ForceMode.Impulse);
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
            _rb.MovePosition(_rb.position + diveDirection * _diveSpeed * Time.fixedDeltaTime);
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
        _rb.linearVelocity = new Vector3(driftDirection.x * _driftSpeed,
            _rb.linearVelocity.y, driftDirection.z * _driftSpeed);
    }
    #endregion

    #region SPECIAL ABILITY 2 (UMBRAL SEEKER)
    public void StartSummon() 
    {
        _playerParticle.CurrentParticle.transform.SetPositionAndRotation(_batLauncher.position,_batLauncher.rotation);
        _playerParticle.PlayParticle();
    } 

    public void SendBat()
    {
        _playerParticle.StopParticle();
        Instantiate(_bat, _batLauncher.position, _batLauncher.rotation);   
    }
    #endregion
    
    #region SPECIAL ABILITY 3 (CRIMSON GAZE)
    private void CastGaze() 
    {
        _playerParticle.CurrentParticle.transform.SetPositionAndRotation(transform.position, transform.rotation);
        _playerParticle.PlayParticle();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _stunRange);
        foreach (var hit in hitColliders) 
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Stun(_stunDuration);
            }
        }
    }
    
    private void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _stunRange);
    }
    #endregion
    
    #region ULTIMATE ATTACK (VAMPIRE'S EMBRACE)
    protected override IEnumerator Ultimate() 
    {
        CurrentRecoveryData = _recoveryDict["UltimateRecovery"];
        _limbsDict["RightHand"].GetComponent<Limb>().SetUltimateData(_ultimate);
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