using UnityEngine;

[CreateAssetMenu(menuName = "PlayerAbility")]
public class PlayerAbilityData : ScriptableObject
{
    [Header("STATS")]
    public float Cooldown;
    public int Damage;
    public int UltPoints;
    
    [Header("ANIMATION")] public string AnimTrigger;

    [Header("AUDIO")] public AudioClip AudioClip;
}
