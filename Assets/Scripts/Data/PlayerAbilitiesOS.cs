using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Ability")]
public class PlayerAbilityOS : ScriptableObject
{
    [Header("STATS")]
    public float Cooldown;
    public int Damage;
    public int UltPoints;
    
    [Header("ANIMATION")]
    public string AnimTrigger;

    [Header("AUDIO")]
    public AudioClip AudioClip;
}
