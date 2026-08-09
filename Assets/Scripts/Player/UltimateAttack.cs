using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// UltimateAttack manages the player's ultimate ability.
/// It keeps track of the ultimate meter, determines when the ultimate is ready,
/// and controls the lighting and timescale effects used during the ultimate attack.
/// </summary>
public class UltimateAttack : MonoBehaviour
{
    #region Variables
    [Header("Ultimate Check")]
    // Indicates whether the ultimate meter is completely filled and the ultimate can be used.
    /*[HideInInspector]*/ public bool UltimateIsReady;
    
    [SerializeField] private int _UltimateMeter = 100; // Maximum amount of points required to fill the ultimate meter.
    [SerializeField] private int _CurrentUltimatePoints; // Current amount of points stored in the ultimate meter.
    [SerializeField] private UltimateBar _UltimateBar; // Reference to the UI element that displays the ultimate meter.

    [Header("Animation Speed")]
    // Multiplier used to slow down time during the ultimate effect.
    [SerializeField] private float _TimeScaleMultiplier = 0.25F;

    // Multiplier used to restore the normal timescale after the ultimate.
    [SerializeField] private float _AnimSpeedRestoration = 4;

    [Header("Dimming Effect")]
    // Target intensity of the directional light while the ultimate is active.
    [SerializeField] private float _DimmingIntensity = 0.5f;

    // Duration of the transition from normal lighting to the dimmed lighting.
    [SerializeField] private float _DimmingDuration = 1;
    
    private float _normalIntensity; // Stores the original light intensity so it can be restored later.
    private Light _light; // Reference to the directional light used for the dimming effect.
    #endregion
    
    private void Awake() => FindDirectionalLight();

    /// <summary>
    /// This method searches for the Directional Light in the scene and stores a reference to it.
    /// It also saves the light's original intensity so it can be restored after the ultimate effect ends.
    /// </summary>
    private void FindDirectionalLight()
    {
        Light[] lights = FindObjectsOfType<Light>(); // Find all lights currently active in the scene.
        foreach (Light l in lights) // Search for the first directional light.
        {
            if (l.type == LightType.Directional)
            {
                _light = l;
                break;
            }
        }
        
        _normalIntensity = _light.intensity; // Store the original intensity of the directional light.
    }

    private void Start()
    {
        if (_UltimateBar == null)
        {
            Debug.Log("_UltimateBar is null");
        }
    }

    #region Prepare Ultimate
    /// <summary>
    /// Adds points to the ultimate meter.
    /// The value cannot exceed the maximum meter capacity.
    /// </summary>
    public void PrepareUltimate(int ultimatePointsAmount)
    {
        _CurrentUltimatePoints = Mathf.Min(_CurrentUltimatePoints + ultimatePointsAmount, _UltimateMeter);
        if (_CurrentUltimatePoints >= _UltimateMeter) // Check if the ultimate meter is completely filled.
        {
            UltimateIsReady = true;
        }
    }
    #endregion

    #region CONSUME ULTIMATE
    /// <summary>
    /// Consumes the ultimate ability by resetting the ultimate meter.
    /// </summary>
    public void ConsumeUltimate()
    {
        _CurrentUltimatePoints = 0;
        _UltimateBar.SetUltimate(0);
        UltimateIsReady = false;
    }
    #endregion

    #region Dimming Effect
    /// <summary>
    /// Starts the lighting and timescale effect.
    /// </summary>
    public void StartDimming() => StartCoroutine(DimmTheLight());

    /// <summary>
    /// Gradually slows down the game and dims the directional light.
    /// </summary>
    private IEnumerator DimmTheLight()
    {
        Time.timeScale *= _TimeScaleMultiplier; // Slow down the entire game by multiplying the current timescale.
        float startIntensity = _light.intensity; // Store the current light intensity as the starting point.
        float elapsedTime = 0f; // Reset the elapsed time for the transition.
        while (elapsedTime < _DimmingDuration) // Gradually change the light intensity over the configured duration.
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / _DimmingDuration; // Calculate the progress of the transition between 0 and 1.

            // Smoothly interpolate between the starting intensity
            // and the target dimming intensity.
            _light.intensity = Mathf.Lerp(startIntensity, _DimmingIntensity, t);

            yield return null;
        }
        
        _light.intensity = _DimmingIntensity; // Make sure the final intensity is exactly the target value.
    }
    #endregion

    #region Brightening Effect
    /// <summary>
    /// Starts the process of restoring the lighting and game speed.
    /// </summary>
    public void StopDimming() => StartCoroutine(BrightenTheLight());
    
    /// <summary>
    /// Restores the original lighting and increases the game speed back to normal.
    /// </summary>
    private IEnumerator BrightenTheLight()
    {
        _light.intensity = _normalIntensity; // Restore the original directional light intensity.
        Time.timeScale *= _AnimSpeedRestoration; // Restore the normal animation/game speed.
        yield break; // End the coroutine.
    }
    #endregion
}