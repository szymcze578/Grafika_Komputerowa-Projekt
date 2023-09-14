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
    public delegate void DeathEvent(Enemy enemy);
    public DeathEvent OnDie;

    [SerializeField]
    private Transform RagdollRoot;
    [SerializeField]
    private float FadeOutDelay = 10f;
    [SerializeField]
    private GameObject attackRadiusObject;
    private Rigidbody[] Rigidbodies;
    private CharacterJoint[] Joints;
    private CapsuleCollider capsule;
    //private List<Vector3> positionsRigid;
    //private List<Quaternion> rotationsRigid;
    //private List<Vector3> positionsJoints;
    //private List<Quaternion> rotationsJoints;
    private Coroutine LookCoroutine;
    private const string ATTACK_TRIGGER = "Attack";

    public WeaponSystem weaponSystem = null;
    public WeaponAudioConfig audioConfig;
    private AudioSource audioSource;
        
    public Player player;

    private void Awake()
    {
        //positionsRigid = new List<Vector3>();
        //rotationsRigid = new List<Quaternion>();
        //positionsJoints = new List<Vector3>();
        //rotationsJoints = new List<Quaternion>();
        AttackRadius.OnAttack += OnAttack;
        weaponSystem = GameObject.Find("GunHolder").GetComponent<WeaponSystem>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Rigidbodies = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        Joints = RagdollRoot.GetComponentsInChildren<CharacterJoint>();
        capsule = GetComponent<CapsuleCollider>();
        //SaveDefaultRagdollPosition();
    }

    private void OnAttack(IDamageable Target)
    {
        Animator.SetTrigger(ATTACK_TRIGGER);
        
        if(EnemyScriptableObject.IsRanged) {
            audioConfig.PlayShootingClip(audioSource);
        }
        

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
        OnDie = null;
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
            OnDie?.Invoke(this);
            EnableRagdoll();
            StartCoroutine(FadeOut());
        }
    }

    public void EnableAnimator()
    {
        Animator.enabled = true;
        Agent.enabled = true;
        foreach (CharacterJoint joint in Joints)
        {
            joint.enableCollision = false;
        }
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }
    }

    public void EnableRagdoll()
    {
        attackRadiusObject.SetActive(false);
        capsule.enabled = false;
        Animator.enabled = false;
        Agent.enabled = false;

        if (Movement.FollowCoroutine != null)
        {
            Movement.StopCoroutine(Movement.FollowCoroutine);
            Movement.FollowCoroutine = null;
        }

        foreach (CharacterJoint joint in Joints)
        {
            joint.enableCollision = true;
        }
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
        }
    }

    public void DisableAllRigidbodies()
    {
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;
            rigidbody.isKinematic = true;
        }
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(FadeOutDelay);

        DisableAllRigidbodies();

        float time = 0;
        while (time < 1)
        {
            transform.position += Vector3.down * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }

        attackRadiusObject.SetActive(true);
        capsule.enabled = true;
        EnableAnimator();
        //ResetRagdollPosition();
        gameObject.SetActive(false);
    }

    //private void ResetRagdollPosition()
    //{
    //    for (int i = 0; i < Rigidbodies.Length; i++)
    //    {
    //        Rigidbodies[i].transform.localPosition = positionsRigid[i];
    //        Rigidbodies[i].transform.localRotation = rotationsRigid[i];
    //    }

    //    for (int i = 0; i < Joints.Length; i++)
    //    {
    //        Joints[i].transform.localPosition = positionsJoints[i];
    //        Joints[i].transform.localRotation = rotationsJoints[i];
    //    }
    //}

    //private void SaveDefaultRagdollPosition()
    //{
    //    foreach (Rigidbody rigidbody in Rigidbodies)
    //    {
    //        positionsRigid.Add(rigidbody.transform.localPosition);
    //        rotationsRigid.Add(rigidbody.transform.localRotation);
    //    }

    //    foreach (Joint joint in Joints)
    //    {
    //        positionsJoints.Add(joint.transform.localPosition);
    //        rotationsJoints.Add(joint.transform.localRotation);
    //    }
    //}

    public Transform GetTransform()
    {
        return transform;
    }
}
