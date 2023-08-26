using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{

    private AudioSource audioSource;
    public AudioClip ClickClip;
    
    
    void Start() {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayClickClip(AudioSource audioSource) {
        if(ClickClip != null) {
            audioSource.PlayOneShot(ClickClip);
        }
    }
    
}
