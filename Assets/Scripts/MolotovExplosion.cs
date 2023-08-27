using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MolotovExplosion : MonoBehaviour
{
    public float damagePerSecond;
    public float burningTime;
    public float burningSize;
    private Rigidbody rb;
    private CapsuleCollider cc;
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
        //fire effect
        //rb.detectCollisions = false;
    }
}
