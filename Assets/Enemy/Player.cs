using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField]
    private int Health = 100;
    public int points = 0;

    public HealthBar healthBar;

    public Image bloodEffect;
    public Image gameOverScreen;
    public float fadeSpeed = 0.5f;



    public void Awake()
    {
        bloodEffect.color = new Color(bloodEffect.color.r, bloodEffect.color.g, bloodEffect.color.b, 0);
        gameOverScreen.gameObject.SetActive(false);
    }

    public void TakeDamage(int Damage)
    {
        if (Health >= 0)
        {
            StartCoroutine(dissapearBloodEffect());
        }

        Health -= Damage;
        bloodEffect.color = new Color(bloodEffect.color.r, bloodEffect.color.g, bloodEffect.color.b, 255);

        if (Health <= 0)
        {
            gameObject.SetActive(false);
            gameOverScreen.gameObject.SetActive(true);
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

    public void playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void returnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
