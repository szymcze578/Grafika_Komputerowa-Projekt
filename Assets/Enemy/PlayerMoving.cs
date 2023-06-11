using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    public float MovementSpeed = 5f;
    public float rotationSpeed = 900f;
    private Rigidbody rb;
    private Vector3 movement;
    private Animator anim;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
    }

    void FixedUpdate()
    {
        moveCharacter(movement);
    }
    void moveCharacter(Vector3 direction)
    {
        rb.velocity = direction * MovementSpeed;
    }

    private void UpdateAnimator(Vector3 normalizedLookingAt, Animator animator)
    {
        float verticalMagnitude = 0;
        float horizontalMagnitude = 0;

        Vector3 axisVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (axisVector.magnitude > 0)
        {
            //Vector3 normalizedLookingAt = lookedAtPoint - transform.position;
            normalizedLookingAt.Normalize();
            verticalMagnitude = Mathf.Clamp(
                    Vector3.Dot(axisVector, normalizedLookingAt), -1, 1
            );

            Vector3 perpendicularLookingAt = new Vector3(
                    normalizedLookingAt.z, 0, -normalizedLookingAt.x
            );
            horizontalMagnitude = Mathf.Clamp(
                    Vector3.Dot(axisVector, perpendicularLookingAt), -1, 1
            );

            //animator.SetBool("isMoving", true);

        }
        else
        {
            //animator.SetBool("isMoving", false);
        }

        // update the animator parameters
        animator.SetFloat("vertical", verticalMagnitude);
        animator.SetFloat("horizontal", horizontalMagnitude);
    }
}
