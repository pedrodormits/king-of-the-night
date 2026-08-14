using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Player")]
public class PlayerSO : ScriptableObject
{
    [Header("jump")]
    public float JumpForce;
    
    [Header("Attack Data")]
    public List<PlayerAttackSO> GroundAttacks;
    public List<PlayerAttackSO> AirAttacks;
    
    [Header("Combo")]
    public float ComboBufferWindow;
    public ComboAttack[] GroundLightAttacks;
    public ComboAttack[] AirLightAttacks;
    
    [Header("Abilities")]
    public List<PlayerAbilitySO> Abilities;
    
    [Header("Ultimate")]
    public UltimateSO Ultimate;
    
    [Header("Audio")]
    public AudioClip HurtClip;
    public AudioClip UltimateClip;
    
    [System.Serializable]
    public class ComboAttack
    {
        public string AnimName;
    }
}