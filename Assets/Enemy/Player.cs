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
    public bool increaseHealth(int amount)
    {
        if(Health == 100) {
            return false;
        }

        Health += amount;

        if (Health > 100)
            Health = 100;

        healthBar.SetHealth(Health);
        return true;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
