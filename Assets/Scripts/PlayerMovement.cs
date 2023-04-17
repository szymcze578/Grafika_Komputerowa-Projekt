using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController m_charCont;
    float m_horizontal;
    float m_vertical;

    public float PlayerSpeed = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        m_charCont = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        m_horizontal = Input.GetAxis("Horizontal");
        m_vertical = Input.GetAxis("Vertical");

        Vector3 m_playerMovement = new Vector3(m_horizontal, 0f, m_vertical) * PlayerSpeed;

        m_charCont.Move(m_playerMovement);
    }
}
