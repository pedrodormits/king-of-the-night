public class VampireLimb : Limb
{
    #region COMPONENTS
    private Health _health;
    private Vampire _vampire;
    #endregion

    private void Awake()
    {
        _health = GetComponentInParent<Health>();
        _vampire = GetComponentInParent<Vampire>();
    }

    protected override void CharacterHit()
    {
        if (_vampire != null && _health != null)
        {
            _health.Heal(_vampire.CurrentRecoveryData.HealAmount);
            if (_vampire.IsAscending && _enemyRB != null)
            {
                _vampire.PerformUppercut(_enemyRB);
            }

            if (_vampire.IsAirAttacking && _enemyRB != null)
            {
                _vampire.StartCoroutine(_vampire.EnableStatic(_enemyRB));
            }
        }
    }
}