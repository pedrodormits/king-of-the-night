using UnityEngine;

[CreateAssetMenu(menuName = "Enemy/Character")]
public class EnemySO : ScriptableObject
{
    [Header("Movement")]
    public float _TimeToMove;
    
    [Header("Stun")]
    public bool _CanBeStunned;
}
