using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Skrypt powodujący miganie świateł
/// </summary>
public class LightFlickering : MonoBehaviour
{
    /// <summary>
    /// Zmienna tablicowa ze światłami
    /// </summary>
    public Light[] Lights;

    /// <summary>
    /// Minimalny czas trwania zgaszonego/włączonego światła
    /// </summary>
    public float minTime;

    /// <summary>
    /// Maksymalny czas trwania zgaszonego/włączonego światła
    /// </summary>
    public float maxTime;

    /// <summary>
    /// Zmienna definiująca opóźnienie rozpoczęcia korutyny odpowiedzialnej za miganie świateł
    /// </summary>
    public float delay;

    /// <summary>
    /// Metoda opakowująca metodę Run(), aby możliwe było opóźnienie korutyny
    /// </summary>
    void Start()
    {
        Invoke("Run", delay);
    }

    /// <summary>
    /// Korutyna migania świateł
    /// </summary>
    /// <returns></returns>
    IEnumerator Flickering()
    {
        while(true) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minTime, maxTime));
            foreach(Light light in Lights) {
                 light.enabled = !light.enabled;
            }               
        }
    }

    /// <summary>
    /// Metoda rozpoczyna korutynę migania świateł
    /// </summary>
    void Run()
    {
        StartCoroutine(Flickering());
    }   
}
