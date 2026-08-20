using UnityEngine;

/// <summary>
/// Stores the keyboard inputs used by the player.
/// Allows player controls to be configured separately from the input logic.
/// </summary>
[CreateAssetMenu(menuName = "Game/PlayerInput")]
public class PlayerInputSO : ScriptableObject
{
    [Header("Inputs")]
    public KeyCode SpecialAbility1Key = KeyCode.LeftShift;
    public KeyCode SpecialAbility2Key = KeyCode.E;
    public KeyCode SpecialAbility3Key = KeyCode.F;
    public KeyCode UltimateAttackKey = KeyCode.Q;
    public KeyCode PauseKey = KeyCode.P;
}