using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickering : MonoBehaviour
{
    public Light[] Lights;

    public float minTime;
    public float maxTime;

    void Start()
    {
        StartCoroutine(Flickering());
    }

    IEnumerator Flickering()
    {
        while(true) {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minTime, maxTime));
            foreach(Light light in Lights) {
                 light.enabled = !light.enabled;
                 Console.Write("flick");
            }               
        }
    }
}
