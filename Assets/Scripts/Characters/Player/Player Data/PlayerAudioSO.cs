using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Player")]
public class PlayerAudioSO : ScriptableObject
{
    [Header("Hurt")]
    public AudioClip HurtAudioClip;
    
    [Header("Ultimate")]
    public AudioClip UltimateChargedAudioClip;
    public AudioClip UltimateAudioClip;
}