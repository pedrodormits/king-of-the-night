using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores the player's combat and ability data.
/// PlayerSO is a ScriptableObject, allowing attack, combo, ability, and
/// ultimate data to be configured separately from the Player script.
/// </summary>
[CreateAssetMenu(menuName = "Characters/Player")]
public class PlayerSO : ScriptableObject
{
    [Header("Attack")]
    // Contains all ground-based attacks available to the player.
    public List<PlayerAttackSO> GroundAttacks;

    // Contains all air-based attacks available to the player.
    public List<PlayerAttackSO> AirAttacks;

    [Header("Combo")]
    // Determines how long the player can wait between attacks while still
    // continuing the current combo.
    public float ComboBufferWindow;

    // Contains the animation data used for ground light attack combos.
    public ComboAttack[] GroundLightAttacks;

    // Contains the animation data used for air light attack combos.
    public ComboAttack[] AirLightAttacks;

    [Header("Abilities")]
    // Contains all special abilities available to the player.
    public List<PlayerAbilitySO> Abilities;

    [Header("Ultimate")]
    // Contains the data for the player's ultimate ability.
    public UltimateSO Ultimate;

    /// <summary>
    /// Stores the animation information for an individual combo attack.
    /// </summary>
    [System.Serializable]
    public class ComboAttack
    {
        // Name of the animation that should be played for this combo attack.
        public string AnimName;
    }
}