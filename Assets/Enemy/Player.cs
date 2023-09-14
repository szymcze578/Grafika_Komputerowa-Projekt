using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDamageable
{
    //punkty �ycia gracza
    [SerializeField]
    private int Health = 100;
    //punkty(pieni�dz) kt�re gracz zdobywa podczas rozgrzewki 
    public int points = 0;

    public HealthBar healthBar;

    public Image bloodEffect;
    public Image gameOverScreen;
    public Image winScreen;
    public float fadeSpeed = 0.5f;

    public AudioSource musicManager;

    public void Awake()
    {
        bloodEffect.color = new Color(bloodEffect.color.r, bloodEffect.color.g, bloodEffect.color.b, 0);
        gameOverScreen.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(false);
        musicManager.enabled = true;
    }

    // Funkcja aktualizuj�ca punkty �ycia gracza po otwymaniu obra�e�
    // Damage - ilo�� otrzymanych obra�e�
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

    // Funkcja zwi�kszaj�ca �ycie gracza po kontakcie z odpowienim przedmiotem znajduj�cym si� na mapie
    // amount - ilo�� �ycia do zwi�kszenia
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

    /*
     * Funkcja powodujaca stopniowy zanik efektu krwi w interfejsie uzytkownika
     */
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

    /*
     * Funkcja powodujaca restart aktualnego poziomu
     */
    public void playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /*
     * Funkcja powodujaca powrot do glownego menu
     */
    public void returnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
