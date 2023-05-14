using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{

    public float life = 3;
    public WeaponSystem gunHolder = null;

    private void Awake()
    {
        gunHolder = GameObject.Find("GunHolder").GetComponent<WeaponSystem>();
        Destroy(gameObject, life);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gunHolder.playerPoints += 10;
            Destroy(collision.gameObject);   
        }

        Destroy(gameObject);
    }

}
