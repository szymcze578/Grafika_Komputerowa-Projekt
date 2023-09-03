using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Enemy Configuration", menuName = "ScriptableObject/Enemy Configuration")]
public class EnemyScriptableObject : ScriptableObject
{
    public Enemy Prefab;
    public int Health = 100;
    public float AttackDelay = 1f;
    public int Damage = 5;
    public float AttackRadius = 1.5f;
    public bool IsRanged = false;

    public float AIUpdateInterval = 0.1f;
    public float Acceleration = 8;
    public float AngularSpeed = 120;
    public int AreaMask = -1;
    public int AvoidancePriority = 50;
    public float BaseOffset = 0;
    public float Height = 2f;
    public ObstacleAvoidanceType ObstacleAvoidanceType = ObstacleAvoidanceType.LowQualityObstacleAvoidance;
    public float Radius = 0.5f;
    public float Speed = 3f;
    public float StoppingDistance = 0.5f;

    public EnemyScriptableObject scaleUpForLevel(ScalingScriptableObject Scaling, int Level)
    {
        EnemyScriptableObject scaledUpEnemy = CreateInstance<EnemyScriptableObject>();

        scaledUpEnemy.name = name;
        scaledUpEnemy.Prefab = Prefab;
        scaledUpEnemy.Health = Mathf.FloorToInt(Health * Scaling.HealthCurve.Evaluate(Level));
        scaledUpEnemy.AttackDelay = AttackDelay;
        scaledUpEnemy.Damage = Mathf.FloorToInt(Damage * Scaling.DamageCurve.Evaluate(Level));
        scaledUpEnemy.AttackRadius = AttackRadius;
        scaledUpEnemy.IsRanged = IsRanged;
        scaledUpEnemy.AIUpdateInterval = AIUpdateInterval;
        scaledUpEnemy.Acceleration = Acceleration;
        scaledUpEnemy.AngularSpeed = AngularSpeed;
        scaledUpEnemy.AreaMask = AreaMask;
        scaledUpEnemy.AvoidancePriority = AvoidancePriority;
        scaledUpEnemy.BaseOffset = BaseOffset;
        scaledUpEnemy.Height = Height;
        scaledUpEnemy.ObstacleAvoidanceType = ObstacleAvoidanceType;
        scaledUpEnemy.Radius = Radius;
        scaledUpEnemy.Speed = Speed * Scaling.SpeedCurve.Evaluate(Level);
        scaledUpEnemy.StoppingDistance = StoppingDistance;

        return scaledUpEnemy;
    }

    public void SetupEnemy(Enemy enemy)
    {
        enemy.Agent.acceleration = Acceleration;
        enemy.Agent.angularSpeed = AngularSpeed;
        enemy.Agent.areaMask = AreaMask;
        enemy.Agent.avoidancePriority = AvoidancePriority;
        enemy.Agent.baseOffset = BaseOffset;
        enemy.Agent.height = Height;
        enemy.Agent.obstacleAvoidanceType = ObstacleAvoidanceType;
        enemy.Agent.radius = Radius;
        enemy.Agent.speed = Speed;
        enemy.Agent.stoppingDistance = StoppingDistance;

        enemy.Health = Health;

        enemy.Movement.UpdateSpeed = AIUpdateInterval;

        (enemy.AttackRadius.Collider == null ? enemy.AttackRadius.GetComponent<SphereCollider>() : enemy.AttackRadius.Collider).radius = AttackRadius;
        enemy.AttackRadius.AttackDelay = AttackDelay;
        enemy.AttackRadius.Damage = Damage;
    }
}
