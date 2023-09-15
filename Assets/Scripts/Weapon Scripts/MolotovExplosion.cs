using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

/// <summary>
/// Skrypt generujący ogień po wybuchu granatu zapalającego
/// </summary>
public class MolotovExplosion : MonoBehaviour
{
    /// <summary>
    /// Komponent rigidbody granatu
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Efekt ognia
    /// </summary>
    public GameObject fireEffect;

    /// <summary>
    /// Efekt dźwiękowy eksplozji
    /// </summary>
    public AudioClip explosionClip;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Metoda wykrywająca kolizję granatu z obiektem
    /// </summary>
    /// <param name="collision"> Wykryta kolizja </param>
    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    /// <summary>
    /// Metoda wywołująca eksplozję granat
    /// </summary>
    private void Explode()
    {
        rb.detectCollisions = false;
        Instantiate(fireEffect, transform.position, fireEffect.transform.rotation);
        Destroy(gameObject);
    }
}
