using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ustawienia kamery
/// </summary>
public class CameraFollow : MonoBehaviour
{

    /// <summary>
    /// Obiekt gracza na scenie
    /// </summary>
    public Transform Player;

    /// <summary>
    /// Wysokość kamery nad graczem
    /// </summary>
    float camOffsetZ;
    
    void Start()
    {

        camOffsetZ = gameObject.transform.position.z - Player.position.z;
  
    }

    /// <summary>
    /// Metoda aktualizuje pozycje kamery, aby podążała za graczem
    /// </summary>
    void Update()
    {

        Vector3 m_cameraPos = new Vector3(Player.position.x, gameObject.transform.position.y, Player.position.z + camOffsetZ);
        gameObject.transform.position = m_cameraPos;
    }
}
