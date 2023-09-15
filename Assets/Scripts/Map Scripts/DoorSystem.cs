using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// System fizyki drzwi
/// </summary>
public class DoorSystem : MonoBehaviour
{
    /// <summary>
    /// Zmienna określająca oś obrotu drzwi
    /// </summary>
    private HingeJoint joint;

    /// <summary>
    /// Zmienna określająca limit obrotu drzwi
    /// </summary>
    private JointLimits limits;

    /// <summary>
    /// Zmienna definiująca, czy drzwi są odblokowane
    /// </summary>
    public bool unlocked;

    void Start()
    {
        joint = GetComponent<HingeJoint>();
        limits = joint.limits;
    }

    /// <summary>
    /// Metoda ustawia odpowiednie parametry drzwi (limity), w zależności od tego czy są odblokowane czy zablokowane
    /// </summary>
    void Update()
    {
        if(unlocked) {
                limits.min = -90;
                limits.max = 90;
                joint.limits = limits;
            }
            else {
                limits.min = 0;
                limits.max = 0;
                joint.limits = limits;
            }
    }
}
