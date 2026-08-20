using UnityEngine;

/// <summary>
/// Stores the base statistics and settings for a character.
/// CharacterSO is a ScriptableObject, allowing character data to be stored
/// separately from the character's
/// MonoBehaviour and easily configured in the Inspector.
/// </summary>
[CreateAssetMenu(menuName = "Characters/Character")]
public class CharacterSO : ScriptableObject
{
    [Header("Stats")]
    public int MaxHealth;
    
    [Header("Movement")]
    public float JumpForce;
    public float MoveSpeed;
    
    // Speed at which the character rotates towards a movement direction.
    public float RotationSpeed;
    
    [Header("Audio")]
    public AudioClip DeathClip;
}