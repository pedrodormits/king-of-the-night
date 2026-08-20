using System;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    #region Variables
    [SerializeField] private PlayerAudioSO _PlayerAudioSO;
    [SerializeField] private CharacterSO _CharacterSO;
    private AudioSource _audioSource;
    private Player _player;
    #endregion

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        if (_PlayerAudioSO == null)
        {
            Debug.Log("PlayerAudioSO is null");
        }
        
        if (_CharacterSO == null)
        {
            Debug.Log("CharacterSO is null");
        }
    }

    #region Play Audio
    public void PlayAttackAudio()
    {
        _audioSource.PlayOneShot(_player.CurrentPlayerAttackData.AudioClip);
    }

    public void PlaySpecialAbilityAudio()
    {
        _audioSource.PlayOneShot(_player.CurrentPlayerAbilityData.AudioClip);    
    }

    public void PlayHurtAudio()
    {
        _audioSource.PlayOneShot(_PlayerAudioSO.HurtAudioClip);   
    }

    public void PlayDeathAudio()
    {
        _audioSource.PlayOneShot(_CharacterSO.DeathClip);
    }

    public void PlayUltimateChargedAudio()
    {
        _audioSource.PlayOneShot(_PlayerAudioSO.UltimateChargedAudioClip);
    }

    public void PlayUltimateAudio()
    {
        _audioSource.PlayOneShot(_PlayerAudioSO.UltimateAudioClip);   
    }
    #endregion
}