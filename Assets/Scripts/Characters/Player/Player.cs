using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region COMPONENTS
    protected PlayerAnimation _playerAnim;
    protected PlayerInput _playerInput;
    protected PlayerParticle _playerParticle;
    protected Rigidbody _rb;
    protected UltimateAttack _ultimateAttack;
    #endregion

    #region MOVEMENT
    [Header("MOVEMENT")]
    [SerializeField] protected float _speed;
    [SerializeField] protected float _jumpForce;
    [HideInInspector] public bool IsGrounded;
    #endregion

    #region COMBO CONFIGURATION
    [Header("COMBO GROUND")]
    [SerializeField] protected GroundComboAttack[] _groundLightAttacks;
    protected int _currentGroundComboIndex = -1;
    
    [Header("COMBO AIR")]
    [SerializeField] protected AirComboAttack[] _airLightAttacks;
    protected int _currentAirComboIndex = -1;
    #endregion
    
    [Header("ANIMATION")] protected bool _isAttacking;

    #region LIMBS
    [Header("LIMBS")]
    [SerializeField] protected List<GameObject> _limbs;
    protected Dictionary<string, GameObject> _limbsDict = new();
    #endregion
    
    #region DATA
    [Header("GROUND DATA")]
    [SerializeField] protected List <PlayerAttackData> _groundAttacks;
    protected Dictionary<string, PlayerAttackData> _groundAttacksDict = new();
    
    [Header("AIR DATA")]
    [SerializeField] protected List <PlayerAttackData> _airAttacks;
    [HideInInspector] public PlayerAttackData CurrentPlayerAttackData;
    protected Dictionary<string, PlayerAttackData> _airAttacksDict = new();
    
    [Header("ABILITY DATA")]
    [SerializeField] protected List <PlayerAbilityData> _abilities;
    [HideInInspector] public PlayerAbilityData CurrentPlayerAbilityData;
    protected Dictionary<string, PlayerAbilityData> _abilitiesDict = new();
    
    [Header("ULTIMATE DATA")] [SerializeField] protected UltimateData _ultimate;
    #endregion
    
    protected virtual void Awake() 
    {
        _playerAnim = GetComponent<PlayerAnimation>();
        _playerInput = GetComponent<PlayerInput>();
        _playerParticle = GetComponent<PlayerParticle>();
        _rb = GetComponent<Rigidbody>();
        _ultimateAttack = GetComponent<UltimateAttack>();
        DefineLimbs();
        DefineGroundLightAttacks();
        DefineAirLightAttacks();
        DefineHeavyAttacks();
        DefineAbilities();
    }

    protected virtual void Update() 
    {
        ExecuteJump();
        ExecuteGroundLightAttack();
        ExecuteAirLightAttack();
        ExecuteGroundHeavyAttack();
        ExecuteAirHeavyAttack();
        ExecuteSpecialAbility1();
        ExecuteSpecialAbility2();
        ExecuteSpecialAbility3();
        ExecuteUltimateAttack();
    }

    #region MOVEMENT
    protected virtual void FixedUpdate() => Move();
    
    protected virtual void Move() 
    {
        if (!_isAttacking) 
        {
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            _rb.linearVelocity = new Vector3(input.x * _speed, _rb.linearVelocity.y, input.z * _speed);
            if (input.magnitude > 0) 
            {
                Quaternion targetRotation = Quaternion.LookRotation(input);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }
        else
            _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
    }
    #endregion

    #region JUMP
    protected virtual void ExecuteJump() 
    {
        if (!_isAttacking && _playerInput.Jump && IsGrounded)
            Jump();
    }
    
    protected virtual void Jump() 
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        IsGrounded = false;
    }
    #endregion

    #region DEFINE LIMBS
    protected virtual void DefineLimbs() 
    {
        _limbsDict.Add("RightHand", _limbs[0]);
        _limbsDict.Add("LeftHand", _limbs[1]);
        _limbsDict.Add("Feet", _limbs[2]);
    }
    #endregion
    
    #region LIMBS ACTIVATION AND DEACTIVATION
    public void EnableRightHand() => _limbsDict["RightHand"].SetActive(true);

    public void DisableRightHand() => _limbsDict["RightHand"].SetActive(false);

    public void EnableLeftHand() => _limbsDict["LeftHand"].SetActive(true);

    public void DisableLeftHand() => _limbsDict["LeftHand"].SetActive(false);

    public void EnableFeet() => _limbsDict["Feet"].SetActive(true);

    public void DisableFeet() => _limbsDict["Feet"].SetActive(false);
    #endregion

    #region DEFINE ATTACKTS
    protected virtual void DefineGroundLightAttacks() 
    {
        _groundAttacksDict.Add("GroundLight1", _groundAttacks[0]);
        _groundAttacksDict.Add("GroundLight2", _groundAttacks[1]);
        _groundAttacksDict.Add("GroundLight3", _groundAttacks[2]);
    }
    
    protected virtual void DefineAirLightAttacks() 
    {
        _airAttacksDict.Add("AirLight1", _airAttacks[0]);
        _airAttacksDict.Add("AirLight2", _airAttacks[1]);
        _airAttacksDict.Add("AirLight3", _airAttacks[2]);
    }
    
    protected virtual void DefineHeavyAttacks() 
    {
        _groundAttacksDict.Add("GroundHeavy", _groundAttacks[3]);
        _airAttacksDict.Add("AirHeavy", _airAttacks[3]);
    }
    #endregion
    
    #region GROUND LIGHT COMBO
    [System.Serializable]
    public class GroundComboAttack
    {
        public string AnimName;
    }

    protected virtual void ExecuteGroundLightAttack() 
    {
        if (IsGrounded && _playerInput.LightAttack && !_isAttacking)
            StartCoroutine(PerformGroundComboAttack());
    }

    protected virtual IEnumerator PerformGroundComboAttack() 
    {
        _isAttacking = true;
        _currentGroundComboIndex = 0;
        while (_currentGroundComboIndex < _groundLightAttacks.Length) 
        {
            GroundComboAttack attack = _groundLightAttacks[_currentGroundComboIndex];
            _playerAnim.SetGroundComboIndex(_currentGroundComboIndex);
            foreach (var limb in _limbs) 
            {
                string key = "GroundLight" + (_currentGroundComboIndex + 1);
                var limbScript = limb.GetComponent<Limb>();
                if (limbScript != null)
                    limbScript.SetAttackData(_groundAttacksDict[key]);
            }
            
            CurrentPlayerAttackData = _groundAttacksDict["GroundLight1"];
            float animLength = _playerAnim.GetAnimationLength(attack.AnimName);
            float bufferWindow = 0.5f;
            float preBuffer = Mathf.Max(0, animLength - bufferWindow);
            yield return new WaitForSeconds(preBuffer);
            bool queued = false;
            float timer = 0f;
            while (timer < bufferWindow) 
            {
                if (_playerInput.LightAttack)
                {
                    queued = true;
                    break;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            if (queued)
                _currentGroundComboIndex++;
            else
                break;
        }

        _isAttacking = false;
        _currentGroundComboIndex = -1;
        _playerAnim.SetGroundComboIndex(_currentGroundComboIndex);
    }
    #endregion
    
    #region AIR LIGHT COMBO
    [System.Serializable]
    public class AirComboAttack
    {
        public string AnimName;
    }

    protected virtual void ExecuteAirLightAttack() 
    {
        if (!IsGrounded && _playerInput.LightAttack && !_isAttacking)
            StartCoroutine(PerformAirComboAttack());
    }

    protected virtual IEnumerator PerformAirComboAttack() 
    {
        _isAttacking = true;
        _currentAirComboIndex = 0;
        while (_currentAirComboIndex < _airLightAttacks.Length) 
        {
            AirComboAttack attack = _airLightAttacks[_currentAirComboIndex];
            _rb.isKinematic = true;
            _playerAnim.SetAirComboIndex(_currentAirComboIndex);
            CurrentPlayerAttackData = _airAttacksDict["AirLight1"];
            foreach (var limb in _limbs) 
            {
                string key = "AirLight" + (_currentAirComboIndex + 1);
                var limbScript = limb.GetComponent<Limb>();
                if (limbScript != null) limbScript.SetAttackData(_airAttacksDict[key]);
            }
            
            float animLength = _playerAnim.GetAnimationLength(attack.AnimName);
            float bufferWindow = 0.5f;
            float preBuffer = Mathf.Max(0, animLength - bufferWindow);
            yield return new WaitForSeconds(preBuffer);
            bool queued = false;
            float timer = 0f;
            while (timer < bufferWindow) 
            {
                if (_playerInput.LightAttack) 
                {
                    queued = true;
                    break;
                }
                
                timer += Time.deltaTime;
                yield return null;
            }

            if (queued)
                _currentAirComboIndex++;
            else
                break;
        }
        
        _rb.isKinematic = false;
        _isAttacking = false;
        _currentAirComboIndex = -1;
        _playerAnim.SetAirComboIndex(_currentAirComboIndex);
    }
    #endregion

    #region GROUND HEAVY ATTACK
    protected virtual void ExecuteGroundHeavyAttack()
    {
        if (IsGrounded && _playerInput.HeavyAttack)
            StartCoroutine(GroundHeavyAttack());
    }

    protected virtual IEnumerator GroundHeavyAttack() 
    {
        _isAttacking = true;
        CurrentPlayerAttackData = _groundAttacksDict["GroundHeavy"];
        _playerAnim.PlayHeavyAttackAnimation();
        float animLength = _playerAnim.GetAnimationLength("Ground Heavy Attack");
        yield return new WaitForSeconds(animLength);
        _isAttacking = false;
    }
    #endregion
    
    #region AIR HEAVY ATTACK
    protected virtual void ExecuteAirHeavyAttack() 
    {
        if (!IsGrounded && _playerInput.HeavyAttack)
            StartCoroutine(AirHeavyAttack());
    }

    protected virtual IEnumerator AirHeavyAttack() 
    {
        _isAttacking = true;
        CurrentPlayerAttackData = _airAttacksDict["AirHeavy"];
        _playerAnim.PlayHeavyAttackAnimation();
        float animLength = _playerAnim.GetAnimationLength("Air Heavy Attack");
        yield return new WaitForSeconds(animLength);
        _isAttacking = false;
    }
    #endregion

    #region DEFINE ABILITIES
    protected virtual void DefineAbilities() 
    {
        for (int i = 0; i < _abilities.Count; i++) 
        {
            string key = "Ability" + (i + 1);
            _abilitiesDict.Add(key, _abilities[i]);
        }
    }
    #endregion
    
    #region SPECIAL ABILITY 1
    protected virtual void ExecuteSpecialAbility1()
    {
        float lastSpecialTime = -Mathf.Infinity;
        if (IsGrounded && _playerInput.SpecialAbility1 && Time.time >=
            lastSpecialTime + _abilitiesDict["Ability1"].Cooldown)
        {
            lastSpecialTime = Time.time;
            StartCoroutine(SpecialAbility1());
        }
    }

    protected virtual IEnumerator SpecialAbility1()
    {
        _isAttacking = true;
        CurrentPlayerAbilityData = _abilitiesDict["Ability1"];
        _playerParticle.CurrentParticle = _playerParticle.ParticlesDict["Particle1"];
        _playerAnim.PlayAbilityAnimation();
        float animLength = _playerAnim.GetAnimationLength("Special Ability 1");
        yield return new WaitForSeconds(animLength);
        _isAttacking = false;
    }
    #endregion
    
    #region SPECIAL ABILITY 2
    protected virtual void ExecuteSpecialAbility2()
    {
        float lastSpecialTime = -Mathf.Infinity;
        if (IsGrounded && _playerInput.SpecialAbility2 && Time.time >=
            lastSpecialTime + _abilitiesDict["Ability2"].Cooldown)
        {
            lastSpecialTime = Time.time;
            StartCoroutine(SpecialAbility2());
        }
    }

    protected virtual IEnumerator SpecialAbility2()
    {
        _isAttacking = true;
        CurrentPlayerAbilityData = _abilitiesDict["Ability2"];
        _playerParticle.CurrentParticle = _playerParticle.ParticlesDict["Particle2"];
        _playerAnim.PlayAbilityAnimation();
        float animLength = _playerAnim.GetAnimationLength("Special Ability 2");
        yield return new WaitForSeconds(animLength);
        _isAttacking = false;
    }
    #endregion
    
    #region SPECIAL ABILITY 3
    protected virtual void ExecuteSpecialAbility3()
    {
        float lastSpecialTime = -Mathf.Infinity;
        if (IsGrounded && _playerInput.SpecialAbility3 && Time.time >=
            lastSpecialTime + _abilitiesDict["Ability3"].Cooldown)
        {
            lastSpecialTime = Time.time;
            StartCoroutine(SpecialAbility3());
        }
    }

    protected virtual IEnumerator SpecialAbility3()
    {
        _isAttacking = true;
        CurrentPlayerAbilityData = _abilitiesDict["Ability3"];
        _playerParticle.CurrentParticle = _playerParticle.ParticlesDict["Particle3"];
        _playerAnim.PlayAbilityAnimation();
        float animLength = _playerAnim.GetAnimationLength("Special Ability 3");
        yield return new WaitForSeconds(animLength);
        _isAttacking = false; 
    }
    #endregion
    
    #region ULTIMATE ATTACK
    protected virtual void ExecuteUltimateAttack()
    {
        if (_ultimateAttack != null && IsGrounded && _playerInput.UltimateAttack && _ultimateAttack.UltimateIsReady)
            StartCoroutine(Ultimate());
    }
    
    protected virtual IEnumerator Ultimate()
    {
        _ultimateAttack.StartDimming();
        _isAttacking = true;
        _playerAnim.PlayUltimateAttackAnimation();
        float animLength = _playerAnim.GetAnimationLength("Ultimate Attack");
        _ultimateAttack.ConsumeUltimate();
        yield return new WaitForSeconds(animLength);
        _ultimateAttack.StopDimming();
        _isAttacking = false;
    }
    #endregion

    #region COLLISION
    protected virtual void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            IsGrounded = true;
    }
    
    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            IsGrounded = false;
    }
    #endregion
}