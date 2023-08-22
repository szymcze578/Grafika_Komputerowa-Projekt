using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolableObject, IDamageable
{
    public AttackRadius AttackRadius;
    public Animator Animator;
    public EnemyMovement Movement;
    public NavMeshAgent Agent;
    public EnemyScriptableObject EnemyScriptableObject;
    public int Health = 100;

    private Coroutine LookCoroutine;
    private const string ATTACK_TRIGGER = "Attack";

    public WeaponSystem weaponSystem = null;
    public WeaponAudioConfig audioConfig;
    private AudioSource audioSource;

    public Player player;

    private void Awake()
    {
        AttackRadius.OnAttack += OnAttack;
        weaponSystem = GameObject.Find("GunHolder").GetComponent<WeaponSystem>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnAttack(IDamageable Target)
    {
        Animator.SetTrigger(ATTACK_TRIGGER);
        audioConfig.PlayShootingClip(audioSource);

        if (LookCoroutine != null)
        {
            StopCoroutine(LookCoroutine);
        }

        LookCoroutine = StartCoroutine(LookAt(Target.GetTransform()));
    }

    private IEnumerator LookAt(Transform Target)
    {
        Quaternion lookRotation = Quaternion.LookRotation(Target.position - transform.position);
        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * 2;
            yield return null;
        }

        transform.rotation = lookRotation;
    }

    public virtual void OnEnable()
    {
        SetupAgentFromConfiguration();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        Agent.enabled = false;
    }

    public virtual void SetupAgentFromConfiguration()
    {
        Agent.acceleration = EnemyScriptableObject.Acceleration;
        Agent.angularSpeed = EnemyScriptableObject.AngularSpeed;
        Agent.areaMask = EnemyScriptableObject.AreaMask;
        Agent.avoidancePriority = EnemyScriptableObject.AvoidancePriority;
        Agent.baseOffset = EnemyScriptableObject.BaseOffset;
        Agent.height = EnemyScriptableObject.Height;
        Agent.obstacleAvoidanceType = EnemyScriptableObject.ObstacleAvoidanceType;
        Agent.radius = EnemyScriptableObject.Radius;
        Agent.speed = EnemyScriptableObject.Speed;
        Agent.stoppingDistance = EnemyScriptableObject.StoppingDistance;

        Movement.UpdateSpeed = EnemyScriptableObject.AIUpdateInterval;

        Health = EnemyScriptableObject.Health;
        AttackRadius.Collider.radius = EnemyScriptableObject.AttackRadius;
        AttackRadius.AttackDelay = EnemyScriptableObject.AttackDelay;
        AttackRadius.Damage = EnemyScriptableObject.Damage;

        if(EnemyScriptableObject.IsRanged) {
            Animator.SetBool("hasPistol", true);
        }
        else {
            Animator.SetBool("hasPistol", false);
        }
    }

    public void TakeDamage(int Damage)
    {
        Health -= Damage;
        if (Health <= 0)
        {
            player.points += 10;
            gameObject.SetActive(false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
