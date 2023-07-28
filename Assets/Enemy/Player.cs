using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int Health = 100;
    public int points = 0;


    public HealthBar healthBar;

    public void TakeDamage(int Damage)
    {
        Health -= Damage;
        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
        healthBar.SetHealth(Health);
    }
    public void increaseHealth(int amount)
    {
        Health+=amount;
        healthBar.SetHealth(Health);
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
