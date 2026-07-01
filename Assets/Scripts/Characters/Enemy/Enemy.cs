using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region MOVEMENT
    [Header("MOVEMENT")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f;
    private Vector3 _target;
    #endregion

    #region STUN
    [Header("STUN")]
    [SerializeField] private bool _canBeStunned = true;
    private bool _isStunned;
    #endregion
    
    private void Update()
    {
        if (_isStunned || GameManager.Instance.GameIsOver) return;
        
        _target = pointB.position;
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            _target,
            speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _target) < 0.1f)
            _target = _target == pointA.position ? pointB.position : pointA.position;
    }

    public void Stun(float duration)
    {
        if (!_canBeStunned) return;
        
        if (_isStunned) return;

        StartCoroutine(StunRoutine(duration));
    }

    private IEnumerator StunRoutine(float duration)
    {
        _isStunned = true;
        yield return new WaitForSeconds(duration);
        _isStunned = false;
    }
}