using UnityEngine;

[CreateAssetMenu (menuName = "Character")]
public class CharacterSO : ScriptableObject
{
    [Header("Stats")]
    public int MaxHealth;
    
    [Header("Movement")]
    public float MoveSpeed;
    public float RotationSpeed;
}