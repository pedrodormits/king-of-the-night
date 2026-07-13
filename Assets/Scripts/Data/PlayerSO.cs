using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Player")]
public class PlayerSO : ScriptableObject
{
    [Header("jump")]
    public float JumpForce;
    
    [Header("Attack Data")]
    public List<PlayerAttackData> GroundAttacks;
    public List<PlayerAttackData> AirAttacks;
    
    [Header("Combo")]
    public float ComboBufferWindow;
    public ComboAttack[] GroundLightAttacks;
    public ComboAttack[] AirLightAttacks;
    
    [Header("Abilities")]
    public List<PlayerAbilityOS> Abilities;
    
    [Header("Ultimate")]
    public UltimateData Ultimate;
    
    [System.Serializable]
    public class ComboAttack
    {
        public string AnimName;
    }
}