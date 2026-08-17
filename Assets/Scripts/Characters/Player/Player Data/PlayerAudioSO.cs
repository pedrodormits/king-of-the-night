using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Player")]
public class PlayerAudioSO : ScriptableObject
{
    [Header("Audio")]
    public AudioClip HurtClip;
    public AudioClip UltimateChargedClip;
    public AudioClip UltimateClip;
}