using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// HealthBar controls the UI slider that displays the player's or enemy's health.
/// The slider is updated whenever the maximum health or current health changes.
/// </summary>
public class HealthBar : MonoBehaviour
{
    // Reference to the UI Slider used as the health bar.
    [SerializeField] private Slider _Slider;
    
    private void Start()
    {
        if (_Slider == null)
        {
            Debug.Log("Health Slider is null");
        }
    }

    /// <summary>
    /// Sets the maximum health value of the slider.
    /// This is usually called once when the character is created or initialized.
    /// </summary>
    public void SetMaxHealth(int health)
    {
        // Set the slider's maximum value.
        _Slider.maxValue = health;
        
        // Fill the slider completely.
        _Slider.value = health;
    }

    /// <summary>
    /// Updates the slider to display the current health.
    /// </summary>
    public void SetHealth(int health) => _Slider.value = health;
}