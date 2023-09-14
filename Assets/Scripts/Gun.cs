using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public Transform bulletSpawnPoint = null;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;

    void Start()
    {
        bulletSpawnPoint = transform.GetChild(1);
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;      
        }

    }
}
