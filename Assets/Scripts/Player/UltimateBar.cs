using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ultimate controls the UI slider that displays the player's ultimate points.
/// The slider is updated whenever the current ultimate points increases or gets expended.
/// </summary>
public class UltimateBar : MonoBehaviour
{
    [SerializeField] private Slider _Slider;
    
    /// <summary>
    /// Sets the maximum ultimate value of the slider.
    /// This is usually called once when the character is created or initialized.
    /// </summary>
    public void SetMaxUltimate(int ultimate) // Reference to the UI Slider used as the ultimate bar.
    {
        _Slider.maxValue = ultimate; // Set the slider's maximum value.
        _Slider.value = ultimate; // Fill the slider completely.
    }

    /// <summary>
    /// Updates the slider to display the current health.
    /// </summary>
    public void SetHealth(int health) => _Slider.value = health;
}