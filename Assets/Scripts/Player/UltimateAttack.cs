using System.Collections;
using UnityEngine;

public class UltimateAttack : MonoBehaviour
{
    #region ULTIMATE CHECK
    [Header("ULTIMATE CHECK")]
    [HideInInspector] public bool UltimateIsReady;
    [SerializeField] private int _UltimateMeter = 100;
    [SerializeField] private int _CurrentUltimatePoints;
    #endregion
    
    #region ANIMATION SPEED
    [Header("ANIMATION SPEED")]
    [SerializeField] private float _TimeScaleMultiplier = 0.25F;
    [SerializeField] private float _AnimSpeedRestoration = 4;
    #endregion
    
    #region DIMMING EFFECT
    [Header("DIMMING EFFECT")]
    [SerializeField] private float _DimmingIntensity = 0.5f;
    [SerializeField] private float _DimmingDuration = 1;
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
        _CurrentUltimatePoints = Mathf.Min(_CurrentUltimatePoints + ultimatePointsAmount, _UltimateMeter);
        if (_CurrentUltimatePoints >= _UltimateMeter)
        {
            UltimateIsReady = true;
        }
    }
    #endregion
    
    #region CONSUME ULTIMATE
    public void ConsumeUltimate()
    {
        _CurrentUltimatePoints = 0;
        UltimateIsReady = false;
    }
    #endregion
    
    #region DIMMING EFFECT
    public void StartDimming() => StartCoroutine(DimmTheLight());
    
    private IEnumerator DimmTheLight()
    {
        Time.timeScale *= _TimeScaleMultiplier;
        float startIntensity = _light.intensity;
        float elapsedTime = 0f;
        while (elapsedTime < _DimmingDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _DimmingDuration;
            _light.intensity = Mathf.Lerp(startIntensity, _DimmingIntensity, t);
            yield return null;
        }

        _light.intensity = _DimmingIntensity;
    }
    #endregion

    #region BRIGHTENING EFFECT
    public void StopDimming()=> StartCoroutine(BrightenTheLight());
    
    private IEnumerator BrightenTheLight()
    {
        _light.intensity = _normalIntensity;
        Time.timeScale *= _AnimSpeedRestoration;
        yield break;
    }
    #endregion
}