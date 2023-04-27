using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public int selectedWeapon = 1; // 1 - pistol, 2 - assault, 3 - shotgun
    public Transform bulletSpawnPoint = null;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon(1);
        bulletSpawnPoint = transform.GetChild(selectedWeapon-1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bulletSpawnPoint.forward * bulletSpeed;
            
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
            SelectWeapon(1);
        if(Input.GetKeyDown(KeyCode.Alpha2))
            SelectWeapon(2);
        if(Input.GetKeyDown(KeyCode.Alpha3))
            SelectWeapon(3);
    }

    void SelectWeapon(int weaponIndex)
    {
        transform.GetChild(selectedWeapon-1).gameObject.SetActive(false);
        transform.GetChild(weaponIndex-1).gameObject.SetActive(true);
        selectedWeapon = weaponIndex;
        bulletSpawnPoint = transform.GetChild(selectedWeapon-1);
    }
}
