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

    public Image bloodEffect;
    public float fadeSpeed = 0.5f;


    public void Awake()
    {
        bloodEffect.color = new Color(bloodEffect.color.r, bloodEffect.color.g, bloodEffect.color.b, 0);
    }

    public void TakeDamage(int Damage)
    {
        Health -= Damage;
        bloodEffect.color = new Color(bloodEffect.color.r, bloodEffect.color.g, bloodEffect.color.b, 255);

        if (Health <= 0)
        {
            gameObject.SetActive(false);
        }
        healthBar.SetHealth(Health);
        StartCoroutine(dissapearBloodEffect());
        
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

    public IEnumerator dissapearBloodEffect()
    {
        Color currentColor = bloodEffect.color;

        for (float i = 1; i >= 0; i -= fadeSpeed * Time.deltaTime)
        {
            bloodEffect.color = new Color(currentColor.r, currentColor.g, currentColor.b, i);
            yield return null;
        }

        yield return null;
    }
}
