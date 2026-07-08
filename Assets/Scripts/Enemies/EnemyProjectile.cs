using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private int _damageAmount = 10;

    private void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(_damageAmount);
        }

        Destroy(gameObject);
    }
}