using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    /*
     * Ustawia pasek zycia
     */
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    /*
     * Aktualizuje wartosc na pasku zycia
     */
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
