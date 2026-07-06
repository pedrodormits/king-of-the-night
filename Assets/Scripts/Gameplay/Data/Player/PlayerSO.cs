using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PlayableCharacter")]
public class PlayerSO : ScriptableObject
{
    [Header("Stats")]
    public int HP;
    public float MoveSpeed;
    public float JumpForce;
    
    [Header("Attacks")]
    public List<PlayerAttackData> GroundAttacks;
    public List<PlayerAttackData> AirAttacks;
    
    [Header("Abilities")]
    public List<PlayerAbilityData> Abilities;
    
    [Header("Ultimate")]
    public UltimateData Ultimate;
}