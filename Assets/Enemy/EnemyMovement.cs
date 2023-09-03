using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
{
    public Transform Target;
    public float UpdateSpeed = 0.1f;
    [SerializeField]
    private Animator Animator;

    private NavMeshAgent Agent;

    private const string isMoving = "isMoving";

    public Coroutine FollowCoroutine;


    public float FootstepVolume;
    public AudioClip[] FootstepClip;
    private AudioSource audioSource;
    public float StepsDelay;
    private float timepassed;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
        audioSource = gameObject.GetComponent<AudioSource>();
        timepassed = 0f;
    }

    private void Update()
    {
        Animator.SetBool(isMoving, Agent.velocity.magnitude > 0.01f);

        timepassed += Time.deltaTime;
        if (Agent.velocity.magnitude > 0.01f && !audioSource.isPlaying && timepassed >= StepsDelay) {
            PlayFootstepClip(audioSource);
            timepassed = 0f;
        }
    }

    public void StartChasing()
    {
        if (FollowCoroutine == null)
        {
            FollowCoroutine = StartCoroutine(FollowTarget());
        }
        else
        {
            Debug.LogWarning("Wywolano metode StartChasing na przeciwniku ktory juz sciga gracza!");
        }
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateSpeed);
        while(enabled)
        {
            Agent.SetDestination(Target.transform.position - (Target.transform.position - transform.position).normalized * 0.5f);

            yield return Wait;
        }
    }

    public void PlayFootstepClip(AudioSource audioSource) {
        if(FootstepClip != null) {
            audioSource.PlayOneShot(FootstepClip[Random.Range(0, FootstepClip.Length)], FootstepVolume);
        }
    }
}
