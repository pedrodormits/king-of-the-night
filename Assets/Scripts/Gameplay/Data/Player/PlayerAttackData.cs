using UnityEngine;

[CreateAssetMenu(menuName = "PlayerAttack")]
public class PlayerAttackData : ScriptableObject
{
    [Header("STATS")]
    public int Damage;
    public int UltPoints;
    
    [Header("ANIMATION")] public string AnimTrigger;
    
    [Header("AUDIO")] public AudioClip AudioClip;
}