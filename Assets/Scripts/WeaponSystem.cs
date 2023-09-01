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

    public WeaponAudioConfig[] audioConfig;
    private AudioSource audioSource;

    public Text ammoDisplay;
    public Text ammoAnimation;
    public Text pointsDisplay;
    public Text hudInfo;
    public Text magazinesLeftUI;

    // 1 - pistol, 2 - assault, 3 - shotgun, 4 - molotov
    public int selectedWeapon = 1; 
    public bool[] weaponLock = { true, false, false, true };


    public int[] magazinesLeft = { 999, 0, 0, 0 };

    public float bulletSpeed = 0.25f;

    private Animator anim;
    public float timeBetweenShoting, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTab;
    public bool allowButtonHold;

    public int[] bulletsLeft = { 10, 30, 15, 0 };
    int bulletsShot, damage;
    bool shooting, reloading, readyToShoot;
    public bool blockShooting = false;

    private bool throwingStance;
    private int previousWeapon;
    private bool readyToThrow;

    [Header("Molotov")]
    public GameObject objectToThrow;
    public float throwForce;
    public float throwUpwardForce;

    Player player;

    //public int playerPoints = 0;


    // Start is called before the first frame update
    void Start()
    {
        anim = transform.root.GetComponent<Animator>();
        SelectWeapon(1);     
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        audioSource = gameObject.transform.GetChild(selectedWeapon-1).gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        SetUpHud();
        pointsDisplay.text = player.points.ToString();
    }

    private void MyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && selectedWeapon != 1) {
            SelectWeapon(1);
        }
            
        if (Input.GetKeyDown(KeyCode.Alpha2) && weaponLock[1] && selectedWeapon != 2){
            SelectWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && weaponLock[2] && selectedWeapon != 3){
            SelectWeapon(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && selectedWeapon != 4 && bulletsLeft[3] > 0){
            SelectWeapon(4);
        }

        if (allowButtonHold)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && selectedWeapon < 4 && bulletsLeft[selectedWeapon - 1] < magazineSize && !reloading && magazinesLeft[selectedWeapon - 1] > 0)
            Reload();

        if (blockShooting && readyToShoot && !throwingStance && shooting && !reloading && bulletsLeft[selectedWeapon - 1] > 0)
        {
            bulletsShot = bulletsPerTab;
            Shoot();
        }

        if (blockShooting && throwingStance && readyToThrow && shooting && !reloading && bulletsLeft[selectedWeapon - 1] > 0)
        {
            bulletsShot = bulletsPerTab;

            bulletsLeft[selectedWeapon - 1]--;
            bulletsShot--;
            readyToThrow = false;
            Throw();
        }

        if (blockShooting && readyToShoot && shooting && !reloading && bulletsLeft[selectedWeapon - 1] <= 0)
        {
            audioConfig[selectedWeapon-1].PlayOutOfAmmoClip(audioSource);
        }
            
    }

    private void Reload()
    {
        reloading = true;
        anim.SetTrigger("reload");
        Invoke("ReloadFinished", 3.0f/reloadTime);
        audioConfig[selectedWeapon-1].PlayReloadingClip(audioSource);
    }

    private void Throw()
    {
        readyToThrow = false;
        Debug.Log("Throw");
        anim.SetTrigger("throw");

        

        Invoke("ThrowBegin", 0.5f);
        Invoke("ThrowEnd", 1.2f);
        audioConfig[selectedWeapon-1].PlayShootingClip(audioSource);
    }

    private void ThrowBegin()
    {
        GameObject projectile = Instantiate(objectToThrow, transform.GetChild(3).transform.position,  transform.GetChild(3).transform.rotation);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        Vector3 forceToAdd = bulletSpawnPoint.transform.right * throwForce;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        transform.GetChild(selectedWeapon-1).gameObject.SetActive(false);
    }
    private void ThrowEnd()
    {
        Debug.Log("Throw End");
        throwingStance = false;
        readyToShoot = true;
        SelectWeapon(previousWeapon);
    }

    private void Shoot()
    {
        readyToShoot = false;
        audioConfig[selectedWeapon-1].PlayShootingClip(audioSource);

        //var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation);
        //bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.transform.forward * bulletSpeed;

        if (selectedWeapon == 3)
        {
            if (Physics.Raycast(bulletSpawnPoint.transform.position, Quaternion.Euler(0f, -15f, 0f) * bulletSpawnPoint.transform.forward, out RaycastHit hit, float.MaxValue, Mask))
            {
                TrailRenderer trail = Instantiate(BulletTrail, bulletSpawnPoint.transform.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
            }
            if (Physics.Raycast(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.forward, out RaycastHit hit2, float.MaxValue, Mask))
            {
                TrailRenderer trail2 = Instantiate(BulletTrail, bulletSpawnPoint.transform.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail2, hit2));
            }
            if (Physics.Raycast(bulletSpawnPoint.transform.position, Quaternion.Euler(0f, 15f, 0f) * bulletSpawnPoint.transform.forward, out RaycastHit hit3, float.MaxValue, Mask))
            {
                TrailRenderer trail3 = Instantiate(BulletTrail, bulletSpawnPoint.transform.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail3, hit3));
            }

            bulletsLeft[selectedWeapon - 1] -= 3;
            bulletsShot -= 3;
            Invoke("ResetShot", timeBetweenShoting);

            if (bulletsShot > 0 && bulletsLeft[selectedWeapon - 1] > 0)
                Invoke("Shoot", timeBetweenShots);
        }
        else
        {
            if (Physics.Raycast(bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.forward, out RaycastHit hit, float.MaxValue, Mask))
            {
                TrailRenderer trail = Instantiate(BulletTrail, bulletSpawnPoint.transform.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, hit));
            }

            bulletsLeft[selectedWeapon - 1]--;
            bulletsShot--;
            Invoke("ResetShot", timeBetweenShoting);

            if (bulletsShot > 0 && bulletsLeft[selectedWeapon - 1] > 0)
                Invoke("Shoot", timeBetweenShots);

        }

       
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        Vector3 startPosition = Trail.transform.position;

        float distance = Vector3.Distance(Trail.transform.position, Hit.point);
        float startingDistance = distance;

        while (distance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, Hit.point, 1 - (distance / startingDistance));
            distance -= Time.deltaTime * bulletSpeed;

            yield return null;
        }
        Trail.transform.position = Hit.point;
        if(Hit.collider.CompareTag("Enemy")) {
            var enemy = Hit.rigidbody.GetComponent<Enemy>();
            enemy.TakeDamage(damage);
            Instantiate(FleshImpactParticleSystem, Hit.point, Quaternion.LookRotation(Hit.normal));
            if (enemy.Health <= 0) {
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

    private void ResetThrow()
    {
        readyToThrow = true;
    }

    private void ReloadFinished()
    {
        bulletsLeft[selectedWeapon - 1] = magazineSize;
        reloading = false;
        magazinesLeft[selectedWeapon - 1]--;
        
    }

    void SelectWeapon(int weaponIndex)
    {
        previousWeapon = selectedWeapon;
        transform.GetChild(selectedWeapon-1).gameObject.SetActive(false);
        transform.GetChild(weaponIndex-1).gameObject.SetActive(true);

        audioSource = gameObject.transform.GetChild(weaponIndex-1).gameObject.GetComponent<AudioSource>();
        selectedWeapon = weaponIndex;
        anim.SetInteger("weapon", weaponIndex);
        bulletSpawnPoint.transform.position = transform.GetChild(selectedWeapon-1).position;
        bulletSpawnPoint.transform.rotation = transform.GetChild(selectedWeapon-1).rotation;

        audioConfig[selectedWeapon-1].PlayEquipGunClip(audioSource);

        //Change weapon stats
        switch(weaponIndex)
        {
            case 1:
                bulletSpawnPoint.transform.Translate(0.0f, 0.08f, 0.10f);
                readyToShoot = true;
                allowButtonHold = false;
                shooting = false;
                reloading = false;

                bulletsPerTab = 1;
                magazineSize = 10;
                damage = 20;

                timeBetweenShoting = 0.2f;
                timeBetweenShots = 0.1f;
                reloadTime = 2f; // 3s / 2 = 1.5s
                throwingStance = false;
                readyToThrow = false;
                break;

            case 2:
                bulletSpawnPoint.transform.Translate(0.0f, 0.13f, 0.7f);
                readyToShoot = true;
                allowButtonHold = true;
                shooting = false;
                reloading = false;

                bulletsPerTab = 3;
                magazineSize = 30;
                damage = 30;

                timeBetweenShoting = 0.1f;
                timeBetweenShots = 0.1f;
                reloadTime = 1f; // 3s / 1
                throwingStance = false;
                readyToThrow = false;
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

                timeBetweenShoting = 1f;
                reloadTime = 0.7f; // 3s / 0.7
                throwingStance = false;
                readyToThrow = false;
                break;

            case 4:
                Invoke("ResetThrow", 1.15f);
                bulletSpawnPoint.transform.Translate(0.0f, 0.13f, 0.6f);
                allowButtonHold = false;
                shooting = false;
                reloading = false;
                damage = 50;

                bulletsPerTab = 1;
                magazineSize = 3;

                timeBetweenShoting = 1f;
                reloadTime = 0.7f; // 3s / 0.7
                throwingStance = true;
                readyToShoot = false;
                break;
        }

        
        anim.SetFloat("reloadTime", reloadTime);

    }

    void SetUpHud()
    {

        if (bulletsLeft[selectedWeapon - 1] < 0.5 * magazineSize)
            hudInfo.text = "Press R to reload";

        if (selectedWeapon == 1)
            magazinesLeftUI.text = "∞";
        else if (selectedWeapon == 4)
            magazinesLeftUI.text = "";
        else
            if(magazinesLeft[selectedWeapon - 1] > 5)
                magazinesLeftUI.text = "+" + string.Concat(Enumerable.Repeat("X", 5));
            else
                magazinesLeftUI.text = string.Concat(Enumerable.Repeat("X", magazinesLeft[selectedWeapon - 1]));

        if (selectedWeapon == 4)
            ammoDisplay.text = bulletsLeft[selectedWeapon - 1].ToString();
        else
            ammoDisplay.text = bulletsLeft[selectedWeapon - 1] + "/" + magazineSize;
        ammoAnimation.text = string.Concat(Enumerable.Repeat("I", bulletsLeft[selectedWeapon - 1]));

        

    }
}
