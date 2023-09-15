using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

/// <summary>
/// System broni
/// </summary>
public class WeaponSystem : MonoBehaviour
{
    /// <summary>
    /// Obiekt na scenie reprezentujący miejsce wystrzału pocisku z broni
    /// </summary>
    [SerializeField]
    private GameObject bulletSpawnPoint = null;

    /// <summary>
    /// System cząsteczek dla trafienia w ściane lub mebel
    /// </summary>
    [SerializeField]
    private ParticleSystem ImpactParticleSystem;

    /// <summary>
    /// System cząsteczek dla trafienia we wroga
    /// </summary>
    [SerializeField]
    private ParticleSystem FleshImpactParticleSystem;

    /// <summary>
    /// Renderer smugi pocisku
    /// </summary>
    [SerializeField]
    private TrailRenderer BulletTrail;

    /// <summary>
    /// Maska ignorująca inne obiekty niż obiekt przeciwnika
    /// </summary>
    [SerializeField]
    private LayerMask Mask;

    /// <summary>
    /// Tablica z konfiguratorami dźwięku dla poszczególnych broni
    /// </summary>
    public WeaponAudioConfig[] audioConfig;

    /// <summary>
    /// Źródło dźwięku dla efektów dźwiękowych broni
    /// </summary>
    private AudioSource audioSource;

    /// <summary>
    /// Pole tekstowe wyswietlajace aktualny stan amunicji
    /// </summary>
    public Text ammoDisplay;

    /// <summary>
    /// Pole tekstowe wyswietlajace aktualny stan amunicji
    /// </summary>
    public Text ammoAnimation;

    /// <summary>
    /// Pole tekstowe wyswietlajace punkty gracza
    /// </summary>
    public Text pointsDisplay;

    /// <summary>
    /// Pole tekstowe wyswietlajace informacje w interfejsie uzytkownika
    /// </summary>
    public Text hudInfo;

    /// <summary>
    /// Pole tekstowe wyswietlajace liczbe posiadanych magazynkow z amunicja
    /// </summary>
    public Text magazinesLeftUI;

    /// <summary>
    /// Zmienna, ktora wskazuje na aktualnie wybraną broń (1 - pistolet, 2 - karabin, 3 - strzelba, 4 - granat zapalający)
    /// </summary>
    public int selectedWeapon = 1;

    /// <summary>
    /// Zmienna tablicowa okreslajaca, ktore bronie sa odblokowane dla gracza
    /// </summary>
    public bool[] weaponLock = { true, false, false, false };

    /// <summary>
    /// Zmienna tablicowa okreslajaca liczbe pozostalych magazynkow dla kazdej broni
    /// </summary>
    public int[] magazinesLeft = { 999, 0, 0, 0 };

    /// <summary>
    /// Zmienna okreslajaca predkosc pocisku
    /// </summary>
    public float bulletSpeed = 0.25f;

    /// <summary>
    /// Kontroler animacji
    /// </summary>
    private Animator anim;

    /// <summary>
    /// Zmienna okreslajaca czas miedzy kolejnymi strzalami
    /// </summary>
    public float timeBetweenShoting;

    /// <summary>
    /// Zmienna okreslajaca czas przeladowania broni
    /// </summary>
    public float reloadTime;

    /// <summary>
    /// Zmienna okreslajaca czas miedzy kolejnymi strzalami
    /// </summary>
    public float timeBetweenShots;

    /// <summary>
    /// Zmienna okreslajaca pojemnosc magazynku
    /// </summary>
    public int magazineSize;

    /// <summary>
    /// Zmienna okreslajaca liczbe wystrzelonych kul po jednym przycisnieciu LPM
    /// </summary>
    public int bulletsPerTab;

    /// <summary>
    /// Zmienna okreslajaca, czy gracz moze przytrzymac LPM aby prowadzic ogien ciagly
    /// </summary>
    public bool allowButtonHold;

    /// <summary>
    /// Zmienna tablicowa okreslajaca liczbe pozostalych kul w magazynku
    /// </summary>
    public int[] bulletsLeft = { 10, 30, 15, 2 };

    /// <summary>
    /// Zmienna okreslajaca ile kul zostalo juz wystrzelonych po wcisnieciu LPM
    /// </summary>
    int bulletsShot;

    /// <summary>
    /// Zmienna okreslajaca liczbe obrazen, ktore zadaje dana bron
    /// </summary>
    int damage;

    /// <summary>
    /// Zmienna okreslajaca czy gracz strzela
    /// </summary>
    bool shooting;

    /// <summary>
    /// Zmienna okreslajaca czy przeladowanie jest w toku
    /// </summary>
    bool reloading;

    /// <summary>
    /// Zmienna okreslajaca czy bron jest gotowa do wystrzalu
    /// </summary>
    bool readyToShoot;

    /// <summary>
    /// Zmienna okreslajaca czy bron jest zablokowana
    /// </summary>
    public bool blockShooting = false;

    /// <summary>
    /// Zmienna okreslajaca czy gracz trzyma granat
    /// </summary>
    private bool throwingStance;

    /// <summary>
    /// Zmienna określająca jaka broń była używana przed wyjęciem granatu
    /// </summary>
    private int previousWeapon;

    /// <summary>
    /// Zmienna określająca, czy gracz może już rzucić granat
    /// </summary>
    private bool readyToThrow;

    /// <summary>
    /// Renderer celownika dla rzutu granatem
    /// </summary>
    [SerializeField]
    private LineRenderer lineRenderer;

    /// <summary>
    /// Ilość punktów na renderowanym celowniku (definiuje gładkość krzywej)
    /// </summary>
    private int LinePoints = 25;

    /// <summary>
    /// Odległość między punktami na celowniku (definiuje gładkość krzywej)
    /// </summary>
    private float TimeBetweenPoints = 0.1f;

    /// <summary>
    /// Model granatu
    /// </summary>
    [Header("Molotov")]
    public GameObject objectToThrow;

    /// <summary>
    /// Zmienna definiująca siłę poziomą rzutu granatem
    /// </summary>
    private float throwForce;

    /// <summary>
    /// Zmienna definiująca siłę pionową rzutu granatem
    /// </summary>
    public float throwUpwardForce;

    /// <summary>
    /// Skrypt gracza
    /// </summary>
    Player player;
    
    /// <summary>
    /// Metoda inicjalizujący i ustawiające odpowiednie komponenty po rozpoczęciu rozgrywki
    /// </summary>
    void Start()
    {
        anim = transform.root.GetComponent<Animator>();
        SelectWeapon(1);     
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        audioSource = gameObject.transform.GetChild(selectedWeapon-1).gameObject.GetComponent<AudioSource>();
    }

    /// <summary>
    /// Metoda pobiera na bieżąco klawisze naciskane przez gracza związane z obsługą broni (zmiana broni, strzelanie)
    /// </summary>
    void Update()
    {
        MyInput();
        SetUpHud();
        pointsDisplay.text = player.points.ToString();

        if(selectedWeapon == 4 && readyToThrow) {
            CalculateThrowForce();
            DrawProjection();
        }   
        else {
            lineRenderer.enabled = false;
        }
    
    }

    /// <summary>
    /// Funkcja odczytujaca klawisze wcisniete przez gracza
    /// </summary>
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

        if (blockShooting && readyToShoot && Input.GetKeyDown(KeyCode.Mouse0) && !reloading && bulletsLeft[selectedWeapon - 1] <= 0)
        {
            audioConfig[selectedWeapon-1].PlayOutOfAmmoClip(audioSource);
        }
            
    }

    /// <summary>
    /// Funkcja odpowiadajaca za przeladowanie broni
    /// </summary>
    private void Reload()
    {
        reloading = true;
        anim.SetTrigger("reload");
        Invoke("ReloadFinished", 3.0f/reloadTime);
        audioConfig[selectedWeapon-1].PlayReloadingClip(audioSource);
    }

    /// <summary>
    /// Funkcja oblicza siłę rzutu
    /// </summary>
    private void CalculateThrowForce()
    {
        Vector3 center = Vector3.zero;
        center.x += Screen.width/2;
        center.y += Screen.height/2;
        Vector3 mousePos = Input.mousePosition - center;
        throwForce = mousePos.magnitude / 55;
        //throwUpwardForce = 5;
    }

    /// <summary>
    /// Funkcja oblicza krzywą wyznaczającą trajektorie rzutu granatem
    /// </summary>
    private void DrawProjection()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt(LinePoints / TimeBetweenPoints) + 1;

        Vector3 startPosition = transform.GetChild(3).transform.position;
        Vector3 startVelocity = (throwForce*transform.root.forward + transform.root.up * (throwUpwardForce+1)) / objectToThrow.GetComponent<Rigidbody>().mass;

        int i = 0;
        lineRenderer.SetPosition(i, startPosition);
        for(float time = 0; time < LinePoints; time += TimeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time * startVelocity;
            point.y = startPosition.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
            lineRenderer.SetPosition(i, point);
        }

    }

    /// <summary>
    /// Metoda odpowiadająca za rzut granatem
    /// </summary>
    private void Throw()
    {
        readyToThrow = false;
        Debug.Log("Throw");
        anim.SetTrigger("throw");

        

        Invoke("ThrowBegin", 0.5f);
        Invoke("ThrowEnd", 1.2f);
        audioConfig[selectedWeapon-1].PlayShootingClip(audioSource);
    }

    /// <summary>
    /// Metoda odpowiadająca za wyciągnięcie granatu i przygotowanie do rzutu
    /// </summary>
    private void ThrowBegin()
    {
        GameObject projectile = Instantiate(objectToThrow, transform.GetChild(3).transform.position,  transform.GetChild(3).transform.rotation);
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        Vector3 forceToAdd = transform.root.forward * throwForce + transform.root.up * throwUpwardForce;
        projectileRb.AddForce(forceToAdd, ForceMode.Impulse);

        transform.GetChild(selectedWeapon-1).gameObject.SetActive(false);
    }

    /// <summary>
    /// Metoda powoduje wyciągnięcie broni używanej przed użyciem granatu
    /// </summary>
    private void ThrowEnd()
    {
        Debug.Log("Throw End");
        throwingStance = false;
        readyToShoot = true;
        SelectWeapon(previousWeapon);
    }

    /// <summary>
    /// Funkcja odpowiadająca za strzelanie
    /// </summary>
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

    /// <summary>
    /// Funkcja renderująca smugę za pociskiem
    /// </summary>
    /// <param name="Trail"> Renderer smugi pocisku </param>
    /// <param name="Hit"> Informacja o tym gdzie powinien trafić pocisk </param>
    /// <returns></returns>
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

    /// <summary>
    /// Funkcja ustawiajaca gotowosc broni do ponownego wystrzalu
    /// </summary>
    private void ResetShot()
    {
        readyToShoot = true;
    }

    /// <summary>
    /// Funkcja ustawiajaca gotowosc do ponownego rzutu granatem
    /// </summary>
    private void ResetThrow()
    {
        readyToThrow = true;
    }

    /// <summary>
    /// Funkcja informujaca o zakonczeniu procedury przeladowania broni
    /// </summary>
    private void ReloadFinished()
    {
        bulletsLeft[selectedWeapon - 1] = magazineSize;
        reloading = false;
        magazinesLeft[selectedWeapon - 1]--;
        
    }

    /// <summary>
    /// Funkcja sluzaca do przelaczania broni
    /// </summary>
    /// <param name="weaponIndex"> Index broni, która powinna zostać wyciągnięta </param>
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
                Cursor.visible = true;
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
                Cursor.visible = true;
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
                Cursor.visible = true;
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
                Cursor.visible = false;
                break;
        }

        
        anim.SetFloat("reloadTime", reloadTime);

    }

    /// <summary>
    /// Funkcja ustawiajaca kontrolki tekstowe w interfejsie uzytkownika
    /// </summary>
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
