using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFollow : MonoBehaviour
{

    public Transform Player;

    float lightOffsetZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 m_lightPos = new Vector3(Player.position.x, gameObject.transform.position.y, Player.position.z + lightOffsetZ);
        gameObject.transform.position = m_lightPos;

    }
}
