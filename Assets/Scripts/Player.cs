using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Reprezentuje obiekt gracza
/// </summary>
public class Player : MonoBehaviour, IDamageable
{
    /// <summary>
    /// Liczba punktów życia gracza
    /// </summary>
    [SerializeField]
    private int Health = 100;
    
    /// <summary>
    /// Liczba punktów, które gracz zbiera podczas rozgrywki
    /// </summary>
    public int points = 0;

    /// <summary>
    /// Reprezentuje pasek z życiem
    /// </summary>
    public HealthBar healthBar;

    /// <summary>
    /// Efekt krwi na ekranie
    /// </summary>
    public Image bloodEffect;

    /// <summary>
    /// Ekran przy przegranej 
    /// </summary>
    public Image gameOverScreen;

    /// <summary>
    /// Prędkość zaniku efektu krwi
    /// </summary>
    public float fadeSpeed = 0.5f;

    /// <summary>
    /// Menedżer zarządzający muzyką
    /// </summary>
    public AudioSource musicManager;

    public void Awake()
    {
        bloodEffect.color = new Color(bloodEffect.color.r, bloodEffect.color.g, bloodEffect.color.b, 0);
        gameOverScreen.gameObject.SetActive(false);
        musicManager.enabled = true;
    }

    /// <summary>
    /// Funkcja aktualizująca punkty życia gracza po otrzymaniu obrażeń
    /// </summary>
    /// <param name="Damage"> Ilość otrzymanych obrażeń </param>
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
            musicManager.enabled = false;

        }
        healthBar.SetHealth(Health);
    }

    /// <summary>
    /// Funkcja regenerująca życie gracza po kontakcie z odpowienim przedmiotem znajdującym się na mapie
    /// </summary>
    /// <param name="amount"> Ilość punktów życia do zregenerowania </param>
    /// <returns> informację, czy życie zostało zregenerowane </returns>
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

    /// <summary>
    /// Funkcja zwracająca obiekt gracza na scenie
    /// </summary>
    /// <returns> obiekt gracza </returns>
    public Transform GetTransform()
    {
        return transform;
    }

    /// <summary>
    /// Funkcja powodujaca stopniowy zanik efektu krwi w interfejsie uzytkownika
    /// </summary>
    /// <returns> null </returns>
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

    /// <summary>
    /// Funkcja powodujaca restart aktualnego poziomu
    /// </summary>
    public void playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Funkcja powodujaca powrot do glownego menu
    /// </summary>
    public void returnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
