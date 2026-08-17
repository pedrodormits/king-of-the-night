using UnityEngine;

/// <summary>
/// Stores the base statistics and settings for a character.
/// CharacterSO is a ScriptableObject, allowing character data to be stored
/// separately from the character's MonoBehaviour and easily configured in the
/// Inspector.
/// </summary>
[CreateAssetMenu(menuName = "Characters/Character")]
public class CharacterSO : ScriptableObject
{
    [Header("Stats")]
    // Maximum amount of health the character can have.
    public int MaxHealth;
    
    [Header("Movement")]
    // Upward force applied when the character jumps.
    public float JumpForce;
    
    // Movement speed of the character.
    public float MoveSpeed;
    
    // Speed at which the character rotates towards a movement direction.
    public float RotationSpeed;
    
    [Header("Audio")]
    // Sound effect played when the character dies.
    public AudioClip DeathClip;
}