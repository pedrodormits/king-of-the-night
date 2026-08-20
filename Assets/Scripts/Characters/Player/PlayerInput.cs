using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region Variables
    [Header("Inputs")]
    [SerializeField] private PlayerInputSO _PlayerInputSO;
    [HideInInspector] public bool Jump;
    [HideInInspector] public bool LightAttack;
    [HideInInspector] public bool HeavyAttack;
    [HideInInspector] public bool SpecialAbility1;
    [HideInInspector] public bool SpecialAbility2;
    [HideInInspector] public bool SpecialAbility3;
    [HideInInspector] public bool UltimateAttack;
    [HideInInspector] public bool Pause;
    #endregion

    private void Update() => DefineKeyboardInput();

    private void DefineKeyboardInput()
    {
        Jump = Input.GetButtonDown("Jump");
        LightAttack = Input.GetButtonDown("Fire1");
        HeavyAttack = Input.GetButtonDown("Fire2");
        SpecialAbility1 = Input.GetKeyDown(_PlayerInputSO.SpecialAbility1Key);
        SpecialAbility2 = Input.GetKeyDown(_PlayerInputSO.SpecialAbility2Key);
        SpecialAbility3 = Input.GetKeyDown(_PlayerInputSO.SpecialAbility3Key);
        UltimateAttack = Input.GetKeyDown(_PlayerInputSO.UltimateAttackKey);
        Pause = Input.GetKeyDown(_PlayerInputSO.PauseKey);
    }
}