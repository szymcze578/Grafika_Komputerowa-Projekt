using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ustawienia światła podążającego za graczem
/// </summary>
public class LightFollow : MonoBehaviour
{

    /// <summary>
    /// Obiekt gracza na scenie
    /// </summary>
    public Transform Player;

    /// <summary>
    /// Wysokość ustawienia światła nad graczem
    /// </summary>
    float lightOffsetZ;

    /// <summary>
    /// Metoda aktualizuje pozycje światła, aby podążało za graczem
    /// </summary>
    void Update()
    {
        Vector3 m_lightPos = new Vector3(Player.position.x, gameObject.transform.position.y, Player.position.z + lightOffsetZ);
        gameObject.transform.position = m_lightPos;
    }
}
