using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ultimate controls the UI slider that displays the player's ultimate points.
/// The slider is updated whenever the current ultimate points increases or gets
/// expended.
/// </summary>
public class UltimateBar : MonoBehaviour
{
    [SerializeField] private Slider _Slider;
    [SerializeField] private UltimateSO _UltimateOS;
    
    private void Start()
    {
        if (_Slider == null)
        {
            Debug.Log("Ultimate Slider is null");
        }
        
        if (_UltimateOS == null)
        {
            Debug.Log("Ultimate Data is null");
        }
        
        SetMaxUltimate(_UltimateOS.RequiredUltimatePoints);
    }

    /// <summary>
    /// Sets the maximum ultimate value of the slider.
    /// This is usually called once when the character is created or
    /// initialized.
    /// </summary>
    private void SetMaxUltimate(int ultimate)
    {
        // Set the slider's maximum value.
        _Slider.maxValue = ultimate;
    }

    /// <summary>
    /// Updates the slider to display the current health.
    /// </summary>
    public void SetUltimate(int ultimate) => _Slider.value = ultimate;
}