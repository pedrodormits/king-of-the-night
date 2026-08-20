using UnityEngine;

/// <summary>
/// Stores the data for a player's ability.
/// Contains the ability's cooldown, damage, ultimate
/// point generation, animation trigger, and associated audio clip.
/// </summary>
[CreateAssetMenu(menuName = "Combat/Ability")]
public class PlayerAbilitySO : ScriptableObject
{
    [Header("Stats")]
    public float Cooldown;
    public int Damage;
    
    // Amount of ultimate points generated when the ability successfully hits.
    public int UltPoints;
    
    [Header("Animation")]
    public string AnimTrigger;

    [Header("Audio")]
    public AudioClip AudioClip;
}