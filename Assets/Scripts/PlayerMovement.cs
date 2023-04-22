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

            UpdateAnimator(direction, anim);

            direction.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }


        m_horizontal = Input.GetAxis("Horizontal");
        m_vertical = Input.GetAxis("Vertical");

        //anim.SetFloat("vertical", m_vertical);
        //anim.SetFloat("horizontal", m_horizontal);

        Vector3 m_playerMovement = new Vector3(m_horizontal, 0f, m_vertical) * PlayerSpeed;

        m_charCont.Move(m_playerMovement);
    }

    private void UpdateAnimator(Vector3 normalizedLookingAt, Animator animator)
    {
        float verticalMagnitude = 0;
        float horizontalMagnitude = 0;

        Vector3 axisVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        if (axisVector.magnitude > 0) {
            //Vector3 normalizedLookingAt = lookedAtPoint - transform.position;
            normalizedLookingAt.Normalize();
            verticalMagnitude = Mathf.Clamp (
                    Vector3.Dot (axisVector, normalizedLookingAt), -1, 1
            );
 
            Vector3 perpendicularLookingAt = new Vector3 (
                    normalizedLookingAt.z, 0, -normalizedLookingAt.x
            );
            horizontalMagnitude = Mathf.Clamp (
                    Vector3.Dot (axisVector, perpendicularLookingAt), -1, 1
            );
 
            //animator.SetBool("isMoving", true);
 
        } 
        else {
            //animator.SetBool("isMoving", false);
        }
 
        // update the animator parameters
        animator.SetFloat ("vertical", verticalMagnitude);
        animator.SetFloat ("horizontal", horizontalMagnitude);
    }
}
