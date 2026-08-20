using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Variables
    protected PlayerAnimation _playerAnim;
    protected PlayerInput _playerInput;
    protected PlayerParticle _playerParticle;
    protected Rigidbody _rb;
    protected UltimateAttack _ultimateAttack;
    [HideInInspector] public bool IsGrounded;
    protected bool _isAttacking;
    protected int _currentGroundComboIndex = -1;
    protected int _currentAirComboIndex = -1;
    
    [Header("Limbs")]
    [SerializeField] protected List<GameObject> _Limbs;
    protected Dictionary<string, GameObject> _limbsDict = new();
    
    [Header("Player Data")]
    [SerializeField] protected CharacterSO _CharacterSO;
    [SerializeField] protected PlayerSO _PlayerSO;
    [HideInInspector] public PlayerAttackSO CurrentPlayerAttackData;
    protected Dictionary<string, PlayerAttackSO> _groundAttacksDict = new();
    protected Dictionary<string, PlayerAttackSO> _airAttacksDict = new();
    [HideInInspector] public PlayerAbilitySO CurrentPlayerAbilityData;
    protected Dictionary<string, PlayerAbilitySO> _abilitiesDict = new();
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

    private void Start()
    {
        if (_Limbs == null)
        {
            Debug.Log("Limbs not found");
        }
        
        if (_CharacterSO == null)
        {
            Debug.Log("CharacterSO not found");
        }
        
        if (_PlayerSO == null)
        {
            Debug.Log("PlayerSO not found");
        }
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

    protected virtual void FixedUpdate() => Move();
    
    protected virtual void Move() 
    {
        if (!_isAttacking) 
        {
            Vector3 input = new Vector3(
                Input.GetAxis("Horizontal"),
                0,
                Input.GetAxis("Vertical")).normalized;
            
            _rb.linearVelocity = new Vector3(
                input.x *
                _CharacterSO.MoveSpeed,
                _rb.linearVelocity.y,
                input.z *
                _CharacterSO.MoveSpeed);
            
            if (input.magnitude > 0) 
            {
                Quaternion targetRotation = Quaternion.LookRotation(input);
                
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    Time.deltaTime *
                    _CharacterSO.RotationSpeed);
            }
        }
        else
        {
            _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
        }
    }

    #region Jump
    protected virtual void ExecuteJump() 
    {
        if (!_isAttacking && _playerInput.Jump && IsGrounded)
        {
            Jump();
        }
    }
    
    protected virtual void Jump() 
    {
        _rb.AddForce(Vector3.up * _CharacterSO.JumpForce, ForceMode.Impulse);
        IsGrounded = false;
    }
    #endregion

    #region Limbs
    protected virtual void DefineLimbs() 
    {
        _limbsDict.Add("RightHand", _Limbs[0]);
        _limbsDict.Add("LeftHand", _Limbs[1]);
        _limbsDict.Add("Feet", _Limbs[2]);
    }
    
    public void EnableRightHand() => _limbsDict["RightHand"].SetActive(true);

    public void DisableRightHand() => _limbsDict["RightHand"].SetActive(false);

    public void EnableLeftHand() => _limbsDict["LeftHand"].SetActive(true);

    public void DisableLeftHand() => _limbsDict["LeftHand"].SetActive(false);

    public void EnableFeet() => _limbsDict["Feet"].SetActive(true);

    public void DisableFeet() => _limbsDict["Feet"].SetActive(false);
    #endregion

    #region Attacks
    protected virtual void DefineGroundLightAttacks() 
    {
        for (int i = 0; i < 3; i++)
        {
            _groundAttacksDict.Add(
                $"GroundLight{i + 1}",
                _PlayerSO.GroundAttacks[i]);
        }
    }
    
    protected virtual void DefineAirLightAttacks() 
    {
        for (int i = 0; i < 3; i++)
        {
            _airAttacksDict.Add($"AirLight{i + 1}", _PlayerSO.AirAttacks[i]);
        }
    }
    
    protected virtual void DefineHeavyAttacks() 
    {
        _groundAttacksDict.Add("GroundHeavy", _PlayerSO.GroundAttacks[3]);
        _airAttacksDict.Add("AirHeavy", _PlayerSO.AirAttacks[3]);
    }

    protected virtual void ExecuteGroundLightAttack() 
    {
        if (IsGrounded && _playerInput.LightAttack && !_isAttacking)
        {
            StartCoroutine(PerformGroundComboAttack());
        }
    }

    protected virtual IEnumerator PerformGroundComboAttack() 
    {
        _isAttacking = true;
        _currentGroundComboIndex = 0;
        while (_currentGroundComboIndex < _PlayerSO.GroundLightAttacks.Length)
        {
            PlayerSO.ComboAttack attack =
                _PlayerSO.GroundLightAttacks[_currentGroundComboIndex];
            
            _playerAnim.SetGroundComboIndex(_currentGroundComboIndex);
            foreach (var limb in _Limbs) 
            {
                string key = "GroundLight" + (_currentGroundComboIndex + 1);
                var limbScript = limb.GetComponent<Limb>();
                if (limbScript != null)
                {
                    limbScript.SetAttackData(_groundAttacksDict[key]);
                }
            }
            
            CurrentPlayerAttackData = _groundAttacksDict["GroundLight1"];
            float animLength = _playerAnim.GetAnimationLength(attack.AnimName);
            float bufferWindow = _PlayerSO.ComboBufferWindow;
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
            {
                _currentGroundComboIndex++;
            }
            else
            {
                break;
            }
        }

        _isAttacking = false;
        _currentGroundComboIndex = -1;
        _playerAnim.SetGroundComboIndex(_currentGroundComboIndex);
    }

    protected virtual void ExecuteAirLightAttack() 
    {
        if (!IsGrounded && _playerInput.LightAttack && !_isAttacking)
        {
            StartCoroutine(PerformAirComboAttack());
        }
    }

    protected virtual IEnumerator PerformAirComboAttack() 
    {
        _isAttacking = true;
        _currentAirComboIndex = 0;
        while (_currentAirComboIndex < _PlayerSO.AirLightAttacks.Length) 
        {
            PlayerSO.ComboAttack attack =
                _PlayerSO.AirLightAttacks[_currentAirComboIndex];
            
            _rb.isKinematic = true;
            _playerAnim.SetAirComboIndex(_currentAirComboIndex);
            CurrentPlayerAttackData = _airAttacksDict["AirLight1"];
            foreach (var limb in _Limbs) 
            {
                string key = "AirLight" + (_currentAirComboIndex + 1);
                var limbScript = limb.GetComponent<Limb>();
                if (limbScript != null)
                {
                    limbScript.SetAttackData(_airAttacksDict[key]);
                }
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
            {
                _currentAirComboIndex++;
            }
            else
            {
                break;
            }
        }
        
        _rb.isKinematic = false;
        _isAttacking = false;
        _currentAirComboIndex = -1;
        _playerAnim.SetAirComboIndex(_currentAirComboIndex);
    }

    protected virtual void ExecuteGroundHeavyAttack()
    {
        if (IsGrounded && _playerInput.HeavyAttack)
        {
            StartCoroutine(GroundHeavyAttack());
        }
    }

    protected virtual IEnumerator GroundHeavyAttack() 
    {
        _isAttacking = true;
        CurrentPlayerAttackData = _groundAttacksDict["GroundHeavy"];
        _playerAnim.PlayHeavyAttackAnimation();
        float animLength = _playerAnim.GetAnimationLength(
            "Ground Heavy Attack");
        yield return new WaitForSeconds(animLength);
        _isAttacking = false;
    }
    
    protected virtual void ExecuteAirHeavyAttack() 
    {
        if (!IsGrounded && _playerInput.HeavyAttack)
        {
            StartCoroutine(AirHeavyAttack());
        }
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

    #region Abilities
    protected virtual void DefineAbilities() 
    {
        for (int i = 0; i < _PlayerSO.Abilities.Count; i++) 
        {
            string key = "Ability" + (i + 1);
            _abilitiesDict.Add(key, _PlayerSO.Abilities[i]);
        }
    }
    
    protected virtual void ExecuteSpecialAbility1()
    {
        float lastSpecialTime = -Mathf.Infinity;
        if (IsGrounded &&
            _playerInput.SpecialAbility1 &&
            Time.time >=
            lastSpecialTime +
            _abilitiesDict["Ability1"].Cooldown)
        {
            lastSpecialTime = Time.time;
            StartCoroutine(SpecialAbility1());
        }
    }

    protected virtual IEnumerator SpecialAbility1()
    {
        _isAttacking = true;
        CurrentPlayerAbilityData = _abilitiesDict["Ability1"];
        
        _playerParticle.CurrentParticle =
            _playerParticle.ParticlesDict["Particle1"];
        
        _playerAnim.PlayAbilityAnimation();
        float animLength = _playerAnim.GetAnimationLength("Special Ability 1");
        yield return new WaitForSeconds(animLength);
        _isAttacking = false;
    }
    
    protected virtual void ExecuteSpecialAbility2()
    {
        float lastSpecialTime = -Mathf.Infinity;
        if (
            IsGrounded &&
            _playerInput.SpecialAbility2 &&
            Time.time >=
            lastSpecialTime +
            _abilitiesDict["Ability2"].Cooldown)
        {
            lastSpecialTime = Time.time;
            StartCoroutine(SpecialAbility2());
        }
    }

    protected virtual IEnumerator SpecialAbility2()
    {
        _isAttacking = true;
        CurrentPlayerAbilityData = _abilitiesDict["Ability2"];
        
        _playerParticle.CurrentParticle =
            _playerParticle.ParticlesDict["Particle2"];
        
        _playerAnim.PlayAbilityAnimation();
        float animLength = _playerAnim.GetAnimationLength("Special Ability 2");
        yield return new WaitForSeconds(animLength);
        _isAttacking = false;
    }
    
    protected virtual void ExecuteSpecialAbility3()
    {
        float lastSpecialTime = -Mathf.Infinity;
        if (
            IsGrounded &&
            _playerInput.SpecialAbility3 &&
            Time.time >=
            lastSpecialTime +
            _abilitiesDict["Ability3"].Cooldown)
        {
            lastSpecialTime = Time.time;
            StartCoroutine(SpecialAbility3());
        }
    }

    protected virtual IEnumerator SpecialAbility3()
    {
        _isAttacking = true;
        CurrentPlayerAbilityData = _abilitiesDict["Ability3"];
        
        _playerParticle.CurrentParticle =
            _playerParticle.ParticlesDict["Particle3"];
        
        _playerAnim.PlayAbilityAnimation();
        float animLength = _playerAnim.GetAnimationLength("Special Ability 3");
        yield return new WaitForSeconds(animLength);
        _isAttacking = false; 
    }
    #endregion
    
    #region Ultimate
    protected virtual void ExecuteUltimateAttack()
    {
        if (
            _ultimateAttack !=
            null &&
            IsGrounded &&
            _playerInput.UltimateAttack &&
            _ultimateAttack.UltimateIsReady)
        {
            StartCoroutine(Ultimate());
        }
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

    #region Collision
    protected virtual void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
        }
    }
    
    protected virtual void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }
    #endregion
}