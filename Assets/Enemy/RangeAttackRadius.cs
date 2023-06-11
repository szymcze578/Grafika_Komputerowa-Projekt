using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeAttackRadius : AttackRadius
{
    public NavMeshAgent Agent;
    public Bullett BulletPrefab;
    public Vector3 BulletSpawnOffset = new Vector3(0, 1, 0);
    public LayerMask Mask;
    private ObjectPool BulletPool;
    [SerializeField]
    private float SpherecastRadius = 0.1f;
    private RaycastHit Hit;
    private IDamageable targetDamageable;
    private Bullett bullet;

    protected override void Awake()
    {
        base.Awake();
        BulletPool = ObjectPool.CreateInstance(BulletPrefab, Mathf.CeilToInt((1 / AttackDelay) * BulletPrefab.AutoDestroyTime));
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
                    //Agent.enabled = false;
                    break;
                }
            }

            if (targetDamageable != null)
            {
                PoolableObject poolableObject = BulletPool.GetObject();
                if (poolableObject != null)
                {
                    bullet = poolableObject.GetComponent<Bullett>();

                    bullet.Damage = Damage;
                    bullet.transform.position = transform.position + BulletSpawnOffset;
                    bullet.transform.rotation = Agent.transform.rotation;
                    bullet.Rigidbody.AddForce(Agent.transform.forward * BulletPrefab.MoveSpeed, ForceMode.VelocityChange);
                }
            }
            else
            {
                Agent.isStopped = false;
                //Agent.enabled = true;
            }

            yield return Wait;

            if(targetDamageable == null || !HasLineOfSightTo(targetDamageable.GetTransform()))
            {
                Agent.isStopped = false;
                //Agent.enabled = true;
            }

            Damagesables.RemoveAll(DisabledDamageables);
        }

        Agent.isStopped = false;
        //Agent.enabled = true;
        AttackCoroutine = null;
    }

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

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if(AttackCoroutine == null)
        {
            Agent.isStopped = false;
            //Agent.enabled = true;
        }
    }
}
