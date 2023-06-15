using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int Health = 300;
    public Text hudHealth;

    public void TakeDamage(int Damage)
    {
        Health -= Damage;
        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        hudHealth.text = "Health: " + Health;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
