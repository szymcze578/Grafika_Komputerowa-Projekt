using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSystem : MonoBehaviour
{
    public Transform bulletSpawnPoint = null;
    public GameObject bulletPrefab;

    public Text ammoDisplay;
    public Text pointsDisplay;

    public int selectedWeapon = 1; // 1 - pistol, 2 - assault, 3 - shotgun
    
    public float bulletSpeed = 10;

    public float timeBetweenShoting, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTab;
    public bool allowButtonHold;

    int bulletsLeft, bulletsShot;
    bool shooting, reloading, readyToShoot;

    public int playerPoints = 0;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon(1);
     
        bulletSpawnPoint = transform.GetChild(selectedWeapon-1);
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        ammoDisplay.text = bulletsLeft + "/" + magazineSize;
        pointsDisplay.text = playerPoints.ToString();
        
    }

    private void MyInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SelectWeapon(1);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SelectWeapon(2);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            SelectWeapon(3);

        if (allowButtonHold)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
            Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTab;
            Shoot();
        }
            
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void Shoot()
    {
        readyToShoot = false;

        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;

        bulletsLeft--;
        bulletsShot--;
        Invoke("ResetShot", timeBetweenShoting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
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
        bulletSpawnPoint = transform.GetChild(selectedWeapon-1);

        //Change weapon stats
        switch(weaponIndex)
        {
            case 1:
                readyToShoot = true;
                allowButtonHold = false;
                shooting = false;
                reloading = false;

                bulletsPerTab = 1;
                magazineSize = 10;
                bulletsLeft = 10;

                timeBetweenShoting = 0.2f;
                timeBetweenShots = 0.1f;
                reloadTime = 1f;
                break;

            case 2:
                readyToShoot = true;
                allowButtonHold = true;
                shooting = false;
                reloading = false;

                bulletsPerTab = 3;
                magazineSize = 30;
                bulletsLeft = 30;

                timeBetweenShoting = 0.1f;
                timeBetweenShots = 0.1f;
                reloadTime = 2f;
                break;

            case 3:
                readyToShoot = true;
                allowButtonHold = false;
                shooting = false;
                reloading = false;

                bulletsPerTab = 1;
                magazineSize = 15;
                bulletsLeft = 15;

                timeBetweenShoting = 1f;
                reloadTime = 3f;
                break;
        }
            
    }
}
