using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reprezentuje ustawienia światła podążającego za graczem i definiującego jego pole widzenia
/// </summary>
public class LightFollow : MonoBehaviour
{

    /// <summary>
    /// Reprezentuje obiekt gracza na scenie
    /// </summary>
    public Transform Player;

    /// <summary>
    /// Definiuje wysokość, na jakiej umieszone nad graczem jest światło
    /// </summary>
    float lightOffsetZ;

    /// <summary>
    /// Aktualizuje pozycję światła, aby podążało za graczem
    /// </summary>
    void Update()
    {
        Vector3 m_lightPos = new Vector3(Player.position.x, gameObject.transform.position.y, Player.position.z + lightOffsetZ);
        gameObject.transform.position = m_lightPos;
    }
}
