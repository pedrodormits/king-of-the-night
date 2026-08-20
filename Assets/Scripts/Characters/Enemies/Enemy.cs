using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    [SerializeField] private Transform _PointA;
    [SerializeField] private Transform _PointB;
    private Vector3 _target;
    
    [Header("Stun")]
    [SerializeField] private bool _CanBeStunned = true;
    private bool _isStunned;
    
    [Header("Data")]
    [SerializeField] private CharacterSO _CharacterOS;
    #endregion
    
    private void Update()
    {
        if (_isStunned || GameManager.Instance.GameIsOver)
        {
            return;
        }
        
        _target = _PointB.position;
        MoveTowardsTarget();
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
            _target = _target == _PointA.position ? _PointB.position : _PointA.position;
        }
    }

    public void Stun(float duration)
    {
        if (!_CanBeStunned)
        {
            return;
        }

        if (_isStunned)
        {
            return;
        }

        StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        _isStunned = true;
        yield return new WaitForSeconds(duration);
        _isStunned = false;
    }
}