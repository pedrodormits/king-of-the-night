using System.Collections;
using UnityEngine;

/// <summary>
/// UltimateAttack manages the player's ultimate ability.
/// It keeps track of the ultimate meter, determines when the ultimate is
/// ready, and controls the lighting and timescale effects used during the
/// ultimate attack.
/// </summary>
public class UltimateAttack : MonoBehaviour
{
    #region Variables
    [Header("Ultimate Check")]
    // Indicates whether the ultimate meter is completely filled and the
    // ultimate can be used.
    [HideInInspector] public bool UltimateIsReady;
    
    // Maximum amount of points required to fill the ultimate meter.
    [SerializeField] private UltimateSO _UltimateOS;
    
    // Current amount of points stored in the ultimate meter.
    [SerializeField] private int _CurrentUltimatePoints;
    
    // Reference to the UI element that displays the ultimate meter.
    [SerializeField] private UltimateBar _UltimateBar;
    
    [SerializeField] private PlayerAudio _PlayerAudio;

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
    
    // Stores the original light intensity so it can be restored later.
    private float _normalIntensity;
    
    // Reference to the directional light used for the dimming effect.
    private Light _light;
    #endregion
    
    private void Awake() => FindDirectionalLight();

    private void Start()
    {
        if (_UltimateOS == null)
        {
            Debug.Log("UltimateOS is null");
        }
        
        if (_UltimateBar == null)
        {
            Debug.Log("UltimateBar is null");
        }
        
        if (_PlayerAudio == null)
        {
            Debug.Log("PlayerAudio is null");
        }
    }

    #region Get Directional Light
    /// <summary>
    /// This method searches for the Directional Light in the scene and stores
    /// a reference to it.
    /// It also saves the light's original intensity so it can be restored
    /// after the ultimate effect ends.
    /// </summary>
    private void FindDirectionalLight()
    {
        // Find all lights currently active in the scene.
        Light[] lights = FindObjectsOfType<Light>();
        
        // Search for the first directional light.
        foreach (Light l in lights)
        {
            if (l.type == LightType.Directional)
            {
                _light = l;
                break;
            }
        }
        
        // Store the original intensity of the directional light.
        _normalIntensity = _light.intensity;
    }
    #endregion

    #region Prepare Ultimate
    /// <summary>
    /// Adds the specified amount of points to the ultimate meter.
    /// The current points are capped at the maximum meter value.
    /// The UltimateBar is updated to reflect the new amount of points.
    /// Once the meter is full, the ultimate is marked as ready.
    /// </summary>
    public void PrepareUltimate(int ultimatePointsAmount)
    {
        // Add the earned points while preventing the meter from exceeding its
        // maximum value.
        _CurrentUltimatePoints = Mathf.Min(
            _CurrentUltimatePoints + ultimatePointsAmount,
            _UltimateOS.RequiredUltimatePoints
        );

        // Update the UI to display the current ultimate points.
        _UltimateBar.SetUltimate(_CurrentUltimatePoints);

        // Mark the ultimate as ready once the meter is completely filled.
        if (_CurrentUltimatePoints >= _UltimateOS.RequiredUltimatePoints)
        {
            UltimateIsReady = true;
            _PlayerAudio.PlayUltimateChargedAudio();
        }
    }
    #endregion

    #region Consume Ultimate
    /// <summary>
    /// Consumes the ultimate ability by resetting the ultimate meter.
    /// The UltimateBar is updated and the ultimate is marked as unavailable.
    /// </summary>
    public void ConsumeUltimate()
    {
        // Reset the current ultimate points after using the ultimate.
        _CurrentUltimatePoints = 0;
        
        // Update the UI to show that the ultimate meter is empty.
        _UltimateBar.SetUltimate(0);
        
        // Mark the ultimate as no longer ready.
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
        // Slow down the entire game by multiplying the current timescale.
        Time.timeScale *= _TimeScaleMultiplier;
        
        // Store the current light intensity as the starting point.
        float startIntensity = _light.intensity;
        
        // Reset the elapsed time for the transition.
        float elapsedTime = 0f;
        
        // Gradually change the light intensity over the configured duration.
        while (elapsedTime < _DimmingDuration) 
        {
            elapsedTime += Time.deltaTime;
            
            // Calculate the progress of the transition between 0 and 1.
            float t = elapsedTime / _DimmingDuration; 

            // Smoothly interpolate between the starting intensity and the
            // target dimming intensity.
            _light.intensity = Mathf.Lerp(startIntensity, _DimmingIntensity, t);

            yield return null;
        }
        
        // Make sure the final intensity is exactly the target value.
        _light.intensity = _DimmingIntensity; 
    }
    #endregion

    #region Brightening Effect
    /// <summary>
    /// Starts the process of restoring the lighting and game speed.
    /// </summary>
    public void StopDimming() => StartCoroutine(BrightenTheLight());
    
    /// <summary>
    /// Restores the original lighting and increases the game speed back to
    /// normal.
    /// </summary>
    private IEnumerator BrightenTheLight()
    {
        // Restore the original directional light intensity.
        _light.intensity = _normalIntensity;
        
        // Restore the normal animation/game speed.
        Time.timeScale *= _AnimSpeedRestoration;
        
        // End the coroutine.
        yield break; 
    }
    #endregion
}