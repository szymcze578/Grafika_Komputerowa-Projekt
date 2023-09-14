using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Reprezentuje pasek życia
/// </summary>
public class HealthBar : MonoBehaviour
{
    /// <summary>
    /// Reprezentuje pasek na ekranie, gdzie wyświetlane jest życie
    /// </summary>
    public Slider slider;

    /// <summary>
    /// Ustawia maksymalną liczbę punktów życia
    /// </summary>
    /// <param name="health"> Ilość punktów życia do ustawienia </param>
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    /// <summary>
    /// Aktualizuje wartosc na pasku zycia
    /// </summary>
    /// <param name="health"> Ilość punktów życia do ustawienia </param>
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
