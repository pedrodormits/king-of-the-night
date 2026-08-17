using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    #region INPUT
    [Header("INPUT")]
    [SerializeField] private KeyCode _specialAbility1Key = KeyCode.LeftShift;
    [SerializeField] private KeyCode _specialAbility2Key = KeyCode.E;
    [SerializeField] private KeyCode _specialAbility3Key = KeyCode.F;
    [SerializeField] private KeyCode _ultimateAttackKey = KeyCode.Q;
    [SerializeField] private KeyCode _pauseKey = KeyCode.P;

    [Header("NAMES")]
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
        SpecialAbility1 = Input.GetKeyDown(_specialAbility1Key);
        SpecialAbility2 = Input.GetKeyDown(_specialAbility2Key);
        SpecialAbility3 = Input.GetKeyDown(_specialAbility3Key);
        UltimateAttack = Input.GetKeyDown(_ultimateAttackKey);
        Pause = Input.GetKeyDown(_pauseKey);
    }
}