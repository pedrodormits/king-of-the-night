using System.Collections;
using UnityEngine;

public class UltimateAttack : MonoBehaviour
{
    #region ULTIMATE CHECK
    [Header("ULTIMATE CHECK")]
    [SerializeField] private int _ultimateMeter = 100;
    [SerializeField] private int _currentUltimatePoints;
    public bool UltimateIsReady;
    #endregion
    
    #region ANIMATION SPEED
    [Header("ANIMATION SPEED")]
    [SerializeField] private float _timeScaleMultiplier = 0.25F;
    [SerializeField] private float _animSpeedRestoration = 4;
    #endregion
    
    #region DIMMING EFFECT
    [Header("DIMMING EFFECT")]
    [SerializeField] private float _dimmingIntensity = 0.5f;
    [SerializeField] private float _dimmingDuration = 1;
    private float _normalIntensity;
    private Light _light;
    #endregion

    private void Awake()
    {
        Light[] lights = FindObjectsOfType<Light>();
        foreach (Light l in lights)
        {
            if (l.type == LightType.Directional)
            {
                _light = l;
                break;
            }
        }
        
        _normalIntensity = _light.intensity;
    }

    #region PREPARE ULTIMATE
    public void PrepareUltimate(int ultimatePointsAmount)
    {
        _currentUltimatePoints = Mathf.Min(_currentUltimatePoints + ultimatePointsAmount, _ultimateMeter);
        if (_currentUltimatePoints >= _ultimateMeter)
        {
            UltimateIsReady = true;
        }
    }
    #endregion
    
    #region CONSUME ULTIMATE
    public void ConsumeUltimate()
    {
        _currentUltimatePoints = 0;
        UltimateIsReady = false;
    }
    #endregion
    
    #region DIMMING EFFECT
    public void StartDimming() => StartCoroutine(DimmTheLight());
    
    private IEnumerator DimmTheLight()
    {
        Time.timeScale *= _timeScaleMultiplier;
        float startIntensity = _light.intensity;
        float elapsedTime = 0f;
        while (elapsedTime < _dimmingDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _dimmingDuration;
            _light.intensity = Mathf.Lerp(startIntensity, _dimmingIntensity, t);
            yield return null;
        }

        _light.intensity = _dimmingIntensity;
    }
    #endregion

    #region BRIGHTENING EFFECT
    public void StopDimming()=> StartCoroutine(BrightenTheLight());
    
    private IEnumerator BrightenTheLight()
    {
        _light.intensity = _normalIntensity;
        Time.timeScale *= _animSpeedRestoration;
        yield break;
    }
    #endregion
}