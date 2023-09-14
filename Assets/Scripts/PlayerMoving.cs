using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reprezentuje ustawienia poruszania się gracza
/// </summary>
public class PlayerMoving : MonoBehaviour
{
    /// <summary>
    /// Prędkość poruszania się gracza
    /// </summary>
    public float MovementSpeed = 5f;

    /// <summary>
    /// Prędkość obracania się gracza
    /// </summary>
    public float rotationSpeed = 900f;
    
    /// <summary>
    /// Głośność kroków gracza
    /// </summary>
    public float FootstepVolume;

    /// <summary>
    /// Tablica z efektami dźwiękowymi kroków
    /// </summary>
    public AudioClip[] FootstepClip;

    /// <summary>
    /// Reprezentuje komponent źródła dźwięku przypiętego do obiektu gracza
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// Odstęp pomiędzy efektami dźwiękowymi kroków
    /// </summary>
    public float StepsDelay;

    /// <summary>
    /// Zlicza czas od poprzedniego kroku
    /// </summary>
    private float timepassed;

    /// <summary>
    /// Reprezentuje komponent Rigidbody przypięty do obiektu gracza
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Wektor reprezentujący kierunek poruszania się gracza
    /// </summary>
    private Vector3 movement;

    /// <summary>
    /// Reprezentuje komponent odpowiedzialny za animacje gracza
    /// </summary>
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.enabled = true;
        timepassed = 0f;
    }

    /// <summary>
    /// Funkcja pobiera aktualną pozycję celownika na ekranie i rotuje gracza w tym kierunku
    /// </summary>
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

        timepassed += Time.deltaTime;
        if (movement != Vector3.zero && !audioSource.isPlaying && timepassed >= StepsDelay) {
            PlayFootstepClip(audioSource);
            timepassed = 0f;
        }

    }

    void FixedUpdate()
    {
        moveCharacter(movement);
    }

    /// <summary>
    /// Funkcja zmienia prędkość gracza i kierunek poruszania się
    /// </summary>
    /// <param name="direction"> Kierunek poruszania się gracza </param>
    void moveCharacter(Vector3 direction)
    {
        rb.velocity = direction * MovementSpeed;
    }

    /// <summary>
    /// Funkcja aktualizuje animację biegania gracza, aby jej kierunek zgadzał się z kierunkiem gdzie celuje gracz
    /// </summary>
    /// <param name="normalizedLookingAt"> Kierunek, w którym celuje gracz</param>
    /// <param name="animator"> Komponent odpowiedzialny za animacje gracza </param>
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

    /// <summary>
    /// Funkcja odtwarza efekt dźwiękowy kroku
    /// </summary>
    /// <param name="audioSource"> Docelowy komponent źródła dźwięku, w którym odtworzony zostanie dźwięk </param>
    public void PlayFootstepClip(AudioSource audioSource) {
        if(FootstepClip != null) {
            audioSource.PlayOneShot(FootstepClip[Random.Range(0, FootstepClip.Length)], FootstepVolume);
        }
    }
}
