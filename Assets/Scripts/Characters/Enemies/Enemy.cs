using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    private CharacterState _enemyState;
    
    [Header("Movement")]
    [SerializeField] private float _TimeToMove;
    [SerializeField] private Transform _PointA;
    [SerializeField] private Transform _PointB;
    private Vector3 _target;
    
    [Header("Stun")]
    [SerializeField] private bool _CanBeStunned = true;
    
    [Header("Data")]
    [SerializeField] private CharacterSO _CharacterOS;
    #endregion

    private void Start()
    {
        _enemyState = CharacterState.Idle;
        _target = _PointB.position;
    }

    private void Update()
    {
        if (GameManager.Instance.GameIsOver)
        {
            return;
        }

        UpdateCharacterState();
    }

    private void UpdateCharacterState()
    {
        switch (_enemyState)
        {
            case CharacterState.Idle:
                PrepareMovement();
                break;
            case CharacterState.Moving:
                MoveTowardsTarget();
                break;
            
            case CharacterState.Attacking:
                
                break;
            
            case CharacterState.Hurt:
                
                break;
            
            case CharacterState.Stunned:
                break;
            
            case CharacterState.Dead:
                
                break;
        }
    }

    private void PrepareMovement() => StartCoroutine("PrepareMovementRoutine");

    private IEnumerator PrepareMovementRoutine()
    {
        yield return new WaitForSeconds(_TimeToMove);
        _enemyState = CharacterState.Moving;
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            _target,
            _CharacterOS.MoveSpeed *
            Time.deltaTime);

        if (Vector3.Distance(transform.position, _target) < 0.1f)
        {
            if (_target == _PointA.position)
            {
                _target = _PointB.position;
            }
            else
            {
                _target = _PointA.position;
            }
        }
    }

    public void Stun(float duration)
    {
        if (!_CanBeStunned)
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
}