using UnityEngine;

[CreateAssetMenu (menuName = "PlayableCharacter")]
public class PlayerSO : ScriptableObject
{
    public int HP;
    public float MoveSpeed;
    public float JumpForce;
}