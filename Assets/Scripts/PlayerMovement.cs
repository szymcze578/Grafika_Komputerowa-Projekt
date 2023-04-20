using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController m_charCont;
    private Animator anim;

    float m_horizontal;
    float m_vertical;

    public float PlayerSpeed = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        m_charCont = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_horizontal = Input.GetAxis("Horizontal");
        m_vertical = Input.GetAxis("Vertical");

        anim.SetFloat("vertical", m_vertical);

        Vector3 m_playerMovement = new Vector3(m_horizontal, 0f, m_vertical) * PlayerSpeed;

        m_charCont.Move(m_playerMovement);
    }
}
