using UnityEngine;

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
