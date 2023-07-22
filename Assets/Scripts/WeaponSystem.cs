using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class WeaponSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletSpawnPoint = null;
    //public GameObject bulletPrefab;
    [SerializeField]
    private ParticleSystem ShootingSystem;

    [SerializeField]
    private ParticleSystem ImpactParticleSystem;

    [SerializeField]
    private ParticleSystem FleshImpactParticleSystem;

    [SerializeField]
    private TrailRenderer BulletTrail;

    [SerializeField]
    private LayerMask Mask;



    public Text ammoDisplay;
    public Text ammoAnimation;
    public Text pointsDisplay;
    public Text hudInfo;

    public int selectedWeapon = 1; // 1 - pistol, 2 - assault, 3 - shotgun
    public bool[] weaponLock = { true, false, false };

    public float bulletSpeed = 10;

    private Animator anim;
    public float timeBetweenShoting, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTab;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot, damage;
    bool shooting, reloading, readyToShoot;
    public bool blockShooting = false;

    Player player;

    //public int playerPoints = 0;


    // Start is called before the first frame update
    void Start()
    {
        anim = transform.root.GetComponent<Animator>();
        SelectWeapon(1);     
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        ammoDisplay.text = bulletsLeft + "/" + magazineSize;
        ammoAnimation.text = string.Concat(Enumerable.Repeat("I", bulletsLeft));
        pointsDisplay.text = player.points.ToString() + " $";
        if (bulletsLeft < 0.5*magazineSize)
            hudInfo.text = "Press R to reload";

    }

    private void MyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && selectedWeapon != 1) 
            SelectWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha2) && weaponLock[1] && selectedWeapon != 2)
            SelectWeapon(2);
        if (Input.GetKeyDown(KeyCode.Alpha3) && weaponLock[2] && selectedWeapon != 3)
            SelectWeapon(3);

        if (allowButtonHold)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
            Reload();

        if (blockShooting && readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTab;
            Shoot();
        }
            
    }

    private void Reload()
    {
        reloading = true;
        anim.SetTrigger("reload");
        Invoke("ReloadFinished", 3.0f/reloadTime);
    }

    private void Shoot()
    {
        readyToShoot = false;

        //var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        //bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.transform.forward * bulletSpeed;

        if(Physics.Raycast(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.forward, out RaycastHit hit, float.MaxValue, Mask)) {
            TrailRenderer trail = Instantiate(BulletTrail, bulletSpawnPoint.transform.position, Quaternion.identity);

            StartCoroutine(SpawnTrail(trail, hit));

        }

        bulletsLeft--;
        bulletsShot--;
        Invoke("ResetShot", timeBetweenShoting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        float time = 0;
        Vector3 startPosition = Trail.transform.position;

        while(time < 1) {
            Trail.transform.position = Vector3.Lerp(startPosition, Hit.point, time);
            time += Time.deltaTime / Trail.time;
            yield return null;
        }
        Trail.transform.position = Hit.point;
        if(Hit.collider.CompareTag("Enemy")) {
                //Instantiate(FleshImpactParticleSystem, Hit.point, Quaternion.LookRotation(Hit.normal));
                var enemy = Hit.rigidbody.GetComponent<Enemy>();
                enemy.TakeDamage(damage);
                if(enemy.Health <= 0) {
                    player.points += 10;
                }
            }
        else {
            Instantiate(ImpactParticleSystem, Hit.point, Quaternion.LookRotation(Hit.normal));
        }
        
        Destroy(Trail.gameObject, Trail.time);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    void SelectWeapon(int weaponIndex)
    {
        transform.GetChild(selectedWeapon-1).gameObject.SetActive(false);
        transform.GetChild(weaponIndex-1).gameObject.SetActive(true);
        selectedWeapon = weaponIndex;
        anim.SetInteger("weapon", weaponIndex);
        bulletSpawnPoint.transform.position = transform.GetChild(selectedWeapon-1).position;
        bulletSpawnPoint.transform.rotation = transform.GetChild(selectedWeapon-1).rotation;

        //Change weapon stats
        switch(weaponIndex)
        {
            case 1:
                bulletSpawnPoint.transform.Translate(0.0f, 0.08f, 0.2f);
                readyToShoot = true;
                allowButtonHold = false;
                shooting = false;
                reloading = false;

                bulletsPerTab = 1;
                magazineSize = 10;
                bulletsLeft = 10;
                damage = 20;

                timeBetweenShoting = 0.2f;
                timeBetweenShots = 0.1f;
                reloadTime = 2f; // 3s / 2 = 1.5s
                break;

            case 2:
                bulletSpawnPoint.transform.Translate(0.0f, 0.13f, 0.7f);
                readyToShoot = true;
                allowButtonHold = true;
                shooting = false;
                reloading = false;

                bulletsPerTab = 3;
                magazineSize = 30;
                bulletsLeft = 30;
                damage = 30;

                timeBetweenShoting = 0.1f;
                timeBetweenShots = 0.1f;
                reloadTime = 1f; // 3s / 1
                break;

            case 3:
                bulletSpawnPoint.transform.Translate(0.0f, 0.13f, 0.6f);
                readyToShoot = true;
                allowButtonHold = false;
                shooting = false;
                reloading = false;
                damage = 50;

                bulletsPerTab = 1;
                magazineSize = 15;
                bulletsLeft = 15;

                timeBetweenShoting = 1f;
                reloadTime = 0.7f; // 3s / 0.7
                break;
        }
           anim.SetFloat("reloadTime", reloadTime); 
    }
}
