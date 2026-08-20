using UnityEngine;

/// <summary>
/// Stores the data for a player's regular attack.
/// Contains the attack's damage, ultimate point generation,
/// animation trigger, and associated audio clip.
/// </summary>
[CreateAssetMenu(menuName = "Player/Attack")]
public class PlayerAttackSO : ScriptableObject
{
    [Header("Stats")]
    public int Damage;

    // Amount of ultimate points generated when the attack successfully hits.
    public int UltPoints;

    [Header("Animation")]
    public string AnimTrigger;

    [Header("Audio")]
    public AudioClip AudioClip;
}