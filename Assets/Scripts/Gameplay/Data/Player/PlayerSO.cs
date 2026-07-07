using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PlayableCharacter")]
public class PlayerSO : ScriptableObject
{
    [Header("Stats")]
    public int HP;
    
    [Header("Movement")]
    public float MoveSpeed;
    public float JumpForce;
    public float RotationSpeed;
    
    [Header("Attack Data")]
    public List<PlayerAttackData> GroundAttacks;
    public List<PlayerAttackData> AirAttacks;
    
    [Header("Combo")]
    public float ComboBufferWindow;
    public ComboAttack[] GroundLightAttacks;
    public ComboAttack[] AirLightAttacks;
    
    [Header("Abilities")]
    public List<PlayerAbilityData> Abilities;
    
    [Header("Ultimate")]
    public UltimateData Ultimate;
    
    [System.Serializable]
    public class ComboAttack
    {
        public string AnimName;
    }
}