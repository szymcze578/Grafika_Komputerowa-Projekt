using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class MolotovExplosion : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider cc;

    public GameObject fireEffect;
    public AudioClip explosionClip;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        rb.detectCollisions = false;
        Instantiate(fireEffect, transform.position, fireEffect.transform.rotation);
        Destroy(gameObject);
    }
}
