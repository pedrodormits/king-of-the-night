using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    #region Variables
    [SerializeField] private AudioMixer _AudioMixer;
    [SerializeField] private Slider _MasterSlider, _MusicSlider, _SFXSlider;
    [SerializeField] private TextMeshProUGUI _MasterLabel, _MusicLabel, _SFXLabel;
    [SerializeField] private AudioSource _SFXLoop;
    #endregion

    private void Start()
    {
        if (PlayerPrefs.HasKey("MasterVol"))
        {
            _AudioMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
            // _MasterSlider.value = PlayerPrefs.GetFloat("MasterVol");
        }
        
        if (PlayerPrefs.HasKey("MusicVol"))
        {
            _AudioMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
            // _MusicSlider.value = PlayerPrefs.GetFloat("MusicVol");
        }
        
        if (PlayerPrefs.HasKey("SFXVol"))
        {
            _AudioMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));
            // _SFXSlider.value = PlayerPrefs.GetFloat("SFXVol");
        }
        
        _MasterLabel.text = (_MasterSlider.value + 80).ToString();
        _MusicLabel.text = (_MusicSlider.value + 80).ToString();
        _SFXLabel.text = (_SFXSlider.value + 80).ToString();
    }

    public void SetMasterVolume()
    {
        _MasterLabel.text = (_MasterSlider.value + 80).ToString();
        _AudioMixer.SetFloat("MasterVol", _MasterSlider.value);
        PlayerPrefs.SetFloat("MasterVol", _MasterSlider.value);
    }
    
    public void SetMusicVolume()
    {
        _MusicLabel.text = (_MusicSlider.value + 80).ToString();
        _AudioMixer.SetFloat("MusicVol", _MusicSlider.value);
        PlayerPrefs.SetFloat("MusicVol", _MusicSlider.value);
    }
    
    public void SetSFXVolume()
    {
        _SFXLabel.text = (_SFXSlider.value + 80).ToString();
        _AudioMixer.SetFloat("SFXVol", _SFXSlider.value);
        PlayerPrefs.SetFloat("SFXVol", _SFXSlider.value);
    }

    public void PlaySFXLoop()
    {
        _SFXLoop.Play();
    }
    
    public void StopSFXLoop()
    {
        _SFXLoop.Stop();
    }
}