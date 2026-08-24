using UnityEngine;

/// <summary>
/// Stores settings specific to an enemy character.
/// </summary>
[CreateAssetMenu(menuName = "Enemy/Character")]
public class EnemySO : ScriptableObject
{
    [Header("Movement")]
    // Amount of time the enemy waits before starting to move.
    public float _TimeToMove;
    
    [Header("Stun")]
    public bool _CanBeStunned;
}