using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Skrypt zadawania obrażeń od ognia
/// </summary>
public class DamageOverTime : MonoBehaviour
{
    /// <summary>
    /// Zmienna definiująca ilość obrażeń na sekundę
    /// </summary>
    public int damage;

    /// <summary>
    /// Zmienna definiująca w jakim promieniu zadawane są obrażenia
    /// </summary>
    public float radius;

    /// <summary>
    /// Zmienna definiująca czas trwania ognia
    /// </summary>
    public float time;

    /// <summary>
    /// Komponent źródła dźwięku
    /// </summary>
    private AudioSource asource;

    /// <summary>
    /// Efekt dźwiękowy rozpoczęcia palenia się
    /// </summary>
    public AudioClip startClip;

    /// <summary>
    /// Zapętlony efekt dźwiękowy palenia się
    /// </summary>
    public AudioClip loopClip;

    /// <summary>
    /// System cząsteczek ognia
    /// </summary>
    private ParticleSystem ps;

    /// <summary>
    /// Metoda zapala obszar w miejscu wybuchu
    /// </summary>
    void Start()
    {   
        ps = GetComponent<ParticleSystem>();
        ps.Stop(); // Cannot set duration whilst particle system is playing
 
        var main = ps.main;
        main.duration = time;
 
        ps.Play();

        asource = GetComponent<AudioSource>();
        asource.PlayOneShot(startClip);
        //asource.loop = true;
        //asource.PlayOneShot(loopClip);
        
        InvokeRepeating("Dps", 0f, 1f);
        Invoke("StopEffect", time);
    }

    /// <summary>
    /// Metoda zadaje obrażenia czasowe wszystkim przeciwnikom wewnątrz ognia
    /// </summary>
    void Dps() {
    // QueryTriggerInteraction.Collide might be needed
    Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, radius);

    foreach(Collider col in hitColliders) {
        if(col.gameObject.CompareTag("Enemy")) {
            col.gameObject.GetComponent<Enemy>().TakeDamage(damage);
        }
        else if(col.gameObject.CompareTag("Player")) {
            //col.gameObject.GetComponent<Player>().TakeDamage(damage/2);
        }
    }
    }

    /// <summary>
    /// Metoda gasi ogień
    /// </summary>
    void StopEffect() {
        asource.Stop();
        Destroy(gameObject);
    }

}
