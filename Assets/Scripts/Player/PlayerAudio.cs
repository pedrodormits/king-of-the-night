using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    #region COMPONENTS
    [SerializeField] private CharacterSO _CharacterSO;
    [SerializeField] private PlayerSO _PlayerSO;
    private AudioSource _audioSource;
    private Player _player;
    #endregion

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GetComponent<Player>();
    } 

    #region PLAY AUDIO

    public void PlayAttackAudio()
    {
        _audioSource.PlayOneShot(_player.CurrentPlayerAttackData.AudioClip);
    }

    public void PlaySpecialAbilityAudio()
    {
        _audioSource.PlayOneShot(_player.CurrentPlayerAbilityData.AudioClip);    
    }

    public void PlayHurtAudio() => _audioSource.PlayOneShot(_PlayerSO.HurtClip);

    public void PlayDeathAudio() => _audioSource.PlayOneShot(_CharacterSO.DeathClip);
    #endregion
}