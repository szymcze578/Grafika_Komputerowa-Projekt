using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{

    public float life = 3;
    public WeaponSystem gunHolder = null;
    public Player player;

    private void Awake()
    {
        gunHolder = GameObject.Find("GunHolder").GetComponent<WeaponSystem>();
        player = gameObject.GetComponent<Player>();
        Destroy(gameObject, life);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            player.points += 10;
            Destroy(collision.gameObject);   
        }

        Destroy(gameObject);
    }

}
