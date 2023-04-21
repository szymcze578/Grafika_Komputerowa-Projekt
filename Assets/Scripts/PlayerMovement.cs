using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController m_charCont;
    private Animator anim;

    float m_horizontal;
    float m_vertical;

    public float PlayerSpeed = 0.03f;
    public float rotationSpeed = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        m_charCont = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPosition = ray.GetPoint(distance);
            Vector3 direction = targetPosition - transform.position;

            direction.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }


        m_horizontal = Input.GetAxis("Horizontal");
        m_vertical = Input.GetAxis("Vertical");

        anim.SetFloat("vertical", m_vertical);

        Vector3 m_playerMovement = new Vector3(m_horizontal, 0f, m_vertical) * PlayerSpeed;

        m_charCont.Move(m_playerMovement);
    }
}
