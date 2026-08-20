using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the configuration data for a playable character.
/// Contains the character's attacks, combo
/// settings, abilities, and ultimate attack data.
/// </summary>
[CreateAssetMenu(menuName = "Characters/Player")]
public class PlayerSO : ScriptableObject
{
    [Header("Attack")]
    public List<PlayerAttackSO> GroundAttacks;
    public List<PlayerAttackSO> AirAttacks;

    [Header("Combo")]
    // Maximum amount of time allowed between attacks to continue a combo.
    public float ComboBufferWindow;

    // Animation data for the player's ground light attack combo.
    public ComboAttack[] GroundLightAttacks;

    // Animation data for the player's air light attack combo.
    public ComboAttack[] AirLightAttacks;

    [Header("Abilities")]
    public List<PlayerAbilitySO> Abilities;

    [Header("Ultimate")]
    public UltimateSO Ultimate;

    /// <summary>
    /// Stores the animation information required for an individual combo attack.
    /// </summary>
    [System.Serializable]
    public class ComboAttack
    {
        public string AnimName;
    }
}