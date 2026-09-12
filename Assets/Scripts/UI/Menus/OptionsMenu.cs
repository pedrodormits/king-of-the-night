using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer _AudioMixer;
    [SerializeField] private Slider _MasterSlider, _MusicSlider, _SFXSlider;
    [SerializeField] private TextMeshProUGUI _MasterLabel, _MusicLabel, _SFXLabel;

    public void SetMasterVolume()
    {
        _MasterLabel.text = (_MasterSlider.value + 80).ToString();
        _AudioMixer.SetFloat("MasterVol", _MasterSlider.value);
    }
    
    public void SetMusicVolume()
    {
        _MusicLabel.text = (_MusicSlider.value + 80).ToString();
        _AudioMixer.SetFloat("MusicVol", _MusicSlider.value);
    }
    
    public void SetSFXVolume()
    {
        _SFXLabel.text = (_SFXSlider.value + 80).ToString();
        _AudioMixer.SetFloat("SFXVol", _SFXSlider.value);
    }
}