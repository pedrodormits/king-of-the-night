using UnityEngine;

public class EnemyHealth : Health
{
    #region Variables
    private Enemy _enemy;
    // private EnemyAnimation _enemyAnim;
    #endregion
    
    private void Awake()
    {
        _enemy = GetComponent<Enemy>();
        // _enemyAnim = GetComponent<EnemyAnimation>();
    }
    
    #region DEATH
    protected override void Die()
    {
        _enemy.enabled = false;
        // _enemyAnim.PlayDeathAnimation();
        if (TryGetComponent(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }
    #endregion
}