using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeAttackRadius : AttackRadius
{
    public NavMeshAgent Agent;
    //public Bullett BulletPrefab;
    public Vector3 BulletSpawnOffset = new Vector3(0, 1, 0);
    public LayerMask Mask;
    //private ObjectPool BulletPool;
    [SerializeField]
    private float SpherecastRadius = 0.1f;
    private RaycastHit Hit;
    private IDamageable targetDamageable;
    //private Bullett bullet;

    [SerializeField]
    private GameObject bulletSpawnPoint = null;

    [SerializeField]
    private TrailRenderer BulletTrail;
    
    [SerializeField]
    private ParticleSystem ImpactParticleSystem;

    [SerializeField]
    private ParticleSystem FleshImpactParticleSystem;

    [SerializeField]
    private Vector3 BulletSpread = new Vector3(0.05f, 0.05f, 0.05f);

    [SerializeField]
    private float BulletSpeed = 0.25f;

    //protected override void Awake()
    //{
    //    base.Awake();
    //    BulletPool = ObjectPool.CreateInstance(BulletPrefab, 10);
    //}



    private bool HasLineOfSightTo(Transform Target)
    {
        if (Physics.SphereCast(transform.position + BulletSpawnOffset, SpherecastRadius, ((Target.position + BulletSpawnOffset) - (transform.position + BulletSpawnOffset)).normalized, out Hit, Collider.radius, Mask))
        {
            IDamageable damageable;
            if (Hit.collider.TryGetComponent<IDamageable>(out damageable))
            {
                return damageable.GetTransform() == Target;
            }
        }
        return false;
    }
    /*
    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;

        while(time < 1) {
            Trail.transform.position = Vector3.Lerp(startPosition, Hit.point, time);
            time += Time.deltaTime / Trail.time;
            yield return null;
        }
        Trail.transform.position = Hit.point;
        if(Hit.collider.CompareTag("Player")) {
                //Instantiate(FleshImpactParticleSystem, Hit.point, Quaternion.LookRotation(Hit.normal));
                var player = Hit.rigidbody.GetComponent<Player>();
                //player.TakeDamage(Damage);
            }
        else {
            Instantiate(ImpactParticleSystem, Hit.point, Quaternion.LookRotation(Hit.normal));
        }
        
        Destroy(Trail.gameObject, Trail.time);
    }
    */
    private IEnumerator SpawnTrail(IDamageable idamageable, TrailRenderer Trail, Vector3 HitPoint, RaycastHit hit)
    {
        Vector3 startPosition = Trail.transform.position;

        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float startingDistance = distance;

        while (distance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (distance / startingDistance));
            distance -= Time.deltaTime * BulletSpeed;

            yield return null;
        }

        Trail.transform.position = HitPoint;

        if (hit.collider.CompareTag("Player"))
        {
            idamageable.TakeDamage(Damage);
            Instantiate(FleshImpactParticleSystem, Hit.point, Quaternion.LookRotation(hit.normal));
        } else
        {
            Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(hit.normal));
        }
        yield return new WaitForSeconds(Trail.time);
        Destroy(Trail.gameObject, Trail.time);
    }

    protected override IEnumerator Attack()
    {
        WaitForSeconds Wait = new WaitForSeconds(AttackDelay);

        yield return Wait;

        while(Damagesables.Count > 0)
        {
            for (int i = 0; i < Damagesables.Count; i++)
            {
                if (HasLineOfSightTo(Damagesables[i].GetTransform()))
                {
                    targetDamageable = Damagesables[i];
                    OnAttack?.Invoke(Damagesables[i]);
                    Agent.isStopped = true;
                    break;
                }
            }

            if (targetDamageable != null)
            {
                Vector3 direction = ((targetDamageable.GetTransform().position + BulletSpawnOffset) - (transform.position + BulletSpawnOffset)).normalized + new Vector3(
                    Random.Range(-BulletSpread.x, BulletSpread.x),
                    Random.Range(-BulletSpread.y, BulletSpread.y),
                    Random.Range(-BulletSpread.z, BulletSpread.z)
                    );
                direction.y = 0;
                direction.Normalize();
                TrailRenderer trail = Instantiate(BulletTrail, transform.position + BulletSpawnOffset, Quaternion.identity);
                if (Physics.Raycast(transform.position + BulletSpawnOffset, direction, out RaycastHit hit, float.MaxValue, Mask)) 
                {
                    StartCoroutine(SpawnTrail(targetDamageable, trail, hit.point, hit));
                    targetDamageable = null;
                } 
            }
            else
            {
                Agent.isStopped = false;
            }

            yield return Wait;

            if(targetDamageable == null || !HasLineOfSightTo(targetDamageable.GetTransform()))
            {
                Agent.isStopped = false;
            }

            Damagesables.RemoveAll(DisabledDamageables);
        }
        Agent.isStopped = false;
        AttackCoroutine = null;
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if(AttackCoroutine == null)
        {
            Agent.isStopped = false;
        }
    }
} 
