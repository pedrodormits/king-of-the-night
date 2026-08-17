using UnityEngine;

[CreateAssetMenu(menuName = "Characters/Character")]
public class CharacterSO : ScriptableObject
{
    [Header("Stats")]
    public int MaxHealth;
    
    [Header("Movement")]
    public float JumpForce;
    public float MoveSpeed;
    public float RotationSpeed;
    
    [Header("Audio")]
    public AudioClip DeathClip;
}