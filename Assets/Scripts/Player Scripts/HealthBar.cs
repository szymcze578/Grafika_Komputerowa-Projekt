using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Ustawienia paska życia
/// </summary>
public class HealthBar : MonoBehaviour
{

    /// <summary>
    /// Pasek życia na ekranie
    /// </summary>
    public Slider slider;
    
    /// <summary>
    /// Ustawia maksymalną ilość punktów życia
    /// </summary>
    /// <param name="health"> Ilość punktów życia do ustawienia </param>
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    /// <summary>
    /// Aktualizuje pasek życia na ekranie, aby wskazywał aktualną wartość
    /// </summary>
    /// <param name="health"> Ilość punktów życia do wyświetlenia </param>
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
