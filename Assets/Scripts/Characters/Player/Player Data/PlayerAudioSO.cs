using UnityEngine;

/// <summary>
/// Stores the audio clips used by the player.
/// Contains audio for taking damage and for the ultimate ability.
/// </summary>
[CreateAssetMenu(menuName = "Audio/Player")]
public class PlayerAudioSO : ScriptableObject
{
    [Header("Hurt")]
    public AudioClip HurtAudioClip;

    [Header("Ultimate")]
    public AudioClip UltimateChargedAudioClip;
    public AudioClip UltimateAudioClip;
}