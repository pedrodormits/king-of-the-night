using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _AudioMixer;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MasterVol"))
        {
            _AudioMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
        }
        
        if (PlayerPrefs.HasKey("MusicVol"))
        {
            _AudioMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        }
        
        if (PlayerPrefs.HasKey("SFXVol"))
        {
            _AudioMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));
        }
    }
}