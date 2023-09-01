using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    public int damage;
    public float radius;
    public float time;
    private AudioSource asource;
    public AudioClip startClip;
    public AudioClip loopClip;
    private ParticleSystem ps;

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        
    }

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
    void StopEffect() {
        asource.Stop();
        Destroy(gameObject);
    }

}
