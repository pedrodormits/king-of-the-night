using UnityEngine;

/// <summary>
/// Stores the audio clips used by the player.
/// Contains audio for taking damage and for the ultimate ability.
/// </summary>
[CreateAssetMenu(menuName = "Player/Audio")]
public class PlayerAudioSO : ScriptableObject
{
    [Header("Hurt")]
    public AudioClip HurtAudioClip;

    [Header("Ultimate")]
    public AudioClip UltimateChargedAudioClip;
    public AudioClip UltimateAudioClip;
}