using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    private CharacterState _enemyState;
    
    [Header("Movement")]
    [SerializeField] private float _MinDist;
    [SerializeField] private Transform _Player;
    private Vector3 _target;
    
    [Header("Data")]
    [SerializeField] private CharacterSO _CharacterOS;
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
    private IEnumerator PrepareMovementRoutine()
    {
        yield return new WaitForSeconds(_EnemySo._TimeToMove);
        
        _enemyState = CharacterState.Moving;
    }
    
    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            _target,
            _CharacterOS.MoveSpeed *
            Time.deltaTime);
        
        float distance = Vector3.Distance(transform.position, _target);
        
        if (distance <= _MinDist)
        {
            _enemyState = CharacterState.Attacking;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _MinDist);
    }
    #endregion
    
    protected virtual void ExecuteAttack() => StartCoroutine(Attack());
    
    protected virtual IEnumerator Attack()
    {
        yield return new WaitForSeconds(3f);
        
        _enemyState = CharacterState.Moving;
    }

    #region Stun
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
    
    private IEnumerator StunRoutine(float duration)
    {
        _enemyState = CharacterState.Stunned;
        
        yield return new WaitForSeconds(duration);
        
        _enemyState = CharacterState.Moving;
    }
    #endregion
}