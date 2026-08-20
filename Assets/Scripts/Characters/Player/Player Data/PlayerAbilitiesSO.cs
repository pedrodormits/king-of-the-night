using UnityEngine;

/// <summary>
/// Stores the data for a player's ability.
/// Contains the ability's cooldown, damage, ultimate point generation,
/// animation trigger, and associated audio clip.
/// </summary>
[CreateAssetMenu(menuName = "Combat/Ability")]
public class PlayerAbilitySO : ScriptableObject
{
    [Header("STATS")]
    public float Cooldown;
    public int Damage;
    
    // Amount of ultimate points generated when the ability successfully hits.
    public int UltPoints;
    
    [Header("ANIMATION")]
    public string AnimTrigger;

    [Header("AUDIO")]
    public AudioClip AudioClip;
}