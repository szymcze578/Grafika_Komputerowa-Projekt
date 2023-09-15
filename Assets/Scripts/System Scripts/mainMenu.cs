using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Skrypt menu głównego
/// </summary>
public class mainMenu : MonoBehaviour
{
    /// <summary>
    /// Komponent źródła dźwięku
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// Efekt dźwiękowy kliknięcia w kontrolkę
    /// </summary>
    public AudioClip ClickClip;
    
    
    void Start() {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Funkcja powoduje przejscie do kolejnej sceny
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Funkcja powoduje wyjscie z gry
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Funkcja odtwarza efekt dźwiękowy kliknięcia w kontrolkę
    /// </summary>
    /// <param name="audioSource"> Źródło dźwięku </param>
    public void PlayClickClip(AudioSource audioSource) {
        if(ClickClip != null) {
            audioSource.PlayOneShot(ClickClip);
        }
    }
    
}
