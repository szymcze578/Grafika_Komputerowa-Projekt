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

    private Coroutine FollowCoroutine;

    private void Awake()
    {
        Agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Animator.SetBool(isMoving, Agent.velocity.magnitude > 0.01f);
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
            Agent.SetDestination(Target.transform.position);

            yield return Wait;
        }
    }
}
