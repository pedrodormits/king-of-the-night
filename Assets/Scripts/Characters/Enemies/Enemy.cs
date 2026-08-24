using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the enemy's basic behaviour using a character state machine.
/// The enemy can move towards the player, attack
/// when within range, and temporarily become stunned.
/// </summary>
public class Enemy : MonoBehaviour
{
    #region Variables
    private CharacterState _enemyState;
    
    [Header("Movement")]
    // Minimum distance the enemy must reach before starting an attack.
    [SerializeField] private float _MinDist;
    [SerializeField] private Transform _Player;
    private Vector3 _target;
    
    [Header("Data")]
    // Contains general character statistics and movement settings,
    // such as health, jump force,
    // movement speed, rotation speed, and death audio.
    [SerializeField] private CharacterSO _CharacterOS;
    
    // Contains enemy-specific settings, such as the
    // initial movement delay and whether the enemy can be stunned.
    [SerializeField] private EnemySO _EnemySo;
    #endregion

    private void Start()
    {
        if (_Player == null)
        {
            Debug.Log("_Player is null");
        }
        
        _enemyState = CharacterState.Idle;
        StartCoroutine("PrepareMovementRoutine");
    }

    private void Update()
    {
        if (GameManager.Instance.GameIsOver)
        {
            return;
        }

        _target = _Player.position;
        UpdateCharacterState();
    }

    /// <summary>
    /// Checks the enemy's current state and executes the corresponding behaviour.
    /// </summary>
    private void UpdateCharacterState()
    {
        switch (_enemyState)
        {
            case CharacterState.Idle:
                break;
            
            case CharacterState.Moving:
                MoveTowardsTarget();
                break;
            
            case CharacterState.Attacking:
                ExecuteAttack();
                break;
            
            case CharacterState.Hurt:
                break;
            
            case CharacterState.Stunned:
                break;
            
            case CharacterState.Dead:
                break;
        }
    }
    
    #region Movement
    /// <summary>
    /// Delays the enemy's movement before changing its state to Moving.
    /// </summary>
    private IEnumerator PrepareMovementRoutine()
    {
        yield return new WaitForSeconds(_EnemySo._TimeToMove);
        
        _enemyState = CharacterState.Moving;
    }

    /// <summary>
    /// Moves the enemy towards the current target position.
    /// Changes the enemy's state to Attacking when it reaches the minimum distance.
    /// </summary>
    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            _target,
            _CharacterOS.MoveSpeed *
            Time.deltaTime);
        
        // Calculate the current distance between the enemy and its target.
        float distance = Vector3.Distance(transform.position, _target);
        
        if (distance <= _MinDist)
        {
            _enemyState = CharacterState.Attacking;
        }
    }
    
    /// <summary>
    /// Displays the enemy's attack range in
    /// the Unity Editor when the object is selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // Draw a sphere representing the minimum distance required to attack.
        Gizmos.DrawWireSphere(
            transform.position,
            _MinDist);
    }
    #endregion
    
    /// <summary>
    /// Starts the enemy's attack coroutine.
    /// </summary>
    protected virtual void ExecuteAttack() => StartCoroutine(Attack());

    /// <summary>
    /// Performs the enemy's attack and
    /// returns the enemy to the Moving state afterward.
    /// </summary>
    protected virtual IEnumerator Attack()
    {
        yield return new WaitForSeconds(3f);
        
        _enemyState = CharacterState.Moving;
    }

    #region Stun
    /// <summary>
    /// Attempts to stun the enemy for the specified duration.
    /// </summary>
    public void Stun(float duration)
    {
        if (!_EnemySo._CanBeStunned)
        {
            return;
        }

        if (_enemyState == CharacterState.Stunned)
        {
            return;
        }

        StartCoroutine(StunRoutine(duration));
    }

    /// <summary>
    /// Temporarily changes the enemy's state to Stunned.
    /// </summary>
    private IEnumerator StunRoutine(float duration)
    {
        _enemyState = CharacterState.Stunned;
        
        yield return new WaitForSeconds(duration);
        
        _enemyState = CharacterState.Moving;
    }
    #endregion
}