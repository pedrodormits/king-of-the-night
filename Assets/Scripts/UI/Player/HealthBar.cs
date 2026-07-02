using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _Slider;

    public void SetMaxHealth(int health)
    {
        _Slider.maxValue = health;
        _Slider.value = health;
    }

    public void SetHealth(int health) => _Slider.value = health;
}