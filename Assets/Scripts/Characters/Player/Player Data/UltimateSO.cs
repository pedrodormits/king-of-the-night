using UnityEngine;

/// <summary>
/// Stores the data for a player's ultimate attack.
/// Contains the damage dealt by the ultimate and the amount of ultimate
/// points required before the ultimate can be activated.
/// </summary>
[CreateAssetMenu(menuName = "Player/Ultimate")]
public class UltimateSO : ScriptableObject
{
    public int Damage;
    public int RequiredUltimatePoints;
}