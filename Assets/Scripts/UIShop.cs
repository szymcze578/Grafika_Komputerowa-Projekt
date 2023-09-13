using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using System;

public class UIShop : MonoBehaviour
{
    /*
     * Wskaznik do skryptu weaponSystem
     */
    public WeaponSystem weaponSystem;

    /*
     * Obiekt na mapie stanowiacy sklep
     */
    public GameObject shop;

    /*
     * Pole tekstowe z komunikatami sklepu 
     */
    public Text shopAlerts;

    /*
     * Pole tekstowe z komunikatami interfejsu uzytkownika
     */
    public Text hudAlerts;

    /*
     * Wskaznik do obiektu gracza na scenie
     */
    public GameObject player;

    /*
     * Okno sklepu w interfejsie uzytkownika
     */
    public GameObject canvas;

    /*
     * Przycisk do zakupu karabinu
     */
    public GameObject assaultButton;

    /*
     * Przycisk do zakupu strzelby
     */
    public GameObject shotgunButton;

    /*
     * Przycisk do zakupu amunicji do strzelby
     */
    public GameObject shotgunAmmoButton;

    /*
     * Przycisk do zakupu amunicji do karabinu
     */
    public GameObject assaultAmmoButton;

    /*
     * Przycisk do zakupu amunicji granatu zapalajacego
     */
    public GameObject molotovButton;

    /*
     * Zmienna informujaca czy fala przeciwnikow jest aktywna
     */
    public bool wave = false;

    /*
     * Zmienna informujaca czy okno sklepu jest aktywne
     */
    bool shopActive;

    /*
     * Wskaznik do skryptu gracza
     */
    public Player gracz;

    private AudioSource audioSource;
    public AudioClip BuyWeaponClip;
    public AudioClip BuyAmmoClip;
    public AudioClip OpenShopClip;

    /*
     * Funkcja inicjalizujaca sklep - ustawia odpowiednie zmienne, oraz dezaktywuje okno sklepu w interfejsie uzytkownika
     */
    void Start()
	{
		canvas.SetActive(false);
        Button shotgun = shotgunButton.GetComponent<Button>();
        Button assault = assaultButton.GetComponent<Button>();
        Button shotgunAmmo = shotgunAmmoButton.GetComponent<Button>();
        Button assaultAmmo = assaultAmmoButton.GetComponent<Button>();
        Button molotov = molotovButton.GetComponent<Button>();

        gracz = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        assault.onClick.AddListener(buyAssault);
		shotgun.onClick.AddListener(buyShotgun);
        shotgunAmmo.onClick.AddListener(buyShotgunAmmo);
        assaultAmmo.onClick.AddListener(buyAssaultAmmo);
        molotov.onClick.AddListener(buyMolotov);

        audioSource = gameObject.GetComponent<AudioSource>();
    }

    /*
     * Funkcja, ktora sprawdza odleglosc gracza od sklepu i aktywuje interakcje
     */
    void Update()
    {
		float distance = Vector3.Distance(player.transform.position, shop.transform.position);
		//Debug.Log(gunInShop + " " + distance);

        if (wave == false && distance < 2.0f)
        {
			if (!shopActive)
				hudAlerts.text = "Press E to enter the shop";

			if (Input.GetKeyDown(KeyCode.E))
            {
				shopActive = true;
				hudAlerts.text = "";
				canvas.SetActive(true);
				weaponSystem.blockShooting = false;
                PlayOpenShopClip(audioSource);
			}

			if (canvas.activeSelf && Input.GetKeyDown(KeyCode.Escape))
			{
				shopActive = false;
				canvas.SetActive(false);
				weaponSystem.blockShooting = true;
			}

		}
		else
        {
			shopActive = false;
			hudAlerts.text = "";
			canvas.SetActive(false);
			weaponSystem.blockShooting = true;
		}

    }

    /*
     * Funkcja sluzaca do zakupu karabinu w sklepie
     */
    void buyAssault()
	{
		if(!weaponSystem.weaponLock[1] && gracz.points >= 30)
        {
			weaponSystem.weaponLock[1] = true;
			weaponSystem.magazinesLeft[1] = 1;
			gracz.points -= 30;
            shopAlerts.color = Color.green;
            shopAlerts.text = "You bought assault gun!";
            PlayBuyWeaponClip(audioSource);

		} else if(weaponSystem.weaponLock[1])
        {
            shopAlerts.color = Color.red;
            shopAlerts.text = "You already bought this weapon!";


        } else if (gracz.points < 30)
        {
            shopAlerts.color = Color.red;
            shopAlerts.text = "You don't have enough money!";

        }
		Invoke("ResetShopAlerts", 2.0f);

	}

    /*
     * Funkcja sluzaca do zakupu strzelby w sklepie
     */
    void buyShotgun()
	{
		if (!weaponSystem.weaponLock[2] && gracz.points >= 15)
		{
			weaponSystem.weaponLock[2] = true;
			weaponSystem.magazinesLeft[2] = 1;
			gracz.points -= 15;
            shopAlerts.color = Color.green;

            shopAlerts.text = "You bought shotgun!";
            PlayBuyWeaponClip(audioSource);
		}
		else if (weaponSystem.weaponLock[2])
		{
            shopAlerts.color = Color.red;
            shopAlerts.text = "You already bought this weapon!";
		}
		else if (gracz.points < 15)
		{
            shopAlerts.color = Color.red;
            shopAlerts.text = "You don't have enough money!";
		}

		Invoke("ResetShopAlerts", 2.0f);

	}

    /*
     * Funkcja sluzaca do zakupu amunicji do strzelby w sklepie
     */
    void buyShotgunAmmo()
    {
        if (weaponSystem.weaponLock[2] && gracz.points >= 15)
        {
            weaponSystem.magazinesLeft[2] += 1;
            gracz.points -= 15;
            shopAlerts.color = Color.green;
            shopAlerts.text = "You bought ammo to shotgun!";
            PlayBuyAmmoClip(audioSource);
        }
        else if(!weaponSystem.weaponLock[2])
        {
            shopAlerts.color = Color.red;
            shopAlerts.text = "You don't have shotgun yet!";
        }
		else if (gracz.points < 15)
		{
            shopAlerts.color = Color.red;
            shopAlerts.text = "You don't have enough money!";
        }

        Invoke("ResetShopAlerts", 2.0f);
    }

    /*
     * Funkcja sluzaca do zakupu amunicji do karabinu w sklepie
     */
    void buyAssaultAmmo()
    {
        if (weaponSystem.weaponLock[1] && gracz.points >= 25)
        {
            weaponSystem.magazinesLeft[1] += 1;
            gracz.points -= 25;
			shopAlerts.color = Color.green;
            shopAlerts.text = "You bought ammo to assault!";
            PlayBuyAmmoClip(audioSource);
        }
        else if (!weaponSystem.weaponLock[2])
        {
            shopAlerts.color = Color.red;
            shopAlerts.text = "You don't have assault yet!";
        }
        else if (gracz.points < 25)
        {
			shopAlerts.color = Color.red;
            shopAlerts.text = "You don't have enough money!";
        }

        Invoke("ResetShopAlerts", 2.0f);
    }

    /*
     * Funkcja sluzaca do zakupu granata zapalajacego w sklepie
     */
    void buyMolotov()
    {
        if (gracz.points >= 50)
        {
            weaponSystem.bulletsLeft[3] += 2;
            gracz.points -= 50;
            shopAlerts.color = Color.green;
            shopAlerts.text = "You bought two molotovs!";
            PlayBuyAmmoClip(audioSource);
        } 
        else if (gracz.points < 50)
        {
            shopAlerts.color = Color.red;
            shopAlerts.text = "You don't have enough money!";
        }

        Invoke("ResetShopAlerts", 2.0f);
    }

    /*
     * Funkcja resetujaca pole tekstowe z komunikatami
     */
    void ResetShopAlerts()
    {
        shopAlerts.text = "";
    }

    public void PlayBuyWeaponClip(AudioSource audioSource) {
        if(BuyWeaponClip != null) {
            audioSource.PlayOneShot(BuyWeaponClip);
        }
    }

    public void PlayBuyAmmoClip(AudioSource audioSource) {
        if(BuyAmmoClip != null) {
            audioSource.PlayOneShot(BuyAmmoClip, 0.5f);
        }
    }

    public void PlayOpenShopClip(AudioSource audioSource) {
        if(OpenShopClip != null) {
            audioSource.PlayOneShot(OpenShopClip);
        }
    }
}